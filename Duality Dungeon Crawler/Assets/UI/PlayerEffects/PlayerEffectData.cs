using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerEffect", menuName = "BodyMind/Player Effect")]
public class PlayerEffectData : ScriptableObject
{
    public PlayerEffect Effect;
    public Sprite Sprite;
    public string Description;
}
