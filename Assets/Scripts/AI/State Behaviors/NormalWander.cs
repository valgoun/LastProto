using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NormalWander : StateMachineBehaviour {

    public bool UseInitialPosition;

    private AIBrain _brain;
    private NavMeshAgent _agent;
    private Vector3 _startPos;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_brain == null)
        {
            _brain = animator.GetComponent<AIBrain>();
            _agent = _brain.Agent;
        }

        _startPos = animator.transform.position;

        var randomPos = Random.insideUnitSphere * _brain.NormalWanderRadius;
        randomPos.y = 0;
        _agent.SetDestination(((UseInitialPosition) ? _brain.InitialPosition : _startPos) + randomPos);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            var randomPos = Random.insideUnitSphere * _brain.NormalWanderRadius;
            randomPos.y = 0;
            _agent.SetDestination(((UseInitialPosition) ? _brain.InitialPosition : _startPos) + randomPos);
        }
    }
}
