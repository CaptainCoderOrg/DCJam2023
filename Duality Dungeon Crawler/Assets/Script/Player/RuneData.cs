using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rune", menuName = "BodyMind/Rune")]
public class RuneData : ScriptableObject
{
    public Sprite sprite;
    public int RuneIndex;
    public string Description;
}
