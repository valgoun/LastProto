using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorGlowOnMouseOver : MonoBehaviour {

    private CursorManager _cursorManager;

	// Use this for initialization
	void Start () {
        _cursorManager = CursorManager.cursorManagerInst;

    }

    private void OnMouseEnter()
    {
        _cursorManager.CursorGlow(true);
    }

    private void OnMouseExit()
    {
        _cursorManager.CursorGlow(false);
    }
}
