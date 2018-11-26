using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaman : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SpellManager.Instance.Shaman = gameObject;
	}
	
}
