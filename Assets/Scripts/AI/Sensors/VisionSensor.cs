using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VisionSensor", menuName = "Sensor/Vision")]
public class VisionSensor : Sensor
{
    public float VisionAngle;
    public float VisionDistance;

    public override int UpdateSensor(AIBrain brain, List<Stimulus> stimuli)
    {
        var forward = brain.transform.forward;
        var position = brain.transform.position;
        var dotLimit = Mathf.Cos(Mathf.Deg2Rad * VisionAngle * 0.5f);
        int activeStimulus = 0;
        foreach (var candidat in brain.ProcessedElements)
        {
            var elementPosition = candidat.Position;
            var elementDirection = Vector3.Normalize(elementPosition - position);
            var dot = Vector3.Dot(forward, elementDirection);

            if(dot >= dotLimit)
            {
                activeStimulus++;
                var stimulus = stimuli.Find(x => x.Origin == candidat.GameObject);
                if(stimulus == null)
                {
                    stimulus = new Stimulus { Origin = candidat.GameObject, Type = StimulusType.SightEnemy };
                    stimuli.Add(stimulus);
                }
                stimulus.Position = candidat.Position;
                stimulus.TimeLeft = 1.0f;
            }
        }

        return activeStimulus;
    }
}
