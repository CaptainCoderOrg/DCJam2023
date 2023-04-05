using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class StatMeterController : MonoBehaviour
{
    private VisualElement _root;
    private void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        Debug.Assert(_root != null, "Could not find root element");
        Debug.Log($"GameManager: {GameManager.Instance}");
        RegisterMeter("BodyMindMeter", GameManager.Instance.PlayerStats.Stat(DualStat.BodyMind));
        RegisterMeter("SunMoonMeter", GameManager.Instance.PlayerStats.Stat(DualStat.SunMoon));
        RegisterMeter("YinYangMeter", GameManager.Instance.PlayerStats.Stat(DualStat.YinYang));
        RegisterMeter("RageCalmMeter", GameManager.Instance.PlayerStats.Stat(DualStat.CalmRage));
    }

    private void RegisterMeter(string label, PlayerStat toObserve)
    {
        VisualElement meter = _root.Q<VisualElement>(label);
        SingleMeterController controller = new (meter, toObserve);
        toObserve.OnChange += controller.HandleStatChange;
    }
}

public class SingleMeterController
{
    private VisualElement _root;
    private VisualElement _leftMeter;
    private VisualElement _rightMeter;
    public SingleMeterController(VisualElement root, PlayerStat toObserve)
    {
        _root = root;
        _leftMeter = _root.Q<VisualElement>("LeftElement");
        Debug.Assert(_leftMeter != null, "Could not locate left element.");
        _rightMeter = _root.Q<VisualElement>("RightElement");
        Debug.Assert(_rightMeter != null, "Could not locate right element.");        
        HandleStatChange(toObserve);
    }

    public void HandleStatChange(PlayerStat stat)
    {
        var percentage = new Length(stat.LeftPercentage * 100, LengthUnit.Percent);
        _leftMeter.style.width = new StyleLength(percentage);
    }
}