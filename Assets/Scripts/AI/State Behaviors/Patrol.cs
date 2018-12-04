using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : StateMachineBehaviour
{
    private AIBrain _brain;
    private NavMeshAgent _agent;
    private int _index = 0;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_brain == null)
        {
            _brain = animator.GetComponent<AIBrain>();
            _agent = _brain.Agent;
        }

        _agent.autoBraking = false;
        _agent.stoppingDistance = 0.25f;

        if (_brain.NormalWaypointsHolder.childCount > 0)
        {
            _agent.SetDestination(_brain.NormalWaypointsHolder.GetChild(_index).position);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_brain.CurrentWaypoint)
        {
            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                Waypoint waypoint = _brain.NormalWaypointsHolder.GetChild(_index).GetComponent<Waypoint>();
                if (waypoint && waypoint.TimeToWait > 0)
                {
                    _brain.CurrentWaypoint = waypoint;
                    _brain.WaypointWaitTime = Time.time;
                }
                else if (_brain.NormalWaypointsHolder.childCount > 0)
                {
                    _index++;
                    if (_index >= _brain.NormalWaypointsHolder.childCount)
                    {
                        _index = 0;
                    }

                    _agent.SetDestination(_brain.NormalWaypointsHolder.GetChild(_index).position);
                }
            }
        }
        else
        {
            if (_brain.WaypointWaitTime + _brain.CurrentWaypoint.TimeToWait <= Time.time)
            {
                _brain.CurrentWaypoint = null;
                if (_brain.NormalWaypointsHolder.childCount > 0)
                {
                    _index++;
                    if (_index >= _brain.NormalWaypointsHolder.childCount)
                    {
                        _index = 0;
                    }

                    _agent.SetDestination(_brain.NormalWaypointsHolder.GetChild(_index).position);
                }
                return;
            }

            if (_brain.CurrentWaypoint.ApplyRotation)
            {
                float angle = Quaternion.Angle(animator.transform.rotation, _brain.CurrentWaypoint.transform.rotation);
                if (angle <= _agent.angularSpeed * Time.deltaTime)
                {
                    animator.transform.rotation = _brain.CurrentWaypoint.transform.rotation;
                }
                else
                {
                    animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, _brain.CurrentWaypoint.transform.rotation, ((_agent.angularSpeed * Time.deltaTime) / angle));
                }
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent.autoBraking = true;
        _agent.stoppingDistance = 0f;
    }
}
