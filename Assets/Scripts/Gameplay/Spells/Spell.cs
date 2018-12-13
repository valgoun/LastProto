using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : ScriptableObject {

    public string SpellName;
    [Space]
    public TargetEnum Targets;
    public float CooldownDuration;

    [NonSerialized]
    public float CurrentCooldown;

    public abstract void StartCasting();
    public abstract void StopCasting();
    public abstract void CastUpdate(Vector3 position, TargetEnum targetType, GameObject target);

    public void ExecuteSpell(GameObject target, Vector3 position)
    {
        CurrentCooldown = Time.time + CooldownDuration;
        SpellEffect(target, position);
    }

    protected abstract void SpellEffect(GameObject target, Vector3 position);

    public virtual bool GetAvailable ()
    {
        return (CurrentCooldown <= Time.time);
    }
}
