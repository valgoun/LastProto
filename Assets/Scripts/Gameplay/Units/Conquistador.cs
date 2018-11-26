using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conquistador : MonoBehaviour {

    public GameObject Corpse;

    bool _isQuitting = false;

    void OnApplicationQuit()
    {
        _isQuitting = true;
    }

    private void OnDestroy()
    {
        if (!_isQuitting)
            Instantiate(Corpse, transform.position, transform.rotation);
    }
}
