using UnityEngine;
using UnityEngine.AI;

public class NavMeshTest : MonoBehaviour
{
    void Start()
    {
        var agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(Vector3.zero);
    }
}
