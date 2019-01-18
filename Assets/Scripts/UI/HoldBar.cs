using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HoldBar : MonoBehaviour
{
    public TextMeshProUGUI MyText;
    public Image MyImage;
    
    public void SetText (string text)
    {
        MyText.text = text;
    }

    public void SetLevel (float level)
    {
        MyImage.transform.localScale = new Vector3(level, 1, 1);
    }
}
