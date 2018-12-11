using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UnitPortraitHighlight : MonoBehaviour {

    public Image[] highlightImages;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


        if (SelectionManager.Instance.SelectedElements.Contains(SelectionManager.Instance.Shaman))
        {
            if (!highlightImages[0].enabled)
                highlightImages[0].enabled = true;
        }
        else
        {
            if (highlightImages[0].enabled)
                highlightImages[0].enabled = false;
        }

            for (int i = 0; i <= 2; i++)
        {
            if (SelectionManager.Instance.SelectedElements.Contains(SelectionManager.Instance.Aztecs[i]))
            {
                if (!highlightImages[i+1].enabled)
                    highlightImages[i+1].enabled = true;
            }
            else
            {
                if (highlightImages[i+1].enabled)
                    highlightImages[i+1].enabled = false;
            }
        }
        

    }
}
