using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relic : MonoBehaviour {

    public RelicInfo MyRelic;

    bool _quitting;

    private void OnApplicationQuit()
    {
        _quitting = true;
    }

    private void OnDestroy()
    {
        if (!_quitting)
        {
            RelicManager.Instance.OpenRelic(MyRelic);
        }
    }
}
