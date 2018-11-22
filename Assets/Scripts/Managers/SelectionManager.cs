using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectionManager : MonoBehaviour {

    public LayerMask SelectableLayer;
    [NonSerialized]
    public Unit SelectedElement;
    
    // Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
        {
            FireSelection(Camera.main);
        }
	}

    void FireSelection(Camera cam)
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
}
