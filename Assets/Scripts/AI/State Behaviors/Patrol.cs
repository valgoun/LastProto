using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : StateMachineBehaviour
{
    private AIBrain _brain;
    private NavMeshAgent _agent;
    private int _index = 0;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_brain == null)
        {
            _brain = animator.GetComponent<AIBrain>();
            _agent = _brain.Agent;
        }

        _agent.autoBraking = false;
        _agent.stoppingDistance = 0.25f;

        if (_brain.NormalWaypointsHolder.childCount > 0)
        {
            if (_index >= _brain.NormalWaypointsHolder.childCount)
            {
                _index = 0;
            }

            _agent.SetDestination(_brain.NormalWaypointsHolder.GetChild(_index).position);

            _index++;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            if (_brain.NormalWaypointsHolder.childCount > 0)
            {
                if (_index >= _brain.NormalWaypointsHolder.childCount)
                {
                    _index = 0;
                }
                _agent.SetDestination(_brain.NormalWaypointsHolder.GetChild(_index).position);

                _index++;
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent.autoBraking = true;
        _agent.stoppingDistance = 0f;
    }
}
