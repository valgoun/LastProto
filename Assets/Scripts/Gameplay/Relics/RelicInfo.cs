using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Relic", menuName = "Relic")]
public class RelicInfo : ScriptableObject {

    public string Head;
    public string Subhead;
    [TextArea(3, 30)]
    public string Description;

    [Space]
    public GameObject RenderObject;
    public Vector3 InitialRotation;
    public Vector3 Scale = Vector3.one;
    public Vector3 Offset;
}
