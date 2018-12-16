using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellDescriptionWindow : MonoBehaviour {

    public Text SpellTitle;
    public Text SpellCooldown;
    public Text SpellDescription;

    private RectTransform _myRect;
    private Spell _currentSpell;

    public static SpellDescriptionWindow Instance;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
        gameObject.SetActive(false);
        _myRect = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        _myRect.position = Input.mousePosition;
	}

    public void ActivateMe (Spell spell)
    {
        _currentSpell = spell;
        gameObject.SetActive(true);
        SpellTitle.text = spell.SpellName;
        SpellCooldown.text = "(" + spell.CooldownDuration + "s)";
        SpellDescription.text = spell.SpellDescription;
    }

    public void DeactivateMe (Spell spell)
    {
        if (_currentSpell == spell)
            gameObject.SetActive(false);
    }
}
