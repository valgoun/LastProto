using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubState : StateMachineBehaviour
{

    public RuntimeAnimatorController Controller;

    private Animator _targetAnimator;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _targetAnimator = _targetAnimator ?? animator.transform.parent.GetComponent<Animator>();
        _targetAnimator.runtimeAnimatorController = Controller;
    }
}
