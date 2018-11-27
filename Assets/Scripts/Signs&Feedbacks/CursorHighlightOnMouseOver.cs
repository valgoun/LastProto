using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHighlightOnMouseOver : MonoBehaviour {

    public Color cursorHighlightColor;
    public Texture2D newCursor;

   void OnMouseOver()
    {
        Cursor.SetCursor(newCursor, Vector2.zero, CursorMode.Auto);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
