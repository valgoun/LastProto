using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public LineRenderer MyLineRenderer;

    private Rigidbody _rgbd;

    private LayerMask _layer;
    private float _pullSpeed;
    private float _pullSafeDistance;
    private float _addedStunTime;
    private float _range;

    private bool _grabbed = false;
    private Transform _enemy;
    private Vector3 _shift;

    public void Initialize(float range, float travelSpeed, Vector3 direction, LayerMask collisionLayer, float pullSpeed, float pullSafeDistance, float addedStunTime)
    {
        _rgbd = GetComponent<Rigidbody>();
        _rgbd.velocity = direction.normalized * travelSpeed;

        _range = range;
        _layer = collisionLayer;
        _pullSpeed = pullSpeed;
        _pullSafeDistance = pullSafeDistance;
        _addedStunTime = addedStunTime;
    }

    // Update is called once per frame
    void Update()
    {
        MyLineRenderer.SetPosition(1, SelectionManager.Instance.Shaman.transform.position - transform.position);
        Vector3 distance = (SelectionManager.Instance.Shaman.transform.position - transform.position);

        if (_grabbed)
        {
            if (distance.magnitude <= _pullSafeDistance)
            {
                if (_enemy)
                {
                    //_enemy.parent = null;
                    _enemy.GetComponent<AIBrain>().Stun(_addedStunTime);
                }
                Destroy(gameObject);
            }
            else
            {
                _rgbd.velocity = distance.normalized * _pullSpeed;
                _enemy.position = transform.position + _shift;
            }
        }
        else if (distance.magnitude > _range)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_grabbed)
        {
            if (other.tag == "Conquistador")
            {
                _grabbed = true;
                _enemy = other.transform.parent;
                //_enemy.parent = transform;
                _enemy.GetComponent<AIBrain>().Stun(100);
                _shift = _enemy.position - transform.position;
            }
            else if ((1 << other.gameObject.layer & _layer) != 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
