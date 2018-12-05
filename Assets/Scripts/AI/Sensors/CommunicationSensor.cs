using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CommunicationSensor", menuName = "Sensor/Communication")]
public class CommunicationSensor : Sensor
{
    public float CommunicationRadius;

    public override int UpdateSensor(AIBrain brain, List<Stimulus> stimuli)
    {

        int activeStimuli = 0;
        var position = brain.transform.position;

        var candidats = brain.ProcessedElements.Where(x => x is IAiFriend).Select(x => x as IAiFriend);
        var friendStimuli = stimuli.Where(x => x.Type == StimulusType.Transmission);

        foreach (var candidat in candidats)
        {
            var data = candidat.Data;
            if (data == null)
                continue;

            Debug.Log($"{candidat} : {data}");
            

            var elementPosition = candidat.Position;
            if (Vector3.Distance(elementPosition, position) > CommunicationRadius)
                continue;

            stimuli.RemoveAll(x => x.GetData<TransmissionData>().Emitter == data.Emitter);

            stimuli.Add(new Stimulus
            {
                Data = data,
                Position = elementPosition,
                TimeLeft = data.TransmittedStimulus.TimeLeft,
                Type = StimulusType.Transmission
            });

            activeStimuli++;
        }

        return activeStimuli;
    }

    public override void OnGizmos(Transform transform)
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(transform.position, CommunicationRadius);
    }
}
