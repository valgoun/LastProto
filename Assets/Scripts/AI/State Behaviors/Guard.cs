using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard : StateMachineBehaviour {

    private AIBrain _brain;
    private NavMeshAgent _agent;
    private Transform _transform;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (_brain == null)
        {
            _brain = animator.GetComponent<AIBrain>();
            _agent = _brain.Agent;
            _transform = _brain.transform;
        }

        _agent.SetDestination(_brain.InitialPosition);
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (_agent && !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            float angle = Quaternion.Angle(_transform.rotation, _brain.InitialRotation);
            if (angle <= _agent.angularSpeed * Time.deltaTime)
            {
                _transform.rotation = _brain.InitialRotation;
            }
            else
            {
                _transform.rotation = Quaternion.Slerp(_transform.rotation, _brain.InitialRotation, ((_agent.angularSpeed * Time.deltaTime) / angle));
            }
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
