using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour, IAiFoe
{

    [Header("Tweaking")]
    public float FollowPrecision;
    public float StimuliLifetimeWhenSeen;
    public float AttackRange;

    [Header("Debug")]
    public bool Invincible;

    [Header("References")]
    public MeshRenderer MyRenderer;
    public GameObject MySelectable;
    public Animator ModelAnimator;

    [Header("Assets")]
    public Material GhostMaterial;

    [NonSerialized]
    public bool Selected;
    [NonSerialized]
    public Transform AttackTarget;
    [NonSerialized]
    public Transform _targetToFollow;

    private Transform _transform;
    private int _squadNumber;
    private int _squadSize;
    private bool _isGhost;

    protected bool _isInBush;
    protected NavMeshAgent _navAgent;

    public Vector3 Position => _transform.position;
    public Transform Transform => _transform;
    public GameObject GameObject => gameObject;
    public bool IsVisible { get; set; }
    public float StimuliLifetime => StimuliLifetimeWhenSeen;


    protected virtual void Start () {
        _navAgent = GetComponent<NavMeshAgent>();
        _transform = transform;
        IsVisible = true;
        AIManager.Instance.AddElement(this);
	}
	
	void Update () {
        if (_targetToFollow)
            FollowRoutine();
	}

    private void OnDestroy()
    {
        AIManager.Instance.RemoveElement(this);   
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

    private void FollowRoutine ()
    {
        if (_targetToFollow.tag == "Conquistador")
        {
            if (AttackDecision(_targetToFollow))
                return;
        }

        Vector3 pos = FormationManager.Instance.GetPositionInFormation(_targetToFollow.position, _squadNumber, _squadSize, _targetToFollow.position - transform.position);
        if ((pos - _navAgent.destination).magnitude >= FollowPrecision)
        {
            _navAgent.SetDestination(pos);
        }
    }

    protected virtual bool AttackDecision (Transform target)
    {
        if (!_navAgent.isStopped && (target.position - transform.position).magnitude <= AttackRange)
        {
            DoBasicAttack(target);
            return true;
        }
        else
            return false;
    }

    protected void DoBasicAttack (Transform target)
    {
        AttackTarget = target;
        _navAgent.isStopped = true;
        ModelAnimator.SetTrigger("BasicAttack");
        _navAgent.ResetPath();
        _targetToFollow = null;
    }

    public void ChangeIntoGhost ()
    {
        MyRenderer.material = GhostMaterial;
        IsVisible = false;
        tag = "Untagged";
        MySelectable.tag = "Untagged";
        _isGhost = true;
    }

    public void Killed ()
    {
        if (!Invincible)
        {
            ModelAnimator.SetTrigger("Dead");
            tag = "Untagged";
            MySelectable.tag = "Untagged";
            _navAgent.isStopped = true;
            Destroy(this);
        }
    }

    public void EnterBush()
    {
        IsVisible = false;
        _isInBush = true;
    }

    public void LeaveBush()
    {
        IsVisible = true;
        _isInBush = false;
    }
}
