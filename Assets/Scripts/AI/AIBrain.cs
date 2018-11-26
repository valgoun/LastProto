using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public class AIBrain : MonoBehaviour
{
    [TabGroup("General")]
    public float ProcessRadius;
    [TabGroup("General")]
    public float AlertLevelIncreaseRate;
    [TabGroup("General")]
    public float AlertLevelDecreaseRate;

    [TabGroup("Investigation")]
    public float WanderRadius;

    [TabGroup("Combat")]
    public float MinAimDistance;
    [TabGroup("Combat")]
    public float MaxAimDistance;
    [TabGroup("Combat")]
    public float AimRotationSpeed;
    [TabGroup("Combat")]
    public float AimDuration;
    [TabGroup("Combat")]
    public float ReloadDuration;
    [TabGroup("Combat")]
    public float ShootAngle;


    [TabGroup("Combat")]
    public float ShootProbality;
    [TabGroup("Combat")]
    public GameObject BulletPrefab;


    public Stimulus BestStimulus => _bestStimulus;
    public IReadOnlyList<Stimulus> Stimuli => _stimuli.AsReadOnly();
    public IReadOnlyList<IAiVisible> ProcessedElements => _processedElements.AsReadOnly();
    public Sensor[] Sensors;
    public float AlertLevel => _alertLevel;
    public NavMeshAgent Agent => _agent;
    public bool Reloaded => _reloaded;

    private NavMeshAgent _agent;
    private Stimulus _bestStimulus;
    private List<Stimulus> _stimuli = new List<Stimulus>();
    private List<IAiVisible> _processedElements = new List<IAiVisible>();
    private List<ISensor> _sensors = new List<ISensor>();
    private float _alertLevel;
    private Animator _brainStates;
    private Animator _subStates;
    private bool _reloaded = true;
    private Transform _transform;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _sensors.AddRange(Sensors);
        _subStates = GetComponent<Animator>();
        _transform = transform;
        _brainStates = _transform.GetChild(0).GetComponent<Animator>();
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
        var candidates = Physics.OverlapSphere(_transform.position, ProcessRadius);

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
        Vector3 pos = _transform.position;
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

    public void Shoot(GameObject Target)
    {
        _reloaded = false;
        var forward = _transform.forward;
        var perfectDirection = (Target.transform.position - _transform.position).normalized;

        if (Vector3.Angle(forward, perfectDirection) <= ShootAngle)
        {
            if (UnityEngine.Random.Range(0f, 1.0f) <= ShootProbality)
            {
                var bullet = Instantiate(BulletPrefab, _transform.position, Quaternion.LookRotation(perfectDirection));
                bullet.GetComponent<Bullet>().SetTarget(Target.transform, Vector3.up * 0.75f);
            }
            else
            {
                var targetPosition = Target.transform.position + UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(0.7f, 2.3f);
                var targetDirection = (targetPosition - _transform.position).normalized;
                var bullet = Instantiate(BulletPrefab, _transform.position, Quaternion.LookRotation(targetDirection));
            }

        }
        else
        {
            var bullet = Instantiate(BulletPrefab, _transform.position, Quaternion.LookRotation(forward));
        }
    }

    public void Reload()
    {
        StartCoroutine(InternalReload());
    }

    private IEnumerator InternalReload()
    {
        yield return new WaitForSeconds(ReloadDuration);
        _reloaded = true;
    }
}
