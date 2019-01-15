using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitter : MonoBehaviour, IAiSound
{
    public bool SoundGoThroughWalls;
    public float SoundRange;
    public float SoundStimuliLifetime;

    public bool GoThroughWalls => SoundGoThroughWalls;

    public float Range => SoundRange;

    public Vector3 Position => transform.position;

    public Transform Transform => transform;

    public GameObject GameObject => gameObject;

    public float StimuliLifetime => SoundStimuliLifetime;

    private void Start()
    {
        AIManager.Instance.AddElement(this);
    }

    private void OnDestroy()
    {
        AIManager.Instance.RemoveElement(this);
    }
}
