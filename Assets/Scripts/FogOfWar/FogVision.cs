using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogVision : MonoBehaviour, IVisionElement
{
    public Vector3 Position => _transform.position;
    public float VisionRange => _visionRange;

    [SerializeField]
    private float _visionRange;

    private Transform _transform;

    void Start () {
        _transform = transform;
        FogManager.Instance.RegisterElement(this);

        var visionChild = new GameObject("Vision Trigger");
        visionChild.transform.parent = _transform;
        visionChild.transform.localPosition = Vector3.zero;
        var trigger = visionChild.AddComponent<SphereCollider>();
        trigger.isTrigger = true;
        trigger.radius = VisionRange;
        visionChild.layer = 18;
	}

    private void OnDestroy()
    {
        FogManager.Instance.DeleteElement(this);
    }
}
