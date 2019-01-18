using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;

public class FadingSystem : JobComponentSystem
{

    struct SetFadeUnit : IJobProcessComponentData<FadeUnit, Position>
    {
        public void Execute(ref FadeUnit data, ref Position position)
        {
            data.Position = position.Value;
        }
    }

    struct SelectBestUnit : IJobProcessComponentData<FadingObject, Position>
    {
        [ReadOnly] public ComponentDataArray<FadeUnit> Positions;

        public void Execute([WriteOnly] ref FadingObject data, [ReadOnly] ref Position position)
        {
            if (Positions.Length == 0)
                return;

            float3 bestPos = Positions[0].Position;
            float bestDistance = math.distance(bestPos, position.Value);
            for (int i = 1; i < Positions.Length; i++)
            {
                float3 pos = Positions[i].Position;
                float dist = math.distance(pos, position.Value);
                if (bestDistance > dist)
                {
                    bestPos = pos;
                    bestDistance = dist;
                }
            }

            data.Position = bestPos;
        }
    }


    private ComponentGroup _unitGroup;
    private ComponentGroup _fadeUnit;


    protected override void OnCreateManager()
    {
        _unitGroup = GetComponentGroup(typeof(MeshRenderer), typeof(FadingObject), typeof(Position));
        _fadeUnit = GetComponentGroup(typeof(FadeUnit), typeof(Position));
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

        var renderers = _unitGroup.GetComponentArray<MeshRenderer>();
        var fadingData = _unitGroup.GetComponentDataArray<FadingObject>();
        for (int i = 0; i < renderers.Length; i++)
        {
            foreach (var material in renderers[i].materials)
            {
                Vector3 data = fadingData[i].Position;
                material.SetVector("_position", data);
            }
        }

        var PositionJob = new SetFadeUnit();
        var SelectJob = new SelectBestUnit
        {
            Positions = _fadeUnit.GetComponentDataArray<FadeUnit>()
        };

        var handle = PositionJob.Schedule(this, inputDeps);
        return SelectJob.Schedule(this, handle);
    }
}
