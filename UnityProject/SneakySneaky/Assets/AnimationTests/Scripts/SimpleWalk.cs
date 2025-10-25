using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleWalk : MonoBehaviour
{
    public Transform left;
    public Transform right;
    bool target = true; // true = right
    Animator animator;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((agent.remainingDistance != 0 && agent.remainingDistance < 0.1))
        {
            if (!target)
            {
                agent.SetDestination(left.position);
            }
            else
            {
                agent.SetDestination(right.position);
            }
        }
    }
}
