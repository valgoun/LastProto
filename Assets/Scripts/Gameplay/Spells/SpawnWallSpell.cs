using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wall Spell", menuName = "Spell/Spawn Wall", order = 23)]
public class SpawnWallSpell : Spell
{
    [Header("Spawn Wall Spell")]
    public GameObject WallPrefab;
    public float Lifetime;

    public override void StartCasting()
    {

    }

    public override void StopCasting()
    {

    }

    protected override void SpellEffect(GameObject target, Vector3 position)
    {
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
