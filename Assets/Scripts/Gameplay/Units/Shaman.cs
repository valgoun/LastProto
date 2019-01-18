using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaman : Unit {

	// Use this for initialization
	protected override void Start () {
        base.Start();
        SelectionManager.Instance.RegisterShaman(this);
	}

    protected override void Killed()
    {
        if (!Invincible)
            DebugUI.Instance.GameOver();
        base.Damage();
    }
}
