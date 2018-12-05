using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System;
using Unity.Mathematics;

[Serializable]
public struct FadingObject : IComponentData
{
    public float3 Position;
}

public class FadingObjectComponent : ComponentDataWrapper<FadingObject> {}
