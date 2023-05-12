using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    NavMeshAgent agent;
    Transform playerTransform;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTransform = transform;
    }

    void Update()
    {
        Vector3 moveDirection = new Vector3(-Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical")).normalized;

        if (moveDirection != Vector3.zero)
        {
            agent.Move(moveDirection * Time.deltaTime * agent.speed);
            playerTransform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }
}
