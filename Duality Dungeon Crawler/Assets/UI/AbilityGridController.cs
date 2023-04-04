using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class AbilityGridController : MonoBehaviour
{
    public static bool IsDragging { get; private set; }
    public static event Action OnDragEnd;
    public static string RunePhrase { get; set; } = string.Empty;

    private VisualElement _root;


    public void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        VisualElement gridContainer = _root.Q<VisualElement>("GridContainer");
        _root.RegisterCallback<PointerDownEvent>(HandleClick);
        _root.RegisterCallback<PointerUpEvent>(HandleRelease);
        _root.RegisterCallback<PointerLeaveEvent>(HandleLeave);
    }

    private void HandleClick(PointerDownEvent evt)
    {
        IsDragging = true;
    }

    private void HandleLeave(PointerLeaveEvent evt) => Release();

    private void HandleRelease(PointerUpEvent evt) => Release();

    private void Release()
    {
        if (RunePhrase != string.Empty) { GenerateAbility(); }
        RunePhrase = string.Empty;
        IsDragging = false;
        OnDragEnd.Invoke();
    }

    private void GenerateAbility()
    {
        
        if (AbilityManifest.TryLookup(RunePhrase, out AbilityDefinition definition))
        {
            definition.OnUse(GameManager.Instance.Player);   
        }
        else
        {
            
        }
    }

}