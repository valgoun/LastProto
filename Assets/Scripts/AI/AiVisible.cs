using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAiVisible
{
    Vector3 Position { get; }
    Transform Transform { get; }
    GameObject GameObject { get; }
    float StimuliLifetime { get; }
}

public interface IAiFoe : IAiVisible
{
    bool IsVisible { get; }
}

public interface IAiFriend : IAiVisible
{
    TransmissionData Data { get; }
}

public interface IAiSound : IAiVisible
{
    bool GoThroughWalls { get; }
    float Range { get; }
}