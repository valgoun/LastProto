using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stimulus
{
    public Vector3 Position;
    public StimulusType Type;
    public float TimeLeft;
    public IStimulusData Data;

    public T GetData<T>() where T : class, IStimulusData
    {
        return Data as T;
    }
}

public enum StimulusType
{
    SightEnemy,
    SightSpell,
    Transmission,
    Sound
}

public interface IStimulusData { }

public class EnemyData : IStimulusData
{
    public GameObject EnemyGameObject;
}

public class TransmissionData : IStimulusData
{
    public Stimulus TransmittedStimulus;
    public IAiFriend Emitter;

    public Stimulus GetTrueStimulus()
    {
        if (TransmittedStimulus.Type != StimulusType.Transmission)
            return TransmittedStimulus;
        else
            return TransmittedStimulus.GetData<TransmissionData>().GetTrueStimulus();
    }
}

public class SoundData : IStimulusData
{
    public GameObject SoundOrigin;
}
