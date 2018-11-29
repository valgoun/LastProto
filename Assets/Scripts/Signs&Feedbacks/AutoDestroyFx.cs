using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyFx : MonoBehaviour {

    private ParticleSystem _particleSystem;

	// Use this for initialization
	void Start () {
        _particleSystem = GetComponent<ParticleSystem>();
        Debug.Log(_particleSystem.main.duration);
      Destroy(gameObject, _particleSystem.main.duration);
    }
}
