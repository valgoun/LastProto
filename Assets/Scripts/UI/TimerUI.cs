using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerUI : MonoBehaviour {

    public Transform Image;

    private bool _init;
    private float _start;
    private float _delay;

    public void Initialize (float duration)
    {
        _start = Time.time;
        _delay = duration;
        _init = true;
    }

	// Update is called once per frame
	void Update () {
		if (_init)
        {
            Vector3 scale = Image.localScale;
            scale.x = Mathf.Clamp(1 - ((Time.time - _start) / _delay), 0, 1);
            Image.localScale = scale;
        }
	}
}
