using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aztec : Unit {

    [Header("Aztec Tweaking")]
    public float JumpAttackRange;

    protected override void Start()
    {
        base.Start();
        SelectionManager.Instance.RegisterAztec(this);
    }

    protected override bool AttackDecision(Transform target)
    {
        if (_isInBush)
        {
            if ((target.position - transform.position).magnitude <= JumpAttackRange)
            {
                DoJumpAttack(target);
                return true;
            }
            return false;
        }
        else
            return base.AttackDecision(target);
    }

    private void DoJumpAttack(Transform target)
    {
        transform.position = target.position;
        Destroy(target.gameObject);
    }
}
