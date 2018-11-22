using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleControl : MonoBehaviour {

    public float Speed = 10;
	
	// Update is called once per frame
	void Update () {
        Vector3 move = Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal");

        transform.Translate(move * Time.deltaTime * Speed);
	}
}
