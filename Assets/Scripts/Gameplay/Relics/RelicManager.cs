using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class RelicManager : MonoBehaviour {

    [Header("References")]
    public Transform RenderHolder;
    public GameObject RelicUI;

    [Space]
    public Text HeadField;
    public Text SubheadField;
    public Text DescriptionField;

    [Header("Debug")]
    [ReadOnly]
    public bool IsReadingRelic = false;

    private GameObject _relicObject;

    public static RelicManager Instance;
    
    // Use this for initialization
	private void Awake () {
        if (!Instance)
            Instance = this;
        else
            Destroy(this);
	}

    private void Update()
    {
        if (IsReadingRelic)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseRelic();
            }
        }
    }

    public void OpenRelic (RelicInfo relic)
    {
        Time.timeScale = 0;
        IsReadingRelic = true;

        RelicUI.SetActive(true);
        _relicObject = Instantiate(relic.RenderObject, RenderHolder);
        _relicObject.transform.localRotation = Quaternion.Euler(relic.InitialRotation);
        _relicObject.transform.localPosition = relic.Offset;
        _relicObject.transform.localScale = relic.Scale;

        HeadField.text = relic.Head;
        SubheadField.text = relic.Subhead;
        DescriptionField.text = relic.Description;
    }

    public void CloseRelic ()
    {
        Time.timeScale = 1;
        IsReadingRelic = false;

        RelicUI.SetActive(false);
        Destroy(_relicObject);
    }
}
