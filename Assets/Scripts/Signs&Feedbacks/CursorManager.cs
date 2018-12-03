using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour {

    public static CursorManager cursorManagerInst;
    private SelectionManager _selection;

    public Texture2D[] cursorTextures;

    public GameObject unitDestinationClickFX;

   public enum CursorType
    {
        Default,
        Unit_Over,
        Spell_Rez,
        Spell_Wall,
        Spell_Explosion
    } 

	// Use this for initialization
	void Awake ()
    {
        cursorManagerInst = this;
      
     //   Cursor.visible = false;

     //   ChangeCursor(CursorType.Default);
    }

    void Start ()
    {
        _selection = SelectionManager.Instance;
    }
	
	// Update is called once per frame
	void Update ()
    {
      //  cursorImage.transform.position = Input.mousePosition + cursorOffset;

        if(Input.GetMouseButtonDown(1) && _selection.SelectedElements.Count > 0)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Ground")))
            {
                SpawnFX(unitDestinationClickFX, hit.point + new Vector3(0,0.1f,0), 3f);
            }
               
        }
    }


    public void ChangeCursor(CursorType type)
    {
        switch(type)
        {

            case CursorType.Default:
                Cursor.SetCursor(cursorTextures[0], Vector2.zero, CursorMode.Auto);
                break;

            case CursorType.Unit_Over:
                Cursor.SetCursor(cursorTextures[1], Vector2.zero, CursorMode.Auto);
                break;

            case CursorType.Spell_Rez:
                Cursor.SetCursor(cursorTextures[2], Vector2.zero, CursorMode.Auto);
                break;

            case CursorType.Spell_Wall:
                Cursor.SetCursor(cursorTextures[3], Vector2.zero, CursorMode.Auto);
                break;

            case CursorType.Spell_Explosion:
                Cursor.SetCursor(cursorTextures[4], Vector2.zero, CursorMode.Auto);
                break;

            default:
                Cursor.SetCursor(cursorTextures[0], Vector2.zero, CursorMode.Auto);
                break;
        }  
    }

    public void CursorGlow(bool b)
    {
        Debug.Log("CursorGlox");
    }

    public void SpawnFX(GameObject fx_go , Vector3 pos, float lifetime)
    {    
        var fxInst = Instantiate(fx_go);
        fxInst.transform.position = pos;
        Destroy(fxInst, lifetime);
    }
}
