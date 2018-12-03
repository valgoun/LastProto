using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ResumeNavMeshOnEnterState : StateMachineBehaviour
{
    private NavMeshAgent _navAgent;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_navAgent)
        {
            _navAgent = animator.transform.parent.GetComponent<NavMeshAgent>();
        }

        _navAgent.isStopped = false;
    }
}

