using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour {

    public static CursorManager cursorManagerInst;

    public Vector3 cursorOffset;

 //   public GameObject movementCommandFeedback;

    public Canvas cursorCanvas;

    public Image cursorImage;

    public Sprite[] cursorSprites;

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
      
        Cursor.visible = false;

        ChangeCursor(CursorType.Default);
    }
	
	// Update is called once per frame
	void Update ()
    {
        cursorImage.transform.position = Input.mousePosition + cursorOffset;

    }


    public void ChangeCursor(CursorType type)
    {
        switch(type)
        {

            case CursorType.Default:
                cursorImage.sprite = cursorSprites[0];
                break;

            case CursorType.Unit_Over:
                cursorImage.sprite = cursorSprites[1];
                break;

            case CursorType.Spell_Rez:
                Debug.Log("yolo");
                break;

            case CursorType.Spell_Wall:
                Debug.Log("yolo");
                break;

            case CursorType.Spell_Explosion:
                Debug.Log("yolo");
                break;

            default:
                cursorImage.sprite = cursorSprites[0];
                break;
        }  
    }

    public void CursorGlow(bool b)
    {
        if (b)
            cursorImage.color = Color.green;
        else
            cursorImage.color = Color.white;
    }
}
