﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Threat : StateMachineBehaviour
{

    private AIBrain _brain;
    private NavMeshAgent _agent;
    private Stimulus _targetStimulus;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Entering Threat");
        if (_brain == null)
        {
            _brain = animator.GetComponent<AIBrain>();
            _agent = _brain.Agent;
        }

        _targetStimulus = _brain.BestStimulus;

        if (_targetStimulus == null)
            return;

        if (_targetStimulus.Type != StimulusType.Transmission)
        {
            _brain.CommunicateStimulus();
            _agent.stoppingDistance = _brain.MinAimDistance;
            _agent.SetDestination(_targetStimulus.Position);
        }
        else
        {
            _agent.stoppingDistance = _brain.MinAimDistance;
            _agent.SetDestination(_targetStimulus.GetData<TransmissionData>().GetTrueStimulus().Position);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _targetStimulus = _brain.BestStimulus;

        if (_targetStimulus != null)
        {

            if (_targetStimulus.Type != StimulusType.Transmission && Vector3.Distance(_agent.destination, _targetStimulus.Position) > 0.1f)
                _agent.SetDestination(_targetStimulus.Position);
            else if (_targetStimulus.Type == StimulusType.Transmission && Vector3.Distance(_agent.destination, _targetStimulus.GetData<TransmissionData>().GetTrueStimulus().Position) > 0.1f)
                _agent.SetDestination(_targetStimulus.GetData<TransmissionData>().GetTrueStimulus().Position);
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
