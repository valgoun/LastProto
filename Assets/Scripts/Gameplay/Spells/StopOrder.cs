using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stop Order", menuName = "Spell/Stop")]
public class StopOrder : Spell
{
    private bool active = false;

    public override void CastUpdate(Vector3 position, TargetEnum targetType, GameObject target)
    {

    }

    public override bool StartCasting()
    {
        SpellEffect(null, Vector3.zero);
        return false;
    }

    public override void StopCasting()
    {

    }

    protected override void SpellEffect(GameObject target, Vector3 position)
    {
        active = !active;

        if (active)
        {
            foreach(Aztec aztec in SelectionManager.Instance.Aztecs)
            {
                aztec.StopHere(SelectionManager.Instance.Shaman.transform.position);
            }
        }
        else
        {
            foreach (Aztec aztec in SelectionManager.Instance.Aztecs)
            {
                aztec.UndoStop();
            }
        }
    }
}
