using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public enum TargetEnum
{
    Conquistador = 1<<0,
    Shaman = 1<<1,
    Aztec = 1<<2,
    Corpse = 1<<3,
    Void = 1<<4
}

public class SpellManager : MonoBehaviour {

    [NonSerialized]
    public static SpellManager Instance;

    public bool SmartCast;
    public float ShamanMaxDistance;
    [Space]
    public Spell[] Spells;

    [ReadOnly]
    public Spell SelectedSpell;
    //[ReadOnly]
    public int Souls = 0;

    private SelectionManager _selection;
    private OrderManager _order;

    private int _oldSpellLength;
    private string[] _spellButtons;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        _selection = GetComponent<SelectionManager>();
        _order = GetComponent<OrderManager>();
        SetSpellButtons();
    }

    private void Update()
    {
        if (SmartCast)
            Shortcuts();

        if (SelectedSpell)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(1))
                {
                    SelectedSpell.StopCasting();
                    SelectedSpell = null;
                }
                else
                {
                    CastingSpell(Camera.main);
                }
            }
            else
            {
                SelectedSpell.CastUpdate(Vector3.zero, TargetEnum.Void, null, true, true);
            }
        }
    }

    public void CastSpell (Spell spell)
    {
        if (SelectedSpell != spell)
        {
            SelectedSpell = spell;
            if (!spell.StartCasting())
                SelectedSpell = null;
        }
        else
        {
            if (SelectedSpell.HoldBehaviour)
                SelectedSpell.ExecuteSpell(null, Vector3.zero);
            else
                SelectedSpell.StopCasting();
            SelectedSpell = null;
        }
    }

    private void CastingSpell (Camera cam, bool bypassClick = false)
    {
        Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * cam.nearClipPlane);

        RaycastHit hit;
        if (Physics.Raycast(pos, cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * cam.farClipPlane) - pos, out hit, Mathf.Infinity, _order.GroundLayer))
        {
            Vector3 worldPoint = hit.point;
            if (((worldPoint - _selection.Shaman.transform.position).magnitude <= ShamanMaxDistance) || !SelectedSpell.UseShamanCastDistance)
            {
                TargetEnum targetType = TargetEnum.Void;
                if (Physics.Raycast(pos, cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * cam.farClipPlane) - pos, out hit, Mathf.Infinity, _selection.SelectableLayer))
                {
                    if (hit.transform.tag == "Aztec")
                        targetType = TargetEnum.Aztec;
                    else if (hit.transform.tag == "Shaman")
                        targetType = TargetEnum.Shaman;
                    else if (hit.transform.tag == "Corpse")
                        targetType = TargetEnum.Corpse;
                    else if (hit.transform.tag == "Conquistador")
                        targetType = TargetEnum.Conquistador;
                }

                if (!bypassClick)
                    SelectedSpell.CastUpdate(worldPoint, targetType, (targetType == TargetEnum.Void) ? null : hit.transform.gameObject);

                if (SelectedSpell.HoldBehaviour && (Input.GetMouseButtonDown(0) || bypassClick))
                {
                    if ((targetType != TargetEnum.Void) && ((SelectedSpell.Targets & targetType) != 0))
                    {
                        SelectedSpell.ExecuteSpell(hit.transform.gameObject, Vector3.zero);
                    }
                    else if ((SelectedSpell.Targets & TargetEnum.Void) != 0)
                    {
                        SelectedSpell.ExecuteSpell(null, worldPoint);
                    }
                    else
                    {
                        SelectedSpell.StopCasting();
                    }

                    SelectedSpell = null;
                }
            }
            else
            {
                if (SelectedSpell.HoldBehaviour && (Input.GetMouseButtonDown(0) || bypassClick))
                {
                    SelectedSpell.StopCasting();
                    SelectedSpell = null;
                }
                else
                {
                    SelectedSpell.CastUpdate(worldPoint, TargetEnum.Void, null, true);
                }
            }
        }
    }

    private void Shortcuts ()
    {
        if (_oldSpellLength != Spells.Length)
            SetSpellButtons();

        Spell spell = null;
        for (int i = 0; i < Spells.Length; i++)
        {
            if (Input.GetButtonDown(_spellButtons[i]))
            {
                if (Spells[i].GetAvailable())
                {
                    spell = Spells[i];
                    break;
                }
            }

            if (SelectedSpell.HoldBehaviour)
            {
                if (Spells[i] == SelectedSpell)
                {
                    if (Input.GetButtonUp(_spellButtons[i]))
                    {
                        SelectedSpell.StopCasting();
                        SelectedSpell = null;
                        break;
                    }
                }
            }
        }

        if (spell)
        {
            if (!spell.HoldBehaviour)
            {
                SelectedSpell = spell;
                CastingSpell(Camera.main, true);
            }
            else
            {
                SelectedSpell = spell;
                spell.ExecuteSpell(null, Vector3.zero);
            }
        }
    }

    private void SetSpellButtons ()
    {
        _oldSpellLength = Spells.Length;
        _spellButtons = new string[_oldSpellLength];
        for (int i = 0; i < _spellButtons.Length; i++)
        {
            _spellButtons[i] = "Spell 0" + (i + 1).ToString();
        }
    }
}
