using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffscreenUnitIndicator : MonoBehaviour {

    private Unit[] playerUnits;
    private Camera _myCam;
    public GameObject[] _offscreenArrows;

    public float edgeOffset = 2f;

	void Awake ()
    {
        _myCam = GetComponent<Camera>();
    }

	void Update ()
    {
        GetOffScreenUnits();

    }

    void GetOffScreenUnits()
    {

        playerUnits = FindObjectsOfType<Unit>();

        for(int i = 0; i < playerUnits.Length; i++)
        {
           Vector3 screenpos = _myCam.WorldToViewportPoint(playerUnits[i].transform.position);
          
            // check if the unit is offscreen
            if(screenpos.x < 0 || screenpos.x > 1 || screenpos.y < 0 || screenpos.y > 1)
            {
                SetOffScreenArrowVisibility(i, true);
                MoveOffScreenArrow(i, screenpos);
            }
            else
            {
                SetOffScreenArrowVisibility(i, false);
            }


        }
    }

    void MoveOffScreenArrow(int index, Vector3 screenPosition)
    {
      
         // Position de la flèche sur l'écran
        var newPos = Camera.main.ViewportToScreenPoint(screenPosition);

        newPos.x = Mathf.Clamp(newPos.x, edgeOffset, Screen.width - edgeOffset);
        newPos.y = Mathf.Clamp(newPos.y, edgeOffset, Screen.height - edgeOffset);

        _offscreenArrows[index].transform.position = newPos;

        /*
        // Rotation de la flèche en direction de la cible
        var newRot = Vector2.Angle(Vector2.zero, new Vector2(newPos.x, newPos.y));
        _offscreenArrows[index].transform.eulerAngles = new Vector3(0, 0, Vector2.Angle(new Vector2(0.5f,0.5f), new Vector2(screenPosition.x + 0.5f, screenPosition.y + 0.5f)));
        Debug.Log(new Vector2(screenPosition.x, screenPosition.y));

    */
    }

    void SetOffScreenArrowVisibility(int index, bool visibility)
    {
       if(_offscreenArrows[index].activeSelf != visibility)
        {
            _offscreenArrows[index].SetActive(visibility);
        }
    }
}
