using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitSelectionCircle : MonoBehaviour {

    private Unit _unitScript;
    public SpriteRenderer selectionCircleSprite;

    private void Awake()
    {
        _unitScript = GetComponent<Unit>();
    }

    // Update is called once per frame
    void Update ()
    {
		if(_unitScript.Selected && !selectionCircleSprite.enabled)
            selectionCircleSprite.enabled = true;

        if (!_unitScript.Selected && selectionCircleSprite.enabled)
            selectionCircleSprite.enabled = false;
    }
}
