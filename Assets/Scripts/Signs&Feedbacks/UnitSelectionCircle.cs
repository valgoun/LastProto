using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Unit))]
public class UnitSelectionCircle : MonoBehaviour {

    private Unit _unitScript;
    public SpriteRenderer selectionCircleSprite;
    public SpriteRenderer destinationSprite;
    private NavMeshAgent _navAgent;

    private void Awake()
    {
        _unitScript = GetComponent<Unit>();
        _navAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update ()
    {
		if(_unitScript.Selected && !selectionCircleSprite.enabled)
            selectionCircleSprite.enabled = true;

        if (!_unitScript.Selected && selectionCircleSprite.enabled)
            selectionCircleSprite.enabled = false;


        /*
        if(Vector3.Distance(transform.position, _navAgent.destination) > 1)
        {
            destinationSprite.enabled = true;
            destinationSprite.transform.position = _navAgent.destination;
        }
        else if(Vector3.Distance(transform.position, _navAgent.destination) <= 1)
        {
            destinationSprite.enabled = false;
        }
        */
    }
}
