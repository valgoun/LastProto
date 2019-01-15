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
        if (_selection.Shaman)
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
                        _selection.Shaman.Follow(nextHit.transform, 0, 1);
                    }
                    else
                    {
                        _selection.Shaman.MoveTo(hit.point, 0, 1);
                    }
                }
            }
        }
    }
}
