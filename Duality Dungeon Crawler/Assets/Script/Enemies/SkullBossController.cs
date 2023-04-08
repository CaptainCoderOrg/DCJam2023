using UnityEngine;

using CaptainCoder.Core;
using System.Collections.Generic;
using System.Collections;
using CaptainCoder.Audio;

public class SkullBossController : EnemyController
{
    public SkullBossController OtherSkull;

    public override bool IsCombatOver()
    {
        Debug.Log($"Is Over: {Health <= 0 && !OtherSkull.IsAlive}");
        return Health <= 0 && !OtherSkull.IsAlive;
    }

}