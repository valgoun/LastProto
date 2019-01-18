using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound Spell", menuName = "Spell/Sound")]
public class SoundSpell : Spell
{
    [Header("Stone Spell")]
    public float EffectDelay;
    public float SoundRadius;
    public bool SoundGoThroughWalls;
    public float SoundStimuliLifeTime;
    public float LifetimeQuickFix;
    [Space]
    public GameObject SoundCursor;
    public int CursorIndicatorPrecision;
    public Material ValidTargetMaterial;
    public Material UnvalidTargetMaterial;

    private GameObject _cursor;
    private LineRenderer _rend;

    public override bool StartCasting()
    {
        if (!_cursor)
        {
            _cursor = Instantiate(SoundCursor);
            _rend = _cursor.GetComponent<LineRenderer>();
        }

        return true;
    }

    public override void CastUpdate(Vector3 pos, TargetEnum targetType, GameObject target, bool forceInvalid, bool isOnUI)
    {
        if (isOnUI)
        {
            _cursor.SetActive(false);
            return;
        }
        else
            _cursor.SetActive(true);

        if (!forceInvalid && ((Targets & TargetEnum.Void) != 0 || (Targets & targetType) != 0))
        {
            _rend.material = ValidTargetMaterial;
            if (target)
                _cursor.transform.position = target.transform.position;
            else
                _cursor.transform.position = pos;
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
            x = Mathf.Sin(angle) * SoundRadius;
            z = Mathf.Cos(angle) * SoundRadius;

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
        StopCasting();
        SpellManager.Instance.StartCoroutine(SoundRoutine(EffectDelay, position));
    }

    private IEnumerator SoundRoutine(float delay, Vector3 position)
    {
        yield return new WaitForSeconds(delay);

        GameObject sound = new GameObject(SpellName + " effect");
        sound.transform.position = position + Vector3.up;
        sound.AddComponent<Lifetime>().StartLifetime(LifetimeQuickFix);
        SoundEmitter script = sound.AddComponent<SoundEmitter>();
        script.SoundRange = SoundRadius;
        script.SoundGoThroughWalls = SoundGoThroughWalls;
        script.SoundStimuliLifetime = SoundStimuliLifeTime;
    }
}
