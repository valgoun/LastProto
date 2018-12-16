using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Explosion Spell", menuName = "Spell/Explosion")]
public class ExplosionSpell : Spell {

    [Header("Explosion Spell")]
    public float Delay;
    public float Radius;
    public GameObject ExplosionPrefab;
    public GameObject UIPrefab;
    [Space]
    public GameObject ExplosionCursor;
    public int CursorIndicatorPrecision;
    public Material ValidTargetMaterial;
    public Material UnvalidTargetMaterial;

    private GameObject _cursor;
    private LineRenderer _rend;

    public override void StartCasting()
    {
        if (!_cursor)
        {
            _cursor = Instantiate(ExplosionCursor);
            _rend = _cursor.GetComponent<LineRenderer>();
        }
    }

    public override void CastUpdate(Vector3 pos, TargetEnum targetType, GameObject target)
    {
        if ((Targets & targetType) != 0)
        {
            _rend.material = ValidTargetMaterial;
            _cursor.transform.position = target.transform.position;
        }
        else
        {
            _rend.material = UnvalidTargetMaterial;
            _cursor.transform.position = pos;
        }

        _rend.positionCount = CursorIndicatorPrecision;

        float x;
        float z;

        float change = 2 * Mathf.PI / CursorIndicatorPrecision;
        float angle = change;

        for (int i = 0; i < CursorIndicatorPrecision; i++)
        {
            x = Mathf.Sin(angle) * Radius;
            z = Mathf.Cos(angle) * Radius;

            _rend.SetPosition(i, new Vector3(x, 0, z));

            angle += change;
        }
    }

    public override void StopCasting()
    {
        if (_cursor)
            Destroy(_cursor);
    }

    protected override void SpellEffect(GameObject target, Vector3 position)
    {
        _cursor.transform.parent = target.transform;
        _cursor = null;

        SpellManager.Instance.StartCoroutine(ExplosionRoutine(target, Delay));
        GameObject ui = Instantiate(UIPrefab, target.transform);
        ui.GetComponent<TimerUI>().Initialize(Delay);
    }

    private IEnumerator ExplosionRoutine (GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (target)
        {
            Instantiate(ExplosionPrefab, target.transform.position, target.transform.rotation).GetComponent<Explosion>().Radius = Radius;
            if (target.tag == "Aztec")
            {
                target.GetComponent<Unit>().Killed();
            }
            else
                Destroy(target);
        }
    }
}
