using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Transform Spell", menuName = "Spell/Transform Target")]
public class TransformTargetSpell : Spell
{
    [Header("Transform Spell")]
    public GameObject TransformInto;

    public override void StartCasting()
    {
        
    }

    public override void CastUpdate(Vector3 position, TargetEnum targetType, GameObject target)
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

        GameObject testGhoul = Instantiate(TransformInto, pos, rot);
        if (testGhoul.tag == "Aztec")
        {
            SelectionManager.Instance.CleanSelection();
            Unit script = testGhoul.GetComponent<Unit>();
            script.Select();
            SelectionManager.Instance.SelectedElements.Add(script);
        }
    }

    public override bool GetAvailable()
    {
        bool openSpot = false;
        foreach(Unit ghoul in SelectionManager.Instance.Aztecs)
        {
            if (!ghoul)
            {
                openSpot = true;
                break;
            }
        }
        return base.GetAvailable() && openSpot;
    }
}
