using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Spell", menuName = "Spell/Spawn Ghoul")]
public class TransformTargetSpell : Spell
{
    [Header("Transform Spell")]
    public GameObject GhoulToSpawn;
    public AnimationCurve SpawningCurve;
    public float TimeMultiplier = 1;
    [Space]
    public GameObject UIBarPrefab;

    private HoldBar _bar;
    private int _holdNumber;
    private float _timeStamp;
    private string _numberText;
    private int _maximum;
    private string _maximumText;

    public override bool StartCasting()
    {
        _bar = Instantiate(UIBarPrefab).GetComponent<HoldBar>();
        _bar.SetLevel(0);
        _holdNumber = 1;
        _maximum = SpellManager.Instance.Souls;
        _maximumText = _maximum.ToString();
        _numberText = _holdNumber.ToString();
        if (_numberText.Length < _maximumText.Length)
        {
            int length = _maximumText.Length - _numberText.Length;
            for (int i = 0; i < length; i++)
            {
                _numberText = "0" + _numberText;
            }
        }
        _bar.SetText(_numberText + " / " + _maximumText);
        _timeStamp = Time.time;

        return true;
    }

    public override void CastUpdate(Vector3 position, TargetEnum targetType, GameObject target, bool forceInvalid, bool isOnUI)
    {
        float realValue = 1 + SpawningCurve.Evaluate((Time.time - _timeStamp) / TimeMultiplier);
        int value = Mathf.FloorToInt(realValue);

        if (_maximum != SpellManager.Instance.Souls)
        {
            _maximum = SpellManager.Instance.Souls;
            _maximumText = _maximum.ToString();
        }

        if (_holdNumber > _maximum || (value > _holdNumber && _holdNumber != _maximum))
        {
            if (value >= _maximum)
                _holdNumber = _maximum;
            else
                _holdNumber = value;

            _numberText = _holdNumber.ToString();
            if (_numberText.Length < _maximumText.Length)
            {
                int length = _maximumText.Length - _numberText.Length;
                for (int i = 0; i < length; i++)
                {
                    _numberText = "0" + _numberText;
                }
            }
        }

        _bar.SetText(_numberText + " / " + _maximumText);

        realValue /= _maximum;
        realValue = (realValue > 1) ? 1 : realValue;
        _bar.SetLevel(realValue);
    }

    public override void StopCasting()
    {
        if (_bar)
            Destroy(_bar.gameObject);
    }

    protected override void SpellEffect(GameObject target, Vector3 position)
    {
        StopCasting();

        Vector3 pos = SelectionManager.Instance.Shaman.transform.position;
        Quaternion rot = Quaternion.identity;
        
        for (int i = 0; i < _holdNumber; i++)
        {
            GameObject testGhoul = Instantiate(GhoulToSpawn, pos, rot);
            if (testGhoul.tag == "Aztec")
            {
                SelectionManager.Instance.CleanSelection();
                Unit script = testGhoul.GetComponent<Unit>();
                script.Select();
                SelectionManager.Instance.SelectedElements.Add(script);
            }
        }

        SpellManager.Instance.Souls -= _holdNumber;
    }

    public override bool GetAvailable()
    {
        if (SpellManager.Instance.Souls > 0)
            return base.GetAvailable();
        else
            return false;
    }
}
