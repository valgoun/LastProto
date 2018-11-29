using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RememberMeshInFog : MonoBehaviour, IFogVisionNotifiable
{
    public bool IsVisible
    {
        get
        {
            return _isVisible;
        }

        set
        {
            _isVisible = value;
        }
    }

    private bool _isVisible;
    private MeshRenderer _renderer;

    public void OnVisionEnter()
    {
        _renderer.enabled = _isVisible;
    }

    public void OnVisionExit()
    {
    }

    private void Awake()
    {
        _isVisible = true;
        _renderer = GetComponent<MeshRenderer>();
        _renderer.enabled = false;
    }
}
