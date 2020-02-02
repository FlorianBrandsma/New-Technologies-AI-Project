using UnityEngine;
using UnityEngine.AI;

public class Drake : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private RaycastHit hitInfo = new RaycastHit();

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                agent.destination = hitInfo.point;           
            }
        }

        bool walking = agent.velocity.x != 0 || agent.velocity.z != 0;

        animator.SetBool("IsWalking", walking);
    }
}
