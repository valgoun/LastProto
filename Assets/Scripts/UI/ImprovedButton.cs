using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImprovedButton : Button
{
    [HideInInspector]
    public Action Pressed = () => { };
    [HideInInspector]
    public Action UnPressed = () => { };

    private bool lastPressedState = false;

    // Update is called once per frame
    void Update()
    {
        if (IsPressed() != lastPressedState)
        {
            lastPressedState = IsPressed();

            if (lastPressedState)
                Pressed();
            else
                UnPressed();
        }
    }
}
