using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSkinMeshInFog : MonoBehaviour , IFogVisionNotifiable
{
    private SkinnedMeshRenderer _renderer;

    public void OnVisionEnter()
    {
        _renderer.enabled = true;
    }

    public void OnVisionExit()
    {
        _renderer.enabled = false;
    }

    void Awake () {
        _renderer = GetComponent<SkinnedMeshRenderer>();
        _renderer.enabled = false;
	}
}
