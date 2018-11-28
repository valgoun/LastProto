using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour, IAiVisible, IVisionElement
{

    [Header("Tweaking")]
    public float FollowPrecision;
    public float StimuliLifetimeWhenSeen;
    [SerializeField]
    private float _visionRange;

    [Header("Debug")]
    public bool Invincible;

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
    private int _squadNumber;
    public int _squadSize;

    public Vector3 Position => _transform.position;
    public Transform Transform => _transform;
    public GameObject GameObject => gameObject;
    public bool IsVisible { get; set; }
    public float VisionRange => _visionRange;
    public float StimuliLifetime => StimuliLifetimeWhenSeen;


    void Start () {
        _navAgent = GetComponent<NavMeshAgent>();
        _transform = transform;
        IsVisible = true;
        FogManager.Instance?.RegisterElement(this);
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

    public void MoveTo (Vector3 destination, int formationIndex, int size)
    {
        _targetToFollow = null;
        _navAgent.stoppingDistance = 0;
        _navAgent.SetDestination(FormationManager.Instance.GetPositionInFormation(destination, formationIndex, size, destination - transform.position));
    }

    public void Follow (Transform target, int formationIndex, int size)
    {
        _targetToFollow = target;
        _navAgent.stoppingDistance = 0.5f;
        _navAgent.SetDestination(FormationManager.Instance.GetPositionInFormation(target.position, formationIndex, size, target.position - transform.position));
        _squadNumber = formationIndex;
        _squadSize = size;
    }

    void FollowRoutine ()
    {
        Vector3 pos = FormationManager.Instance.GetPositionInFormation(_targetToFollow.position, _squadNumber, _squadSize, _targetToFollow.position - transform.position);
        if ((pos - _navAgent.destination).magnitude >= FollowPrecision)
        {
            _navAgent.SetDestination(pos);
        }
    }

    private void OnDestroy()
    {
        FogManager.Instance?.DeleteElement(this);
    }

    public void ChangeIntoGhost ()
    {
        MyRenderer.material = GhostMaterial;
        IsVisible = false;
        tag = "Untagged";
        MySelectable.tag = "Untagged";
    }

    public void Killed ()
    {
        if (!Invincible)
            Destroy(gameObject);
    }
}
