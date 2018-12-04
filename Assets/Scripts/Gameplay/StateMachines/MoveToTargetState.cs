using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToTargetState : StateMachineBehaviour {

    public float Duration;
    public AnimationCurve VerticalMovement;
    public AnimationCurve HorizontalMovement;

    private float _index;
    private Vector3 _origin;
    private Unit _unit;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (!_unit)
        {
            _unit = animator.transform.parent.GetComponent<Unit>();
        }

        _index = 0;
        _origin = animator.transform.parent.position;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _index += Time.deltaTime / (Duration * stateInfo.length);
        if (_index > 1)
            _index = 1;

        float vertical = VerticalMovement.Evaluate(_index) + Mathf.Lerp(_origin.y, _unit.AttackTarget.parent.position.y, _index);
        Vector3 horizontal = new Vector3(Mathf.Lerp(_origin.x, _unit.AttackTarget.parent.position.x, HorizontalMovement.Evaluate(_index)), 0, Mathf.Lerp(_origin.z, _unit.AttackTarget.parent.position.z, HorizontalMovement.Evaluate(_index)));

        horizontal.y = vertical;
        animator.transform.parent.position = horizontal;
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
