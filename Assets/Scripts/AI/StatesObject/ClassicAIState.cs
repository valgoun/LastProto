using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIState", menuName = "AI State/Classic")]
public class ClassicAIState : AIState {

    [Space]
    public RuntimeAnimatorController Controller;

    public override RuntimeAnimatorController GetController(AIBrain brain)
    {
        return Controller;
    }
}
