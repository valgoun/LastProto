using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Aztec" || other.tag == "Shaman")
        {
            Unit unit = other.transform.parent.GetComponent<Unit>();
            if (unit)
            {
                unit.IsVisible = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Aztec" || other.tag == "Shaman")
        {
            Unit unit = other.transform.parent.GetComponent<Unit>();
            if (unit)
            {
                unit.IsVisible = true;
            }
        }
    }
}
