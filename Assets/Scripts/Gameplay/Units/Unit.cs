using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public class Unit : MonoBehaviour, IAiFoe
{
    [TabGroup("General")]
    public int LifePoint;
    [TabGroup("General")]
    public float FollowPrecision;
    [TabGroup("General")]
    public float StimuliLifetimeWhenSeen;
    [TabGroup("General")]
    public float AttackRange;
    [TabGroup("General")]
    public bool CanHandleRelic;

    [TabGroup("References")]
    public GameObject MySelectable;
    [TabGroup("References")]
    public Animator ModelAnimator;

    [Header("Debug")]
    public bool Invincible;

    [Space]
    [ReadOnly]
    public bool Selected;

    [NonSerialized]
    public Transform AttackTarget;
    [NonSerialized]
    public Transform TargetToFollow;

    private Transform _transform;
    private int _squadNumber;
    private int _squadSize;

    protected bool _isInBush;

    [NonSerialized]
    public NavMeshAgent NavAgent;

    public Vector3 Position => _transform.position;
    public Transform Transform => _transform;
    public GameObject GameObject => gameObject;
    public bool IsVisible { get; set; }
    public float StimuliLifetime => StimuliLifetimeWhenSeen;


    protected virtual void Start () {
        NavAgent = GetComponent<NavMeshAgent>();
        _transform = transform;
        IsVisible = true;
        AIManager.Instance.AddElement(this);
	}
	
	protected virtual void Update () {
        if (TargetToFollow)
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
        TargetToFollow = null;
        NavAgent.stoppingDistance = 0;
        NavAgent.SetDestination(FormationManager.Instance.GetPositionInFormation(destination, formationIndex, size, destination - transform.position));
    }

    public void Follow (Transform target, int formationIndex, int size)
    {
        if (target.tag == "Relic")
        {
            if (!CanHandleRelic)
                return;
        }

        TargetToFollow = target;
        NavAgent.stoppingDistance = 0.5f;
        NavAgent.SetDestination(FormationManager.Instance.GetPositionInFormation(target.position, formationIndex, size, target.position - transform.position));
        _squadNumber = formationIndex;
        _squadSize = size;
    }

    protected void FollowRoutine ()
    {
        if (TargetToFollow.tag == "Conquistador")
        {
            if (AttackDecision(TargetToFollow))
                return;
        }
        else if (TargetToFollow.tag == "Relic")
        {
            if (CanHandleRelic)
                if (AttackDecision(TargetToFollow))
                    return;
        }

        Vector3 pos = FormationManager.Instance.GetPositionInFormation(TargetToFollow.position, _squadNumber, _squadSize, TargetToFollow.position - transform.position);
        if ((pos - NavAgent.destination).magnitude >= FollowPrecision)
        {
            NavAgent.SetDestination(pos);
        }
    }

    protected virtual bool AttackDecision (Transform target)
    {
        if (!NavAgent.isStopped && (target.position - transform.position).magnitude <= AttackRange)
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
        ModelAnimator.SetTrigger("BasicAttack");
        NavAgent.ResetPath();
        NavAgent.isStopped = true;
        TargetToFollow = null;
    }

    public void Damage()
    {
        LifePoint--;
        if (LifePoint <= 0)
            Killed();
    }

    protected virtual void Killed ()
    {
        if (!Invincible)
        {
            ModelAnimator.SetTrigger("Dead");
            tag = "Untagged";
            MySelectable.tag = "Untagged";
            NavAgent.isStopped = true;
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
