using UnityEngine;
using UnityEngine.AI;

public class Warrior : MonoBehaviour, IPlayable
{
    private Vector3 startPosition;

    public GameObject character;
    public GameObject Character { get { return character; } }
    
    public NavMeshAgent Agent { get { return GetComponent<NavMeshAgent>(); } }
    private Animator animator;

    private float cameraOffset;

    public bool caught;

    void Awake()
    {
        startPosition = transform.position;

        animator = GetComponent<Animator>();

        GameManager.warrior = this;

        cameraOffset = Camera.main.transform.position.z - transform.position.z;
    }

    public void Update()
    {
        if (!TouchControls.controlsLocked) return;

        var walking = Agent.velocity.x != 0 || Agent.velocity.z != 0;

        animator.SetBool("IsWalking", walking);
        animator.SetFloat("WalkSpeedSensitivity", Agent.speed);
    }

    public void Move(float sensitivity)
    {
        var speed = Agent.speed * sensitivity;

        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        animator.SetBool("IsWalking", true);
        animator.SetFloat("WalkSpeedSensitivity", speed);
        
        MoveCameraWithCharacter();
    }

    private void MoveCameraWithCharacter()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, cameraOffset + transform.position.z);
    }

    public void StopMoving()
    {
        animator.SetBool("IsWalking", false);
    }

    public void ResetWarrior()
    {
        Agent.isStopped = true;
        Agent.ResetPath();

        caught = false;

        StopMoving();
        transform.position = startPosition;
        MoveCameraWithCharacter();
    }
}
