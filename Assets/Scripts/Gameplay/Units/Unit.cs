using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour, IAiVisible
{

    [Header("Tweaking")]
    public float FollowPrecision;

    [Header("References")]
    public MeshRenderer MyRenderer;
    public GameObject MySelectable;

    [Header("Assets")]
    public Material GhostMaterial;

    [NonSerialized]
    public bool Selected;

    private NavMeshAgent _navAgent;
    private Transform _targetToFollow;
    private Transform _transform;

    public Vector3 Position => _transform.position;
    public Transform Transform => _transform;
    public GameObject GameObject => gameObject;
    public bool IsVisible { get; set; }

    void Start () {
        _navAgent = GetComponent<NavMeshAgent>();
        _transform = transform;
        IsVisible = true;
	}
	
	void Update () {
        if (_targetToFollow)
            FollowRoutine();
	}

    public void Select ()
    {
        Selected = true;
    }

    public void UnSelect ()
    {
        Selected = false;
    }

    public void MoveTo (Vector3 destination)
    {
        _targetToFollow = null;
        _navAgent.stoppingDistance = 0;
        _navAgent.SetDestination(destination);
    }

    public void Follow (Transform target)
    {
        _targetToFollow = target;
        _navAgent.stoppingDistance = 0.5f;
        _navAgent.SetDestination(target.position);
    }

    void FollowRoutine ()
    {
        if ((_targetToFollow.position - _navAgent.destination).magnitude >= FollowPrecision)
        {
            _navAgent.SetDestination(_targetToFollow.position);
        }
    }

    public void ChangeIntoGhost ()
    {
        MyRenderer.material = GhostMaterial;
        IsVisible = false;
        tag = "Untagged";
        MySelectable.tag = "Untagged";
    }
}
