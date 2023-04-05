using System;
using UnityEngine;

[Serializable]
public class PlayerStat : ISerializationCallbackReceiver
{
    [SerializeField]
    private int _value;

    public PlayerStat(DualStat stat, int max) => (Stat, Max) = (stat, max);

    public event Action<PlayerStat> OnChange;
    public event Action<Stat> OnStatIsZero;

    public int Value
    {
        get => _value;
        set
        {
            _value = Mathf.Clamp(value, -Max, Max);
            if (_value == -Max)
            {
                OnStatIsZero?.Invoke(Parts.Left);
            }
            if (_value == Max)
            {
                OnStatIsZero?.Invoke(Parts.Right);
            }
            if (!isSerializing)
            {
                OnChange?.Invoke(this);
            }
        }
    }

    public float LeftPercentage => .5f + ((float)Value / (float)Max) * .5f;

    [field: SerializeField]
    public int Max { get; set; }
    [field: SerializeField]
    public DualStat Stat { get; private set; }
    public (Stat Left, Stat Right) Parts => Stat.Parts();

    public int PartValue(Stat toCheck)
    {
        if (toCheck == Parts.Left)
        {
            return Max + Value;
        }
        else if (toCheck == Parts.Right)
        {
            return Max - Value;
        }
        throw new System.ArgumentException($"Cannot get value of {toCheck} on dual stat {Stat}");
    }

    private bool isSerializing;
    public void OnAfterDeserialize()
    {
        isSerializing = true;
        Value = _value;
        _left = PartValue(Parts.Left);
        _right = PartValue(Parts.Right);
        isSerializing = false;
    }

    public void OnBeforeSerialize()
    {
        // Ignore
    }

    internal void NotifyObservers() => OnChange?.Invoke(this);

    #region Inspector Helpers
    [SerializeField]
    private int _right;
    [SerializeField]
    private int _left;
    #endregion

}
