using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour {

    public LayerMask SelectableLayer;
    public LayerMask GroundLayer;
    [NonSerialized]
    public List<Unit> SelectedElements = new List<Unit>();

    private Vector3 _origin;
    private bool _selecting;
    private Collider[] _results = new Collider[500];

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            StartSelection(Camera.main);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _selecting = false;
        }

        if (Input.GetMouseButton(0) && _selecting)
        {
            Selecting(Camera.main);
        }
	}

    /*void FireSingleSelection(Camera cam)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (SelectedElement)
                SelectedElement.UnSelect();

            Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * cam.nearClipPlane);
            RaycastHit hit;
            if (Physics.Raycast(pos, cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * cam.farClipPlane) - pos, out hit, Mathf.Infinity, SelectableLayer))
            {
                Unit unit = hit.transform.parent.GetComponent<Unit>();
                if (unit)
                {
                    SelectedElement = unit;
                    SelectedElement.Select();
                    return;
                }
            }

            SelectedElement = null;
        }
    }*/

    private void StartSelection(Camera cam)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            foreach(var select in SelectedElements)
            {
                if(select)
                    select.UnSelect();
            }
            SelectedElements.Clear();

            Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * cam.nearClipPlane);
            RaycastHit hit;
            if (Physics.Raycast(pos, cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * cam.farClipPlane) - pos, out hit, Mathf.Infinity, GroundLayer))
            {
                _origin = hit.point;
                _selecting = true;
                if (Physics.Raycast(pos, cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * cam.farClipPlane) - pos, out hit, Mathf.Infinity, SelectableLayer))
                {
                    Unit unit = hit.transform.parent.GetComponent<Unit>();
                    if (unit)
                    {
                        SelectedElements.Add(unit);
                        unit.Select();
                        return;
                    }
                }
            }
        }
    }

    private void Selecting(Camera cam)
    {
        List<Unit> newElements = new List<Unit>();

        Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * cam.nearClipPlane);
        RaycastHit hit;
        if (Physics.Raycast(pos, cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * cam.farClipPlane) - pos, out hit, Mathf.Infinity, GroundLayer))
        {
            Vector3 dest = hit.point;
            Vector3 dir = cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * cam.farClipPlane) - pos;
            if (_origin == dest)
            {
                if (Physics.Raycast(pos, dir, out hit, Mathf.Infinity, SelectableLayer))
                {
                    Unit unit = hit.transform.parent.GetComponent<Unit>();
                    if (unit)
                    {
                        newElements.Add(unit);
                    }
                }
            }
            else
            {
                dir = dest - _origin + -dir.normalized * 1;
                dir /= 2;
                
                int length = Physics.OverlapBoxNonAlloc(_origin + dir, new Vector3(Mathf.Abs(dir.x), Mathf.Abs(dir.y), Mathf.Abs(dir.z)), _results, Quaternion.identity, SelectableLayer);

                for(int i = 0; i < length; i++)
                {
                    Unit unit = _results[i].transform.parent.GetComponent<Unit>();
                    if (unit)
                    {
                        newElements.Add(unit);
                    }
                }
            }

            for (int i = 0; i < SelectedElements.Count; i++)
            {
                if (newElements.Contains(SelectedElements[i]))
                {
                    newElements.Remove(SelectedElements[i]);
                }
                else
                {
                    if (SelectedElements[i])
                        SelectedElements[i].UnSelect();
                    SelectedElements.Remove(SelectedElements[i]);
                    i--;
                }
            }

            for (int i = 0; i < newElements.Count; i++)
            {
                newElements[i].Select();
                if (newElements[i].gameObject.tag == "Shaman")
                {
                    SelectedElements.Insert(0, newElements[i]);
                }
                else
                {
                    SelectedElements.Add(newElements[i]);
                }
            }
        }
    }
}
