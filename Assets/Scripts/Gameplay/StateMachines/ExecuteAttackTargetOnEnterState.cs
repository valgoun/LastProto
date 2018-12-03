using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteAttackTargetOnEnterState : StateMachineBehaviour
{
    private Unit _unit;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_unit)
        {
            _unit = animator.transform.parent.GetComponent<Unit>();
        }

        Destroy(_unit.AttackTarget.parent.gameObject);
    }
}

