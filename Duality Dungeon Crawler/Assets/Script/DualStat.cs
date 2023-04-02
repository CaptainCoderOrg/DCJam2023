using System;
using UnityEngine;

[Serializable]
public class PlayerStat : ISerializationCallbackReceiver
{
    [SerializeField]
    private int _value;

    public PlayerStat(DualStat stat, int max) => (Stat, Max) = (stat, max);

    public int Value
    {
        get => _value;
        set => _value = Mathf.Clamp(value, -Max, Max);
    }
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

    public void OnAfterDeserialize()
    {
        Value = _value;
        _left = PartValue(Parts.Left);
        _right = PartValue(Parts.Right);
    }

    public void OnBeforeSerialize()
    {
        // Ignore
    }

    #region Inspector Helpers
    [SerializeField]
    private int _right;
    [SerializeField]
    private int _left;
    #endregion

}
