using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stimulus {
    public Vector3 Position;
    public StimulusType Type;
    public float TimeLeft;
    public GameObject Origin;
}

public enum StimulusType
{
    SightEnemy,
    SightSpell
}
