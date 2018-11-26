using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wander : StateMachineBehaviour {

    private AIBrain _brain;
    private NavMeshAgent _agent;
    private Stimulus _targetStimulus;
    private Vector3 _wanderCenter;

     // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_brain == null)
        {
            _brain = animator.GetComponent<AIBrain>();
            _agent = _brain.Agent;
        }

        _targetStimulus = _brain.BestStimulus;
        _wanderCenter = animator.transform.position;

        var randomPos = Random.insideUnitSphere * _brain.WanderRadius;
        randomPos.y = 0;
        _agent.SetDestination(_wanderCenter + randomPos);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_brain.BestStimulus != null && _targetStimulus != _brain.BestStimulus)
        {
            animator.SetTrigger("MoveTo");
            return;
        }

        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            var randomPos = Random.insideUnitSphere * _brain.WanderRadius;
            randomPos.y = 0;
            _agent.SetDestination(_wanderCenter + randomPos);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
