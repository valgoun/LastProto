using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationManager : MonoBehaviour
{

    public static FormationManager Instance => _instance;

    private static FormationManager _instance;

    [SerializeField]
    private List<Transform> _formations;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this);
    }


    public Vector3 GetPositionInFormation(Vector3 destination, int formationIndex, int formationSize, Vector3 formationDirection)
    {
        if (formationIndex == 0)
            return destination;
        var formation = _formations[formationSize - 2];
        formation.forward = formationDirection;
        return destination + formation.GetChild(formationIndex - 1).position;
    }

    //For debug purpose only
    //private void OnDrawGizmos()
    //{
    //    Vector3 destination = new Vector3(6, 1, 8);
    //    Vector3 direction = Quaternion.AngleAxis(30f, Vector3.up) *  Vector3.right;

    //    Gizmos.color = Color.green;
    //    Gizmos.DrawSphere(destination, 0.35f);
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawLine(destination + direction * 0.35f, destination + direction);

    //    int size = 4;
    //    for (int i = 0; i < size - 1; i++)
    //    {
    //        Vector3 p = GetPositionInFormation(destination, i+1, size, direction);
    //        Gizmos.DrawSphere(p, 0.35f);
    //    }
    //}
}
