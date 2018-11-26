using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBrain : MonoBehaviour
{

    public float ProcessRadius;
    public float AlertLevelIncreaseRate;
    public float AlertLevelDecreaseRate;

    public Stimulus BestStimulus => _bestStimulus;
    public IReadOnlyList<Stimulus> Stimuli => _stimuli.AsReadOnly();
    public IReadOnlyList<IAiVisible> ProcessedElements => _processedElements.AsReadOnly();
    public Sensor[] Sensors;
    public float AlertLevel => _alertLevel;
    public NavMeshAgent Agent => _agent;

    private NavMeshAgent _agent;
    private Stimulus _bestStimulus;
    private List<Stimulus> _stimuli = new List<Stimulus>();
    private List<IAiVisible> _processedElements = new List<IAiVisible>();
    private List<ISensor> _sensors = new List<ISensor>();
    private float _alertLevel;
    private Animator _brainStates;
    private Animator _subStates;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _sensors.AddRange(Sensors);
        _subStates = GetComponent<Animator>();
        _brainStates = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateElements();

        UpdateStimuli();

        UpdateSensorsAndAlertLevel();

        SelectBestStimulus();

        _brainStates.SetFloat("Alert", _alertLevel);
    }

    private void UpdateElements()
    {
        _processedElements.Clear();
        var candidates = Physics.OverlapSphere(transform.position, ProcessRadius);

        foreach (var element in candidates)
        {
            var component = element.GetComponent<IAiVisible>();
            component = component ?? element.GetComponentInParent<IAiVisible>();
            component = component ?? element.GetComponentInChildren<IAiVisible>();
            if (component != null && !_processedElements.Contains(component))
                _processedElements.Add(component);
        }
    }

    private void SelectBestStimulus()
    {
        Vector3 pos = transform.position;
        if (_stimuli.Count > 0)
            _bestStimulus = _stimuli.Aggregate((x, y) => Vector3.SqrMagnitude(pos - x.Position) < Vector3.SqrMagnitude(pos - y.Position) ? x : y);
        else
            _bestStimulus = null;
    }

    private void UpdateSensorsAndAlertLevel()
    {
        int newStimuli = 0;
        foreach (var sensor in _sensors)
        {
            newStimuli += sensor.UpdateSensor(this, _stimuli);
        }

        if (newStimuli > 0)
            _alertLevel += Time.deltaTime * AlertLevelIncreaseRate;
        else
            _alertLevel -= Time.deltaTime * AlertLevelDecreaseRate;
        _alertLevel = Mathf.Clamp01(_alertLevel);
    }

    private void UpdateStimuli()
    {
        float deltaTime = Time.deltaTime;

        foreach (var stimulus in _stimuli)
        {
            stimulus.TimeLeft -= deltaTime;
        }

        _stimuli.RemoveAll(x => x.TimeLeft <= 0.0f);
    }

    private void OnDrawGizmosSelected()
    {
        foreach (var sensor in Sensors)
        {
            sensor.OnGizmos(transform);
        }
    }
}
