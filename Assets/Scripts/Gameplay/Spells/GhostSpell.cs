using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ghost Spell", menuName = "Spell/Invisibility")]
public class GhostSpell : Spell
{
    [Header("Ghost Spell")]
    public float DeathDelay;
    public GameObject UIPrefab;

    public override bool StartCasting()
    {
        return true;
    }

    public override void CastUpdate(Vector3 position, TargetEnum targetType, GameObject target, bool forceInvalid)
    {

    }

    public override void StopCasting()
    {

    }

    protected override void SpellEffect(GameObject target, Vector3 position)
    {
        if (!target)
            return;

        if (target.tag == "Aztec")
        {
            target.GetComponent<Aztec>().ChangeIntoGhost();
        }

        target.AddComponent<Lifetime>().StartLifetime(DeathDelay);
        GameObject ui = Instantiate(UIPrefab, target.transform);
        ui.GetComponent<TimerUI>().Initialize(DeathDelay);
    }
}
