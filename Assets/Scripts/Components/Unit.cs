using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour {

    [Header("Read Only")]
    public bool Selected;

    [NonSerialized]
    public NavMeshAgent NavAgent;

    // Use this for initialization
    void Start () {
        NavAgent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Select ()
    {
        Selected = true;
    }

    public void UnSelect ()
    {
        Selected = false;
    }
}
