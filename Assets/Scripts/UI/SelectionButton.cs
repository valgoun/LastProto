using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectionButton : MonoBehaviour {

    public int Index;
    [Header("Assets")]
    public Sprite ActiveSprite;

    private Button _button;
    private Image _image;
    private Sprite _baseSprite;
    private bool _active = true;
    
    // Use this for initialization
	void Start () {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
        _baseSprite = _image.sprite;
        _image.sprite = ActiveSprite;
	}
	
	// Update is called once per frame
	void Update () {
		if (Index > 1)
        {
            if (!_active && SelectionManager.Instance.Aztecs[Index - 2])
            {
                _button.interactable = true;
                _image.sprite = ActiveSprite;
            }
            else if (_active && !SelectionManager.Instance.Aztecs[Index - 2])
            {
                _button.interactable = false;
                _image.sprite = _baseSprite;
            }
        }

        if (_button.interactable)
        {
            if (Index == -1)
            {
                if (Input.GetButtonDown("Selection All"))
                {
                    ExecuteEvents.Execute(gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
                }
            }
            else if (Index == 1)
            {
                if (Input.GetButtonDown("Selection Shaman"))
                {
                    ExecuteEvents.Execute(gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
                }
            }
            else if (Index >= 1)
            {
                if (Input.GetButtonDown("Selection Aztec 0" + (Index - 1).ToString()))
                {
                    ExecuteEvents.Execute(gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
                }
            }
        }
    }

    public void Clicked ()
    {
        if (Index == -1)
            SelectionManager.Instance.SelectAll();
        else if (Index == 1)
            SelectionManager.Instance.SelectShaman();
        else if (Index > 1)
            SelectionManager.Instance.SelectGhoul(Index - 2);
    }
}
