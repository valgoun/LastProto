using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CbtEntry : StateMachineBehaviour
{

    public string OnReloadedTrigger;
    public string NoReloadedTrigger;

    private AIBrain _brain;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _brain = _brain ?? animator.GetComponent<AIBrain>();
        if (_brain.Reloaded)
            animator.SetTrigger(OnReloadedTrigger);
        else
            animator.SetTrigger(NoReloadedTrigger);
    }
}
