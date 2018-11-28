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
        int activeStimulus = 0;
        foreach (var candidat in brain.ProcessedElements)
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
                        activeStimulus++;
                        var stimulus = stimuli.Find(x => x.Origin == candidat.GameObject);
                        if (stimulus == null)
                        {
                            stimulus = new Stimulus { Origin = candidat.GameObject, Type = StimulusType.SightEnemy };
                            stimuli.Add(stimulus);
                        }
                        stimulus.Position = candidat.Position;
                        stimulus.TimeLeft = element.StimuliLifetime;
                    }
                }

            }
        }

        return activeStimulus;
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
