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

    private Button _button;
    
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
            Debug.LogError("Spell Button on " + gameObject.name + " as an ID superior to the number of available Spells.");
            SpellID = 0;
        }

        _button = GetComponent<Button>();
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

        if (!SpellManager.Instance.SmartCast && _button.interactable)
        {
            if (Input.GetButtonDown("Spell 0" + (SpellID + 1).ToString()))
            {
                ExecuteEvents.Execute(gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
            }
        }
	}

    public void OnClick ()
    {
        SpellManager.Instance.CastSpell(SpellManager.Instance.Spells[SpellID]);
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
