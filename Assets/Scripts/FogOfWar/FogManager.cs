using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogManager : MonoBehaviour {

    public static FogManager Instance => _instance;
    public IReadOnlyList<IVisionElement> VisionElements => _elements;

    private List<IVisionElement> _elements = new List<IVisionElement>();
    private static FogManager _instance;

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    public void RegisterElement(IVisionElement element)
    {
        _elements.Add(element);
    }
    
    public void DeleteElement(IVisionElement element)
    {
        _elements.Remove(element);
    }
}

public interface IVisionElement
{
    Vector3 Position { get; }
    float VisionRange { get; }
}
