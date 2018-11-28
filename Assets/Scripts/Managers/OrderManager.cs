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
        for(int i = 0; i < _selection.SelectedElements.Count; i++)
        {
            if (_selection.SelectedElements[i])
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
                            _selection.SelectedElements[i].Follow(nextHit.transform.parent, i, _selection.SelectedElements.Count);
                        }
                        else
                        {
                            _selection.SelectedElements[i].MoveTo(hit.point, i, _selection.SelectedElements.Count);
                        }
                    }
                }
            }
        }
    }
}
