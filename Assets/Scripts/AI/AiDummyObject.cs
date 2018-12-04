using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDummyObject : MonoBehaviour, IAiFoe
{
    public float StimuliLifetimeWhenSeen;

    public Vector3 Position => _transform.position;
    public Transform Transform => _transform;
    public GameObject GameObject => gameObject;
    public bool IsVisible => true;
    public float StimuliLifetime => StimuliLifetimeWhenSeen;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }
}
