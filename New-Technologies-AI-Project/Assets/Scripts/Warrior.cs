using UnityEngine;
using UnityEngine.AI;

public class Warrior : MonoBehaviour, IPlayable
{
    public GameObject character;
    public GameObject Character { get { return character; } }
    
    private NavMeshAgent agent;
    private Animator animator;

    private float cameraOffset;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        cameraOffset = Camera.main.transform.position.z - transform.position.z;
    }
    
    public void Move(float sensitivity)
    {
        var speed = agent.speed * sensitivity;

        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        animator.SetBool("IsWalking", true);
        animator.SetFloat("WalkSpeedSensitivity", sensitivity);

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
}
