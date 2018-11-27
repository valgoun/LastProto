using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float LifeTime;
    public float Speed;
    public event System.Action OnBulletDestroy;

    private Transform _transform;
    private Transform _target;
    private Vector3 _offSet;
    private float _timer = 0.0f;
    private bool _isDead = false;


    public void SetTarget(Transform target, Vector3 offSet)
    {
        _target = target;
        _offSet = offSet;
    }

    private void Awake()
    {
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDead)
            return;
        if (_target != null)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, _target.position + _offSet, Time.deltaTime * Speed);
            _transform.LookAt(_target.position + _offSet);
            if (Vector3.Distance(_transform.position, _target.position + _offSet) < 0.1f)
            {
                _isDead = true;
            }
        }
        else
        {
            _timer += Time.deltaTime;
            if (_timer >= LifeTime)
                Destroy(gameObject);
            _transform.position += _transform.forward * Speed * Time.deltaTime;
        }
    }
}
