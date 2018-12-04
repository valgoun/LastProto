using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour {

    public static CursorManager cursorManagerInst;
    private SelectionManager _selection;

    public Texture2D[] cursorTextures;

    public GameObject unitDestinationClickFX;

    private bool _isCursorDefault = true;

	// Use this for initialization
	void Awake ()
    {
        cursorManagerInst = this;

        //   Cursor.visible = false;

        ChangeCursor(0);
    }

    void Start ()
    {
        _selection = SelectionManager.Instance;
    }
	
	// Update is called once per frame
	void Update ()
    {
     // Fx de destination d'unité
        if(Input.GetMouseButtonDown(1) && _selection.SelectedElements.Count > 0)
        {

            Ray rayDest = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitDest;
            if (Physics.Raycast(rayDest, out hitDest, 1000, LayerMask.GetMask("Ground")))
            {
                SpawnFX(unitDestinationClickFX, hitDest.point + new Vector3(0,0.1f,0), 3f);
            }
               
        }

        // Targeting de spell
        if (!_isCursorDefault)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))
            {
                Cursor.SetCursor(cursorTextures[0], Vector2.zero, CursorMode.Auto);
            }
        }

        // Highlight quand le curseur est sur un élement cliquable

        Ray rayHL = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitHL;
        if (Physics.Raycast(rayHL, out hitHL, 1000, LayerMask.GetMask("Selection")) && _isCursorDefault)
        {
            Cursor.SetCursor(cursorTextures[5], Vector2.zero, CursorMode.Auto);
        }
        else if(_isCursorDefault)
        {
            Cursor.SetCursor(cursorTextures[0], Vector2.zero, CursorMode.Auto);
        }
    }


    public void ChangeCursor(int index)
    {        
      Cursor.SetCursor(cursorTextures[index], Vector2.zero, CursorMode.Auto);

        if(index != 0)
         _isCursorDefault = false;
    }


    public void SpawnFX(GameObject fx_go , Vector3 pos, float lifetime)
    {    
        var fxInst = Instantiate(fx_go);
        fxInst.transform.position = pos;
        Destroy(fxInst, lifetime);
    }
}
