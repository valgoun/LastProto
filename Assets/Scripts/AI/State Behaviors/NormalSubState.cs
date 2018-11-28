using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum NormalState
{
    Void,
    Guard,
    Wander,
    Patrol,
    WanderGuard
}

public class NormalSubState : SerializedStateMachineBehaviour
{

    public Dictionary<NormalState, RuntimeAnimatorController> Controllers;

    private Animator _targetAnimator;
    private AIBrain _brain;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_brain == null)
        {
            _brain = animator.transform.parent.GetComponent<AIBrain>();
            _targetAnimator = animator.transform.parent.GetComponent<Animator>();
        }

        _targetAnimator.runtimeAnimatorController = Controllers[_brain.NormalBehaviour];
    }
}
