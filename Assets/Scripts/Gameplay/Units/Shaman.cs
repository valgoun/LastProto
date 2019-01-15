using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaman : Unit {

	// Use this for initialization
	protected override void Start () {
        base.Start();
        SelectionManager.Instance.Shaman = GetComponent<Unit>();
	}

    public override void Killed()
    {
        if (!Invincible)
            DebugUI.Instance.GameOver();
        base.Killed();
    }
}
