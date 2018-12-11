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

    [TabGroup("Asset")]
    public GameObject GhostFX;

    [Space]
    [ReadOnly]
    public bool IsGhost;

    protected override void Start()
    {
        base.Start();
        SelectionManager.Instance.RegisterAztec(this);
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
        _navAgent.isStopped = true;
        ModelAnimator.SetTrigger("JumpAttack");
        AttackTarget = target;
        _navAgent.ResetPath();
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
