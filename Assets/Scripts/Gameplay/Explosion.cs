using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Explosion : MonoBehaviour {

    [NonSerialized]
    public float Radius;
    public LayerMask SelectableLayer;
    
    // Use this for initialization
	void Start () {
        Collider[] results = new Collider[100];
        results = Physics.OverlapSphere(transform.position, Radius, SelectableLayer);
        foreach(Collider col in results)
        {
            if (col.tag == "Conquistador")
            {
                col.transform.parent.gameObject.GetComponent<Conquistador>().KillMe();
            }
            else if (col.tag == "Destroyable")
            {
                Destroy(col.gameObject);
            }
        }
	}
}
