using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hook Spell", menuName = "Spell/Hook")]
public class HookSpell : Spell
{
    [Header("Hook Spell")]
    public float HookRange;
    public float HookTravelSpeed;
    public LayerMask HookBlockLayers;
    [Space]
    public float HookPullSpeed;
    public float HookPullSafeDistance;
    public float HookAddedStunTime;
    [Space]
    public GameObject HookPrefab;
    [Space]
    public GameObject HookCursor;
    public Material ValidTargetMaterial;
    public Material UnvalidTargetMaterial;

    private GameObject _cursor;
    private LineRenderer _rend;

    public override void CastUpdate(Vector3 pos, TargetEnum targetType, GameObject target, bool forceInvalid = false)
    {
        Vector3 aim = pos;
        if (!forceInvalid && ((Targets & TargetEnum.Void) != 0 || (Targets & targetType) != 0))
        {
            _rend.material = ValidTargetMaterial;
            if (target)
                aim = target.transform.position;
        }
        else
        {
            _rend.material = UnvalidTargetMaterial;
        }

        Vector3 shamanPos = SelectionManager.Instance.Shaman.transform.position;
        shamanPos.y = aim.y;
        _rend.positionCount = 2;
        _rend.SetPosition(0, shamanPos);
        _rend.SetPosition(1, shamanPos + (pos - shamanPos).normalized * HookRange);
    }

    public override bool StartCasting()
    {
        if (!_cursor)
        {
            _cursor = Instantiate(HookCursor);
            _cursor.transform.position = Vector3.zero;
            _rend = _cursor.GetComponent<LineRenderer>();
        }

        return true;
    }

    public override void StopCasting()
    {
        if (_cursor)
            Destroy(_cursor);
    }

    protected override void SpellEffect(GameObject target, Vector3 position)
    {
        StopCasting();

        Vector3 shamanPos = SelectionManager.Instance.Shaman.transform.position;

        GameObject hook = Instantiate(HookPrefab);
        hook.transform.position = shamanPos;
        Vector3 dir = position - shamanPos;
        dir.y = 0;
        hook.GetComponent<Hook>().Initialize(HookRange, HookTravelSpeed, dir, HookBlockLayers, HookPullSpeed, HookPullSafeDistance, HookAddedStunTime);
    }
}
