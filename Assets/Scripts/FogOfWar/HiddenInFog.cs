using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenInFog : MonoBehaviour {

    private List<IVisionElement> _elementSeeingUs = new List<IVisionElement>();
    private List<IFogVisionNotifiable> _notificationList = new List<IFogVisionNotifiable>();

	// Use this for initialization
	void Start () {
        GetComponentsInChildren<IFogVisionNotifiable>(true, _notificationList);
	}

    private void OnTriggerEnter(Collider other)
    {
        var component = other.transform.parent.GetComponent<IVisionElement>();
        if(component != null)
        {
            _elementSeeingUs.Add(component);
            if (_elementSeeingUs.Count == 1.0)
                _notificationList.ForEach(x => x.OnVisionEnter());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var component = other.transform.parent.GetComponent<IVisionElement>();
        if (component != null)
        {
            _elementSeeingUs.Remove(component);
            if (_elementSeeingUs.Count == 0.0)
                _notificationList.ForEach(x => x.OnVisionExit());
        }
    }
}

public interface IFogVisionNotifiable
{
    void OnVisionEnter();
    void OnVisionExit();
}
