using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAiVisible
{
    Vector3 Position { get; }
    Transform Transform { get; }
    GameObject GameObject { get; }
    bool IsVisible { get; }
}
