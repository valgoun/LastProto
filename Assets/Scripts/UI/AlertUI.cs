using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AlertUI : MonoBehaviour {

    public AIBrain Brain;
    private Image _target;

	// Use this for initialization
	void Start () {
        _target = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        _target.fillAmount = Brain.AlertLevel;
	}
}
