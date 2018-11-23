using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

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
                Destroy(col.transform.parent.gameObject);
            }
        }
	}
}
