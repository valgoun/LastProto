using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public uint SpellID;

    [Header("References")]
    public Text CooldownText;
    public Image CooldownImage;

    private ImprovedButton _button;
    private bool _casting;
    
    // Use this for initialization
	void Start () {
        if (SpellID < SpellManager.Instance.Spells.Length)
        {
            if (!SpellManager.Instance.Spells[SpellID])
            {
                Debug.LogError("Spell " + SpellID + " hasn't been set.");
            }
        }
        else
        {
            Debug.LogError("Spell Button on " + gameObject.name + " has an ID superior to the number of available Spells.");
            SpellID = 0;
        }

        _button = GetComponent<ImprovedButton>();
        _button.Pressed += Pressed;
        _button.UnPressed += UnPressed;
    }
	
	// Update is called once per frame
	void Update () {
		if (SpellManager.Instance.Spells[SpellID].GetAvailable())
        {
            _button.interactable = true;
            CooldownText.text = "";
            CooldownImage.fillAmount = 0;
        }
        else
        {
            _button.interactable = false;
            if (Time.time < SpellManager.Instance.Spells[SpellID].CurrentCooldown)
            {
                CooldownText.text = Mathf.Abs(Time.time - SpellManager.Instance.Spells[SpellID].CurrentCooldown).ToString("0.0");
                CooldownImage.fillAmount = Mathf.Abs(Time.time - SpellManager.Instance.Spells[SpellID].CurrentCooldown) / SpellManager.Instance.Spells[SpellID].CooldownDuration;
            }
            else
            {
                CooldownText.text = "";
                CooldownImage.fillAmount = 0;
            }
        }

        if (_button.interactable)
        {
            if (!SpellManager.Instance.SmartCast)
            {
                if (Input.GetButtonDown("Spell 0" + (SpellID + 1).ToString()))
                {
                    if (SpellManager.Instance.Spells[SpellID].HoldBehaviour)
                    {
                        ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
                        SpellDescriptionWindow.Instance.DeactivateMe(SpellManager.Instance.Spells[SpellID]);
                    }
                    else
                    {
                        ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
                    }
                }
                else if (SpellManager.Instance.Spells[SpellID].HoldBehaviour)
                {
                    if (Input.GetButtonUp("Spell 0" + (SpellID + 1).ToString()))
                    {
                        ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
                    }
                }
            }
        }
	}

    public void OnClick ()
    {
        SpellManager.Instance.CastSpell(SpellManager.Instance.Spells[SpellID]);
        _casting = false;
    }

    public void Pressed()
    {
        if (SpellManager.Instance.Spells[SpellID].HoldBehaviour)
        {
            SpellManager.Instance.CastSpell(SpellManager.Instance.Spells[SpellID]);
            SpellDescriptionWindow.Instance.DeactivateMe(SpellManager.Instance.Spells[SpellID]);
            _casting = true;
        }
    }

    public void UnPressed()
    {
        if (_casting)
        {
            SpellManager.Instance.Spells[SpellID].StopCasting();
            SpellManager.Instance.SelectedSpell = null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SpellDescriptionWindow.Instance.ActivateMe(SpellManager.Instance.Spells[SpellID]);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SpellDescriptionWindow.Instance.DeactivateMe(SpellManager.Instance.Spells[SpellID]);
    }
}
