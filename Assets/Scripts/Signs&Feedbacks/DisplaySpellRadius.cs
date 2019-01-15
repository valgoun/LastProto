using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaySpellRadius : MonoBehaviour {

    private LineRenderer _LineRend;

    private SpellManager _SpellManager;

    public int segments = 50;

    private float _radius;

    private GameObject _shaman;

    // Use this for initialization
    void Start () {

        _LineRend = GetComponent<LineRenderer>();
        _SpellManager = transform.parent.GetComponent<SpellManager>();
        _radius = _SpellManager.ShamanMaxDistance;

        _shaman = FindObjectOfType<Shaman>().gameObject;

        DrawCircle();
        ToggleCircleVisibility(false);
    }
	
	// Update is called once per frame
	void Update () {

        if(_shaman != null)
            transform.position = _shaman.transform.position;


        if(!SpellManager.Instance.SelectedSpell)
        {
            ToggleCircleVisibility(false);
        }
    }

    public void DrawCircle()
    {
        _LineRend.positionCount = segments;

        float x;
        float z;

        float change = 2 * Mathf.PI / segments;
        float angle = change;

        for (int i = 0; i < segments; i++)
        {
            x = Mathf.Sin(angle) * _radius;
            z = Mathf.Cos(angle) * _radius;

            _LineRend.SetPosition(i, new Vector3(x, 0, z));

            angle += change;
        }
    }

    public void ToggleCircleVisibility(bool visibility)
    {
        _LineRend.enabled = visibility;
    }
}
