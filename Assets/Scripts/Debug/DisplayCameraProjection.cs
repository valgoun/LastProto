using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class DisplayCameraProjection : MonoBehaviour {

    private Camera _mainCamera;
    private LineRenderer _trapezoid;
    public LayerMask layerMask;

    private void Awake()
    {
        _mainCamera = GetComponent<Camera>();     
        _trapezoid = GetComponent<LineRenderer>();

        _trapezoid.positionCount = 4;
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = _mainCamera.ViewportPointToRay(new Vector3(0F, 0F, 0));
        if (Physics.Raycast(ray, out hit, layerMask))
            _trapezoid.SetPosition(0, hit.point);

        ray = _mainCamera.ViewportPointToRay(new Vector3(0F, 1F, 0));
        if (Physics.Raycast(ray, out hit, layerMask))
            _trapezoid.SetPosition(1, hit.point);

        ray = _mainCamera.ViewportPointToRay(new Vector3(1F, 1F, 0));
        if (Physics.Raycast(ray, out hit, layerMask))
            _trapezoid.SetPosition(2, hit.point);

        ray = _mainCamera.ViewportPointToRay(new Vector3(1F, 0F, 0));
        if (Physics.Raycast(ray, out hit, layerMask))
            _trapezoid.SetPosition(3, hit.point);
    }
}
