using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    private Spell _selectedSpell;
    private SelectionManager _selection;
    private OrderManager _order;

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
    }

    private void Update()
    {
        if (SmartCast)
            Shortcuts();

        if (_selectedSpell)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(1))
                {
                    _selectedSpell = null;
                    _selectedSpell.StopCasting();
                }
                else
                {
                    CastingSpell(Camera.main);
                }
            }
        }
    }

    public void CastSpell (Spell spell)
    {
        if (_selectedSpell != spell)
        {
            _selectedSpell = spell;
            spell.StartCasting();
        }
        else
        {
            _selectedSpell = null;
        }
    }

    private void CastingSpell (Camera cam, bool bypassClick = false)
    {
        Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * cam.nearClipPlane);

        RaycastHit hit;
        if (Physics.Raycast(pos, cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * cam.farClipPlane) - pos, out hit, Mathf.Infinity, _order.GroundLayer))
        {
            Vector3 worldPoint = hit.point;
            if ((worldPoint - _selection.Shaman.transform.position).magnitude <= ShamanMaxDistance)
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

                if (Input.GetMouseButtonDown(0) || bypassClick)
                {
                    if ((targetType != TargetEnum.Void) && ((_selectedSpell.Targets & targetType) != 0))
                    {
                        _selectedSpell.ExecuteSpell(hit.transform.parent.gameObject, Vector3.zero);
                    }
                    else if ((_selectedSpell.Targets & TargetEnum.Void) != 0)
                    {
                            _selectedSpell.ExecuteSpell(null, worldPoint);
                    }

                    _selectedSpell = null;
                }
            }
            else if (Input.GetMouseButtonDown(0) || bypassClick)
            {
                _selectedSpell = null;
            }
        }
    }

    private void Shortcuts ()
    {
        Spell spell = null;
        for (int i = 0; i < Spells.Length; i++)
        {
            if (Input.GetButtonDown("Spell 0" + (i + 1).ToString()))
            {
                if (Spells[i].CurrentCooldown <= Time.time)
                {
                    spell = Spells[i];
                    break;
                }
            }
        }

        if (spell)
        {
            _selectedSpell = spell;
            CastingSpell(Camera.main, true);
        }
    }
}
