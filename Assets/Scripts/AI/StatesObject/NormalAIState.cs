using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NormalState
{
    Void,
    Guard,
    Wander,
    Patrol,
    WanderGuard
}

[CreateAssetMenu(fileName = "AIState", menuName = "AI State/Normal Behaviour")]
public class NormalAIState : AIState
{
    [Space]
    public Dictionary<NormalState, RuntimeAnimatorController> Controllers;

    public override RuntimeAnimatorController GetController(AIBrain brain)
    {
        return Controllers[brain.NormalBehaviour];
    }
}
