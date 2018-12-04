using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VisionSensor", menuName = "Sensor/Vision")]
public class VisionSensor : Sensor
{
    public float VisionAngle;
    public float VisionDistance;
    public LayerMask VisionLayer;

    public override int UpdateSensor(AIBrain brain, List<Stimulus> stimuli)
    {
        var forward = brain.transform.forward;
        var position = brain.transform.position;
        var dotLimit = Mathf.Cos(Mathf.Deg2Rad * VisionAngle * 0.5f);
        int activeStimuli = 0;

        var EnemyStimuli = stimuli.Where(x => x.Type == StimulusType.SightEnemy);
        var candidats = brain.ProcessedElements.Where(x => x is IAiFoe).Select(x => x as IAiFoe);

        foreach (var candidat in candidats)
        {
            if (!candidat.IsVisible)
                continue;

            var elementPosition = candidat.Position;
            if (Vector3.Distance(elementPosition, position) > VisionDistance)
                continue;

            var elementDirection = Vector3.Normalize(elementPosition - position);
            var dot = Vector3.Dot(forward, elementDirection);

            if (dot >= dotLimit)
            {
                RaycastHit hit;
                if (Physics.Raycast(position, elementDirection, out hit, VisionDistance, VisionLayer))
                {
                    var element = hit.collider.GetComponent<IAiVisible>();
                    element = element ?? hit.collider.GetComponentInParent<IAiVisible>();
                    element = element ?? hit.collider.GetComponentInChildren<IAiVisible>();
                    if (element != null && element == candidat)
                    {
                        activeStimuli++;
                        var stimulus = EnemyStimuli.Where(x => x.GetData<EnemyData>().EnemyGameObject == candidat.GameObject).FirstOrDefault();
                        if (stimulus == null)
                        {
                            stimulus = new Stimulus
                            {
                                Type = StimulusType.SightEnemy,
                                Data = new EnemyData { EnemyGameObject = candidat.GameObject }
                            };
                            stimuli.Add(stimulus);
                        }
                        stimulus.Position = candidat.Position;
                        stimulus.TimeLeft = element.StimuliLifetime;
                    }
                }

            }
        }

        return activeStimuli;
    }

    public override void OnGizmos(Transform transform)
    {
        Gizmos.color = Color.green;

        Vector3 pos = transform.position;
        Vector3 fw = pos + transform.forward * VisionDistance;
        Vector3 p1 = pos + Quaternion.AngleAxis(VisionAngle * 0.5f, transform.up) * transform.forward * VisionDistance;
        Vector3 p2 = pos + Quaternion.AngleAxis(-VisionAngle * 0.5f, transform.up) * transform.forward * VisionDistance;

        int tessFactor = Mathf.FloorToInt(VisionAngle) / 5;

        for (int i = 0; i <= tessFactor; i++)
        {
            Vector3 pt = pos + Quaternion.AngleAxis(VisionAngle * 0.5f * (((float)(i) / (float)(tessFactor)) * 2 - 1), transform.up) * transform.forward * VisionDistance;
            if (i == 0)
                Gizmos.DrawLine(pos, pt);
            else
            {
                Vector3 pt0 = pos + Quaternion.AngleAxis(VisionAngle * 0.5f * (((float)(i - 1) / (float)(tessFactor)) * 2 - 1), transform.up) * transform.forward * VisionDistance;
                Gizmos.DrawLine(pt0, pt);

                if (i == tessFactor)
                    Gizmos.DrawLine(pos, pt);
            }
        }
    }
}
