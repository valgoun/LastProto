using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class RelicManager : MonoBehaviour {

    [Header("Tweaking")]
    public float RotationSpeed;

    [Space]
    public LayerMask ModelLayer;

    [Header("References")]
    public Camera RelicCamera;
    public Transform RenderHolder;
    public GameObject RelicUI;

    [Space]
    public Text HeadField;
    public Text SubheadField;
    public Text DescriptionField;

    [Header("Debug")]
    [ReadOnly]
    public bool IsReadingRelic;
    [ReadOnly]
    public bool IsRotatingObject;

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
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pos = RelicCamera.ScreenToWorldPoint(Input.mousePosition * 2 + Vector3.forward * RelicCamera.nearClipPlane);
                if (Physics.Raycast(pos, RelicCamera.ScreenToWorldPoint(Input.mousePosition * 2 + Vector3.forward * RelicCamera.farClipPlane) - pos, ModelLayer))
                {
                    IsRotatingObject = true;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                IsRotatingObject = false;
            }
            else if (IsRotatingObject)
            {
                Quaternion qx = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationSpeed * Time.fixedDeltaTime, RelicCamera.transform.forward);
                Quaternion qy = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * RotationSpeed * Time.fixedDeltaTime, RelicCamera.transform.right);

                _relicObject.transform.Rotate(RelicCamera.transform.up, Input.GetAxis("Mouse X") * -RotationSpeed * Time.fixedDeltaTime, Space.World);
                _relicObject.transform.Rotate(RelicCamera.transform.right, Input.GetAxis("Mouse Y") * RotationSpeed * Time.fixedDeltaTime, Space.World);
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
