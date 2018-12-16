using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideLightInFog : MonoBehaviour , IFogVisionNotifiable
{
    private MeshRenderer _renderer;

    public void OnVisionEnter()
    {
        _renderer.enabled = true;
    }

    public void OnVisionExit()
    {
        _renderer.enabled = false;
    }

    void Awake () {
        _renderer = GetComponent<MeshRenderer>();
        _renderer.enabled = false;
	}
}
