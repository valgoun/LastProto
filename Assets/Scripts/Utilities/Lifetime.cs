using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour {

    [SerializeField]
    private float _duration;

    private bool _started = false;
    private float _timestamp;

    public void Start()
    {
       if (!_started)
        {
            StartLifetime(_duration);
        }
    }

    public void StartLifetime (float duration)
    {
        _timestamp = Time.time + duration;
        _started = true;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        if (Time.time >= _timestamp)
            Destroy(gameObject);
	}
}
