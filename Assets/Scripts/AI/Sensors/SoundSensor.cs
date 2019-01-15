using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundSensor", menuName = "Sensor/Sound")]
public class SoundSensor : Sensor
{
    public LayerMask EaringLayer;

    public override int UpdateSensor(AIBrain brain, List<Stimulus> stimuli)
    {
        var position = brain.transform.position;
        int activeStimuli = 0;

        var EnemyStimuli = stimuli.Where(x => x.Type == StimulusType.Sound);
        var candidats = brain.ProcessedElements.Where(x => x is IAiSound).Select(x => x as IAiSound);

        foreach (var candidat in candidats)
        {
            var elementPosition = candidat.Position;
            float distance = Vector3.Distance(elementPosition, position);
            if (distance > candidat.Range)
                continue;

            var elementDirection = Vector3.Normalize(elementPosition - position);

            if (candidat.GoThroughWalls || !Physics.Raycast(position, elementDirection, distance, EaringLayer))
            {
                activeStimuli++;
                var stimulus = EnemyStimuli.Where(x => x.GetData<SoundData>().SoundOrigin == candidat.GameObject).FirstOrDefault();
                if (stimulus == null)
                {
                    stimulus = new Stimulus
                    {
                        Type = StimulusType.Sound,
                        Data = new SoundData { SoundOrigin = candidat.GameObject }
                    };
                    stimuli.Add(stimulus);
                }
                stimulus.Position = candidat.Position;
                stimulus.TimeLeft = candidat.StimuliLifetime;
            }
        }

        return activeStimuli;
    }

    public override void OnGizmos(Transform transform)
    {

    }
}

