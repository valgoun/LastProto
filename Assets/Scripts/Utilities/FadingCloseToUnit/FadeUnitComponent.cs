using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using System;

[Serializable]
public struct FadeUnit : IComponentData
{
    public float3 Position;
}

public class FadeUnitComponent : ComponentDataWrapper<FadeUnit> {}
