using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellButton : MonoBehaviour {

    public uint SpellID;

    [Header("References")]
    public Text CooldownText;

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
		if (SpellManager.Instance.Spells[SpellID].CurrentCooldown <= Time.time)
        {
            _button.interactable = true;
            CooldownText.text = "";
        }
        else
        {
            _button.interactable = false;
            CooldownText.text = Mathf.Abs(Time.time - SpellManager.Instance.Spells[SpellID].CurrentCooldown).ToString("0.0");
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
}
