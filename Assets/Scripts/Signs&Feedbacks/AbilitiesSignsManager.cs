using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesSignsManager : MonoBehaviour {

    public GameObject unitDestinationClickFX;

    private SelectionManager _selection;

    public LineRenderer[] jumpTrajectoryLineRenderer;

    void Start()
    {
        _selection = SelectionManager.Instance;
    }


    void Update ()
    {
        // Fx de destination d'unité
        if (Input.GetMouseButtonDown(1) && _selection.SelectedElements.Count > 0)
        {

            Ray rayDest = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitDest;
            if (Physics.Raycast(rayDest, out hitDest, 1000, LayerMask.GetMask("Ground")))
            {
                SpawnFX(unitDestinationClickFX, hitDest.point + new Vector3(0, 0.1f, 0), 3f);
            }

        }

        JumpAttackIndicator();

    }

    public void SpawnFX(GameObject fx_go, Vector3 pos, float lifetime)
    {
        var fxInst = Instantiate(fx_go);
        fxInst.transform.position = pos;
        Destroy(fxInst, lifetime);
    }

    public void JumpAttackIndicator()
    {

        jumpTrajectoryLineRenderer[0].SetPosition(0, Vector3.zero);
        jumpTrajectoryLineRenderer[0].SetPosition(1, Vector3.zero);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (_selection.SelectedElements.Count > 0 && Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Selection")))
        {

            foreach(Aztec ghoul in _selection.SelectedElements)
            {

                // The ghoul is in a bush

                

            if (!ghoul.IsVisible && hit.transform.tag == "Conquistador")
                {

                    if ((hit.transform.position - ghoul.transform.position).magnitude <= ghoul.JumpAttackRange)
                    {
                        jumpTrajectoryLineRenderer[0].SetPosition(0, _selection.SelectedElements[0].transform.position);
                        jumpTrajectoryLineRenderer[0].SetPosition(1, hit.transform.position);

                        // Need color correction if unavailable;
                    }
            
                }
            }
        }
    }
}
