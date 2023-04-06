using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MiniMapController : MonoBehaviour
{
    private VisualElement _overlay;
    // Start is called before the first frame update
    void Awake()
    {
        _overlay = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("MiniMapOverlay");
        Debug.Assert(_overlay != null, "Could not find MiniMap overlay.");
        Opacity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private float Opacity 
    {
        set
        {
            _overlay.style.opacity = value;
        }
    }
}
