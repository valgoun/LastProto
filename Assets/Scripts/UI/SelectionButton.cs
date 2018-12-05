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
    private BaseEventData _baseEvent;
    private int _oldIndex;
    private string _buttonID;

    // Use this for initialization
    void Start () {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
        _baseSprite = _image.sprite;
        _image.sprite = ActiveSprite;
        _baseEvent = new BaseEventData(EventSystem.current);
        SetIndex();
    }

    private void SetIndex ()
    {
        _oldIndex = Index;
        if (Index == -1)
            _buttonID = "Selection All";
        else if (Index == 1)
            _buttonID = "Selection Shaman";
        else if (Index >= 1)
            _buttonID = "Selection Aztec 0" + (Index - 1).ToString();
    }
	
	// Update is called once per frame
	void Update () {
        if (_oldIndex != Index)
        {
            SetIndex();
        }

        if (Index > 1)
        {
            if (!_active && SelectionManager.Instance.Aztecs[Index - 2])
            {
                _active = true;
                _button.interactable = true;
                _image.sprite = ActiveSprite;
            }
            else if (_active && !SelectionManager.Instance.Aztecs[Index - 2])
            {
                _active = false;
                _button.interactable = false;
                _image.sprite = _baseSprite;
            }
        }

        if (_button.interactable)
        {
            if (Input.GetButtonDown(_buttonID))
            {
                ExecuteEvents.Execute(gameObject, _baseEvent, ExecuteEvents.submitHandler);
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
