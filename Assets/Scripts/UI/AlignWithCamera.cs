using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignWithCamera : MonoBehaviour {

    private Transform _camera;

	void Start () {
        _camera = Camera.main.transform;
	}
	
	void Update () {
        transform.forward = -_camera.forward;
	}
}
