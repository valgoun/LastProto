using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour {

    [NonSerialized]
    public static SelectionManager Instance;

    public LayerMask SelectableLayer;
    public LayerMask GroundLayer;
    [NonSerialized]
    public List<Unit> SelectedElements = new List<Unit>();

    [NonSerialized]
    public bool IsSelecting;
    [NonSerialized]
    public Vector3 Origin;
    [NonSerialized]
    public Vector3 Destination;
    [NonSerialized]
    public Unit Shaman;
    [NonSerialized]
    public Unit[] Aztecs = new Aztec[3];

    private Collider[] _results = new Collider[500];

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(this);
    }

    // Update is called once per frame
    void Update () {
        //Shortcuts();

        if (Input.GetMouseButtonDown(0))
        {
            StartSelection(Camera.main);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            IsSelecting = false;
        }

        if (Input.GetMouseButton(0) && IsSelecting)
        {
            Selecting(Camera.main);
        }
	}

    private void StartSelection(Camera cam)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            CleanSelection();

            Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * cam.nearClipPlane);
            RaycastHit hit;
            if (Physics.Raycast(pos, cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * cam.farClipPlane) - pos, out hit, Mathf.Infinity, GroundLayer))
            {
                Origin = hit.point;
                IsSelecting = true;
                if (Physics.Raycast(pos, cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * cam.farClipPlane) - pos, out hit, Mathf.Infinity, SelectableLayer))
                {
                    Unit unit = hit.transform.GetComponent<Unit>();
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
            Destination = hit.point;
            Vector3 dir = cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * cam.farClipPlane) - pos;
            if (Origin == Destination)
            {
                if (Physics.Raycast(pos, dir, out hit, Mathf.Infinity, SelectableLayer))
                {
                    Unit unit = hit.transform.GetComponent<Unit>();
                    if (unit)
                    {
                        newElements.Add(unit);
                    }
                }
            }
            else
            {
                dir = Destination - Origin + -dir.normalized * 1;
                dir /= 2;
                
                int length = Physics.OverlapBoxNonAlloc(Origin + dir, new Vector3(Mathf.Abs(dir.x), Mathf.Abs(dir.y), Mathf.Abs(dir.z)), _results, Quaternion.identity, SelectableLayer);

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

    public void RegisterAztec (Unit unit)
    {
        bool registered = false;
        for (int i = 0; i < Aztecs.Length; i++)
        {
            if (!Aztecs[i])
            {
                registered = true;
                Aztecs[i] = unit;
                break;
            }
        }

        if (!registered)
        {
            Debug.LogWarning("Careful, you currently have more Aztecs than what the game is designed for.");
        }
    }

    private void Shortcuts ()
    {
        if (Input.GetButtonDown("Selection Shaman"))
        {
            SelectShaman();
        }
        else if (Input.GetButtonDown("Selection Aztec 01"))
        {
            SelectGhoul(0);
        }
        else if (Input.GetButtonDown("Selection Aztec 02"))
        {
            SelectGhoul(1);
        }
        else if (Input.GetButtonDown("Selection Aztec 03"))
        {
            SelectGhoul(2);
        }
        else if (Input.GetButtonDown("Selection All"))
        {
            SelectAll();
        }
    }

    public void SelectShaman ()
    {
        if (Shaman)
        {
            CleanSelection();
            SelectedElements.Add(Shaman);
            Shaman.Select();
        }
    }

    public void SelectGhoul (int index)
    {
        if (Aztecs[index])
        {
            CleanSelection();
            SelectedElements.Add(Aztecs[index]);
            Aztecs[index].Select();
        }
    }

    public void SelectAll ()
    {
        CleanSelection();

        if (Shaman)
        {
            SelectedElements.Add(Shaman);
            Shaman.Select();
        }

        foreach (Unit unit in Aztecs)
        {
            if (unit)
            {
                SelectedElements.Add(unit);
                unit.Select();
            }
        }
    }

    public void CleanSelection()
    {
        foreach (var select in SelectedElements)
        {
            if (select)
                select.UnSelect();
        }
        SelectedElements.Clear();
    }
}
