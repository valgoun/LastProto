using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CbtAim : StateMachineBehaviour {

    private AIBrain _brain;
    private Transform _transform;
    private Stimulus _targetStimulus;
    private float _timer;
    private Conquistador _conq;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_brain)
        {
            _brain = animator.GetComponent<AIBrain>();
            _transform = _brain.transform;
            _conq = animator.GetComponent<Conquistador>();
        }

        _targetStimulus = _brain.BestStimulus;
        _timer = 0.0f;
        if (_conq.MyAnimator)
            _conq.MyAnimator.SetBool("Aim", true);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_targetStimulus == null)
            return;

        if(Vector3.Distance(_transform.position, _targetStimulus.Position) > _brain.MaxAimDistance)
        {
            animator.SetTrigger("Move");
            return;
        }

        var lookAtPos = _targetStimulus.Position;
        lookAtPos.y = _transform.position.y;
        var aimDirection = lookAtPos - _transform.position;

        _transform.forward =  Vector3.RotateTowards(_transform.forward, aimDirection.normalized, _brain.AimRotationSpeed * Mathf.Deg2Rad * Time.deltaTime, 0.0f);

        _timer += Time.deltaTime;
        if(_timer >= _brain.AimDuration)
        {
            _brain.Shoot(_targetStimulus.GetData<EnemyData>().EnemyGameObject);
            animator.SetTrigger("Reload");
            if (_conq.MyAnimator)
                _conq.MyAnimator.SetTrigger("Shoot");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (_conq.MyAnimator)
            _conq.MyAnimator.SetBool("Aim", false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
