using UnityEngine;

[RequireComponent(typeof(Light))]
public class TorchLight : MonoBehaviour
{

    private Light _light;

    [field: SerializeField]
    public float MinRange { get; private set; } = 10f;
    [field: SerializeField]
    public float MaxRange { get; private set; } = 14f;
    [field: SerializeField]
    public float MinIntensity { get; private set; } = 3f;
    [field: SerializeField]
    public float MaxIntensity { get; private set; } = 5f;
    [field: SerializeField]
    public float Scale { get; private set; } = 5f;
    [field: SerializeField]
    public AnimationCurve Curve { get; private set; }

    public void Awake()
    {
        _light = GetComponent<Light>();   
    }

    public void Update()
    {
        _light.range = MinRange + ((MaxRange - MinRange) * Curve.Evaluate(Mathf.Abs(Mathf.Sin(Time.time * Scale))));
        _light.intensity = MinIntensity + ((MinIntensity - MaxIntensity) * Curve.Evaluate(Mathf.Abs(Mathf.Sin(Time.time * Scale))));
    }

}