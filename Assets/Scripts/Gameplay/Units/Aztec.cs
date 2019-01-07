using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Aztec : Unit {

    [TabGroup("Ghoul")]
    public float JumpAttackRange;
    [TabGroup("Ghoul")]
    public LayerMask JumpAttackLineOfSight;
    [TabGroup("Ghoul")]
    public float ShamanFormationDistance;
    [TabGroup("Ghoul")]
    public float MaxShamanDistance;
    [TabGroup("Ghoul")]
    public float MaxAggroDistance;

    [TabGroup("Asset")]
    public GameObject GhostFX;

    [Space]
    [ReadOnly]
    public bool IsGhost;
    [ReadOnly]
    public int FormationPosition = 0;

    protected override void Start()
    {
        base.Start();
        FormationPosition = SelectionManager.Instance.RegisterAztec(this);
    }

    protected override void Update()
    {
        if (!TargetToFollow)
            AggroCheck();
        else
        {
            if ((SelectionManager.Instance.Shaman.transform.position - transform.position).magnitude > MaxShamanDistance)
                TargetToFollow = null;
            else if ((SelectionManager.Instance.Shaman.transform.position - TargetToFollow.position).magnitude > MaxShamanDistance)
                TargetToFollow = null;
        }

        if (!TargetToFollow)
        {
            float angle = ((float)FormationPosition / (float)SelectionManager.Instance.Aztecs.Count) * 360;
            angle += 90;
            Vector3 offset = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), 0, Mathf.Sin(Mathf.Deg2Rad * angle));
            offset *= ShamanFormationDistance;

            Vector3 pos = SelectionManager.Instance.Shaman.transform.position + offset;
            _navAgent.SetDestination(pos);
        }
        else
            FollowRoutine();
    }

    void AggroCheck ()
    {
        Transform target = null;
        foreach (Collider coll in Physics.OverlapSphere(transform.position, MaxAggroDistance, SelectionManager.Instance.SelectableLayer))
        {
            if (coll.tag == "Conquistador")
            {
                if ((SelectionManager.Instance.Shaman.transform.position - coll.transform.position).magnitude > MaxShamanDistance)
                    continue;

                if (!target)
                {
                    target = coll.transform;
                }
                else if ((target.position - transform.position).sqrMagnitude > (coll.transform.position - transform.position).sqrMagnitude)
                {
                    target = coll.transform;
                }
            }
        }

        if (target != null)
        {
            Follow(target, 0, 1);
        }
    }

    protected override bool AttackDecision(Transform target)
    {
        if (IsGhost)
            return false;

        if (_isInBush)
        {
            if (!_navAgent.isStopped && (target.position - transform.position).magnitude <= JumpAttackRange)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, target.position - transform.position, out hit, (target.position - transform.position).magnitude, JumpAttackLineOfSight))
                {
                    if (hit.transform.gameObject == target.gameObject)
                    {
                        DoJumpAttack(target);
                        return true;
                    }
                }
            }
            return false;
        }
        else
            return base.AttackDecision(target);
    }

    private void DoJumpAttack(Transform target)
    {
        ModelAnimator.SetTrigger("JumpAttack");
        AttackTarget = target;
        _navAgent.ResetPath();
        _navAgent.isStopped = true;
        TargetToFollow = null;
    }

    public void ChangeIntoGhost()
    {
        IsVisible = false;
        tag = "Untagged";
        MySelectable.tag = "Untagged";
        IsGhost = true;

        Instantiate(GhostFX, transform);
    }
}
