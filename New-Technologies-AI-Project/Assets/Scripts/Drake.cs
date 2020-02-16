using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

//1. Roam around within given range
//2. Warrior is detected
//3. Follow warrior
//4a.Catch warrior > restart game
//4b.Lose warrior (vision obscured) go to last warrior position
//5b.Roam new position for x seconds
//6b.Return to start position

public class Drake : MonoBehaviour
{
    public enum ResponseType
    {
        Chase,
        Alert
    }

    public float roamingDistance;
    public float sightDistance;
    public float alertDistance;
    
    private Vector3 startPosition;

    public ResponseType response;

    private float baseSpeed;

    private bool chasing;

    public float maxPatience;
    private float patience;

    private NavMeshAgent Agent
    {
        get { return GetComponent<NavMeshAgent>(); }
    }

    private Animator Animator
    {
        get { return GetComponent<Animator>(); }
    }

    private SphereCollider SphereCollider
    {
        get { return GetComponent<SphereCollider>(); }
    }
    
    void Awake()
    {
        baseSpeed = Agent.speed;

        startPosition = transform.position;
        patience = maxPatience;

        InitializeCollider();

        //animator.keepAnimatorControllerStateOnDisable = true;
    }

    public void ResetDrake()
    {
        transform.position = startPosition;

        Agent.isStopped = true;
        Agent.ResetPath();
    }

    private void InitializeCollider()
    {
        SphereCollider.radius = sightDistance;
    }

    void Update()
    {
        if(chasing)
        {
            Agent.speed = baseSpeed * 1.5f;

            var distance = Vector3.Distance(gameObject.transform.position, GameManager.warrior.transform.position);

            if (distance < 3f)
            {
                //GameManager.warrior.caught = true;
                StartCoroutine(GameManager.gameManager.ResetGame());
            }
                
        } else {

            Agent.speed = baseSpeed;
        }

        if(chasing && GameManager.warrior.caught)
            ReturnToStartPosition();
        
        //Set to 0.1 in case agent gets stuck
        bool walking = Agent.velocity.x > 0.1f || Agent.velocity.z > 0.1f;

        if(!walking)
            TestPatience();
        
        Animator.SetBool("IsWalking", walking);
    }

    private void TestPatience()
    {
        patience -= 1 * Time.deltaTime;

        if (patience > 0) return;

        if (chasing)
            ReturnToStartPosition();
        else
            ChangeDestination();
    }

    private void ReturnToStartPosition()
    {
        Agent.destination = startPosition;

        chasing = false;
    }

    private void ChangeDestination()
    {
        Agent.destination = new Vector3(startPosition.x + Random.Range(-roamingDistance, roamingDistance), 
                                        transform.position.y,
                                        startPosition.z + Random.Range(-roamingDistance, roamingDistance));

        ResetPatience();
    }

    private void OnTriggerStay(Collider other)
    {
        if (GameManager.warrior.caught) return;

        if (other.gameObject == GameManager.warrior.gameObject)
        {
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            if(angle < 90 * 0.5f)
            {
                RaycastHit hit;

                if(Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, sightDistance))
                {
                    if(hit.collider.gameObject == GameManager.warrior.gameObject)
                    {
                        switch(response)
                        {
                            case ResponseType.Chase:

                                Agent.destination = GameManager.warrior.transform.position;

                                StartChase();
                                break;

                            case ResponseType.Alert:
                                AlertOthers();
                                break; 
                        }
                    }
                }
            }
        }
    }

    private void AlertOthers()
    {
        var others = GameObject.FindGameObjectsWithTag("Drake").ToList();

        others.ForEach(x =>
        {
            var distance = Vector3.Distance(gameObject.transform.position, x.transform.position);

            if (distance <= alertDistance)
                x.GetComponent<NavMeshAgent>().destination = GameManager.warrior.transform.position;
        });
    }

    private void StartChase()
    {
        if (chasing) return;

        ResetPatience();

        chasing = true;
    }

    private void ResetPatience()
    {
        patience = maxPatience;
    }
}
