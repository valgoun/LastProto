using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDummyObject : MonoBehaviour, IAiVisible
{
    public Vector3 Position => _transform.position;
    public Transform Transform => _transform;
    public GameObject GameObject => gameObject;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }
}
