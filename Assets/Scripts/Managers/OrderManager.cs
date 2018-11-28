using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OrderManager : MonoBehaviour {

    public LayerMask GroundLayer;

    private SelectionManager _selection;
    
    // Use this for initialization
	void Start () {
        _selection = GetComponent<SelectionManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            MoveOrder(Camera.main);
        }
    }

    public void MoveOrder (Camera cam)
    {
        foreach(var select in _selection.SelectedElements)
        {
            if (select)
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * cam.nearClipPlane);
                    RaycastHit hit;
                    if (Physics.Raycast(pos, cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * cam.farClipPlane) - pos, out hit, Mathf.Infinity, GroundLayer))
                    {
                        RaycastHit nextHit;
                        if (Physics.Raycast(pos, cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * cam.farClipPlane) - pos, out nextHit, Mathf.Infinity, _selection.SelectableLayer))
                        {
                            select.Follow(nextHit.transform.parent);
                        }
                        else
                        {
                            select.MoveTo(hit.point);
                        }
                    }
                }
            }
        }
    }
}
