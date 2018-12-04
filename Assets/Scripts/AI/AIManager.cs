using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Jobs;


public class AIManager : MonoBehaviour {

    public static AIManager Instance => _instance;
    public IReadOnlyList<IAiVisible> ProcessedElements => _processedElements.AsReadOnly();

    private static AIManager _instance;
    private List<IAiVisible> _processedElements = new List<IAiVisible>();
    private List<AIBrain> _brains = new List<AIBrain>();
    private NativeList<float3> _processedPositions;

    struct ProcessedListCreationJob : IJobParallelForFilter
    {
        [ReadOnly] public float3                   _position;
        [ReadOnly] public float                    _radius;
        [ReadOnly] public NativeArray<float3>      _processedPositions;

        public bool Execute(int index)
        {
            return math.distance(_processedPositions[index], _position) < _radius;
        }
    }

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        _processedPositions = new NativeList<float3>(Allocator.Persistent);
    }

    private void Update()
    {
        //Update position array
        int i = 0;
        for (i = 0; i < _processedPositions.Length; i++)
        {
            _processedPositions[i] = _processedElements[i].Position;
        }
        for(; i < _processedElements.Count; i++)
        {
            _processedPositions.Add(_processedElements[i].Position);
        }

        //Update brain
        NativeArray<JobHandle> jobs             = new NativeArray<JobHandle>(_brains.Count, Allocator.TempJob);
        List<NativeList<int>> filteredResult    = new List<NativeList<int>>();
        for (int x = 0; x < _brains.Count; x++)
        {
            var job = new ProcessedListCreationJob
            {
                _position = _brains[x].Position,
                _radius = _brains[x].ProcessRadius,
                _processedPositions = _processedPositions
            };
            filteredResult.Add(new NativeList<int>(Allocator.TempJob));
            jobs[x] = job.ScheduleAppend(filteredResult[x], _processedPositions.Length, 1);
        }

        JobHandle.CompleteAll(jobs);

        for (int x = 0; x < _brains.Count; x++)
        {
            _brains[x].SetProcessedIndices(filteredResult[x]);
        }
        jobs.Dispose();
    }

    private void OnDestroy()
    {
        _processedPositions.Dispose();
    }

    public void AddAI(AIBrain brain)
    {
        _brains.Add(brain);
        _processedElements.Add(brain);
    }

    public void RemoveAI(AIBrain brain)
    {
        _brains.Remove(brain);
        _processedElements.Remove(brain);
    }

    public void AddElement(IAiVisible element)
    {
        _processedElements.Add(element);
    }

    public void RemoveElement(IAiVisible element)
    {
        _processedElements.Remove(element);
    }
}
