using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RotateToTargetState : StateMachineBehaviour
{
    private Unit _unit;
    private NavMeshAgent _agent;
    private Vector3 _origin;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_unit)
        {
            _unit = animator.transform.parent.GetComponent<Unit>();
            _agent = _unit.GetComponent<NavMeshAgent>();
        }

        _origin = animator.transform.parent.position;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, _unit.AttackTarget.parent.position - _origin);
        float angle = Quaternion.Angle(animator.transform.parent.rotation, rot);
        if (angle <= _agent.angularSpeed * Time.deltaTime)
        {
            animator.transform.parent.rotation = rot;
        }
        else
        {
            animator.transform.parent.rotation = Quaternion.Slerp(animator.transform.parent.rotation, rot, ((_agent.angularSpeed * Time.deltaTime) / angle));
        }
    }
}
