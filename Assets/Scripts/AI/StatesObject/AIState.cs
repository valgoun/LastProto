using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class AIState : SerializedScriptableObject {

    public float AlertLevelIncreaseRate;
    public float AlertLevelDecreaseRate;
    public float AlertTimeBeforeDecrease;

    public abstract RuntimeAnimatorController GetController(AIBrain brain);
}
