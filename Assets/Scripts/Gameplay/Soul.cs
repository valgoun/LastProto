using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    public float DrawnInRadius;
    public float DrawnInGradient;
    public float DrawnInForce;
    public float AbsorbedRadius;

    // Update is called once per frame
    void Update()
    {
        Vector3 sham = SelectionManager.Instance.Shaman.transform.position;
        Vector3 dir = sham - transform.position;
        float magn = dir.magnitude;
        dir.Normalize();
        if (magn <= AbsorbedRadius)
        {
            SpellManager.Instance.Souls++;
            Destroy(gameObject);
        }
        else if (magn <= DrawnInRadius)
        {
            float grad = Mathf.Pow((1 - (magn / DrawnInRadius)), DrawnInGradient);
            transform.position += dir * grad * DrawnInForce * Time.deltaTime;
        }
    }
}
