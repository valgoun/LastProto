using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wall Spell", menuName = "Spell/Spawn Wall")]
public class SpawnWallSpell : Spell
{
    [Header("Spawn Wall Spell")]
    public GameObject WallPrefab;
    public float Lifetime;
    public Vector3 WallScale;
    [Space]
    public GameObject ExplosionCursor;
    public Material ValidTargetMaterial;
    public Material UnvalidTargetMaterial;

    private GameObject _cursor;
    private LineRenderer _rend;

    public override bool StartCasting()
    {
        if (!_cursor)
        {
            _cursor = Instantiate(ExplosionCursor);
            _rend = _cursor.GetComponent<LineRenderer>();
            _rend.positionCount = 4;
        }

        return true;
    }

    public override void CastUpdate(Vector3 position, TargetEnum targetType, GameObject target)
    {
        Vector3 pos;
        if ((Targets & targetType) != 0)
        {
            _rend.material = ValidTargetMaterial;
            pos = target.transform.position;
        }
        else
        {
            _rend.material = UnvalidTargetMaterial;
            pos = position;
        }

        _cursor.transform.position = pos;

        Vector3 dir = pos - SelectionManager.Instance.Shaman.transform.position;
        dir.y = 0;
        _cursor.transform.rotation = Quaternion.FromToRotation(Vector3.forward, dir);

        _rend.SetPosition(0, new Vector3(-(WallScale.x / 2), 0, -(WallScale.z / 2)));
        _rend.SetPosition(1, new Vector3((WallScale.x / 2), 0, -(WallScale.z / 2)));
        _rend.SetPosition(2, new Vector3((WallScale.x / 2), 0, (WallScale.z / 2)));
        _rend.SetPosition(3, new Vector3(-(WallScale.x / 2), 0, (WallScale.z / 2)));
    }

    public override void StopCasting()
    {
        if (_cursor)
            Destroy(_cursor);
    }

    protected override void SpellEffect(GameObject target, Vector3 position)
    {
        StopCasting();

        Vector3 pos;
        if (target)
        {
            pos = target.transform.position;
            Destroy(target);
        }
        else
        {
            pos = position;
        }

        Vector3 dir = pos - SelectionManager.Instance.Shaman.transform.position;
        dir.y = 0;
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, dir);

        GameObject obj = Instantiate(WallPrefab, pos, rot);
        obj.AddComponent<Lifetime>().StartLifetime(Lifetime);
    }
}
