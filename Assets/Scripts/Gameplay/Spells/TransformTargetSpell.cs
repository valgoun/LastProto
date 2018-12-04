using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Transform Spell", menuName = "Spell/Transform Target", order = 21)]
public class TransformTargetSpell : Spell
{
    [Header("Transform Spell")]
    public GameObject TransformInto;

    public override void StartCasting()
    {
        
    }

    public override void StopCasting()
    {

    }

    protected override void SpellEffect(GameObject target, Vector3 position)
    {
        Vector3 pos;
        Quaternion rot;
        if (target)
        {
            if (target.tag == "Corpse")
            {
                target = target.transform.parent.gameObject;
            }
            pos = target.transform.position;
            rot = target.transform.rotation;
            Destroy(target);
        }
        else
        {
            pos = position;
            rot = Quaternion.identity;
        }

        Instantiate(TransformInto, pos, rot);
    }
}
