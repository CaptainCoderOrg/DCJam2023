using UnityEngine;
using System;
using System.Collections.Generic;
using CaptainCoder.Core;
using CaptainCoder.Audio;

[CreateAssetMenu(fileName = "CombatStartEvent", menuName = "BodyMind/Events/Common/Combat Start Event")]
public class CombatStartEvent : MapEvent
{
    public List<string> EnemyNames;
    public AudioClip SecondTrack;
    // public bool _hasStarted = false;
    // public bool _isSuccessful = false;
    public void OnEnable()
    {
        // _hasStarted = false;
    }

    public override bool OnEnter()
    {
        foreach (string enemyName in EnemyNames)
        {
            GameObject enemy = GameObject.Find(enemyName);
            EnemyController controller = enemy.GetComponent<EnemyController>();
            controller.StartCombat();
            if (SecondTrack != null) { MusicController.Instance.StartSecondTrack(SecondTrack); }
        }
        return false;   
    }



}