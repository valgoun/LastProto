using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ghost Spell", menuName = "Spell/Invisibility", order = 24)]
public class GhostSpell : Spell
{
    [Header("Ghost Spell")]
    public float DeathDelay;

    public override void StartCasting()
    {

    }

    public override void StopCasting()
    {

    }

    protected override void SpellEffect(GameObject target, Vector3 position)
    {
        if (!target)
            return;

        if (target.tag == "Aztec" || target.tag == "Shaman")
        {
            target.GetComponent<Unit>().ChangeIntoGhost();
        }

        target.AddComponent<Lifetime>().StartLifetime(DeathDelay);
    }
}
