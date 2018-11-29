using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionRect : MonoBehaviour {

    private Image _image;
    private SelectionManager _selection;
    private RectTransform _transform;

	// Use this for initialization
	void Start () {
        _image = GetComponent<Image>();
        _selection = SelectionManager.Instance;
        _transform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		if (_selection.IsSelecting)
        {
            _image.enabled = true;

            Vector3 start = Camera.main.WorldToScreenPoint(_selection.Origin);
            start.z = 0;
            Vector3 end = Camera.main.WorldToScreenPoint(_selection.Destination);
            end.z = 0;

            Vector3 dir = end - start;

            _transform.position = start + (dir / 2);

            start = ConvertToCorrectScreenSpace(start);
            end = ConvertToCorrectScreenSpace(end);
            dir = end - start;

            _transform.sizeDelta = new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
        }
        else
        {
            _image.enabled = false;
        }
	}

    Vector3 ConvertToCorrectScreenSpace(Vector3 position)
    {
        position.x = (position.x / Screen.width) * 1920;
        position.y = (position.y / Screen.height) * 1080;
        return position;
    }
}
