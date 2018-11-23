﻿using System.Collections.Generic;
using UnityEngine;

public interface ISensor
{
    int UpdateSensor(AIBrain brain, List<Stimulus> stimuli);

    void OnGizmos(Transform transform);
}

public abstract class Sensor : ScriptableObject, ISensor
{
    public abstract int UpdateSensor(AIBrain brain, List<Stimulus> stimuli);

    public abstract void OnGizmos(Transform transform);
}
