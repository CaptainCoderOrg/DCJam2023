using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CaptainCoder.Core;

[CreateAssetMenu(fileName = "LightAbility", menuName = "BodyMind/Ability/Light")]
public class LightAbility : AbilityDefinition
{

    private static readonly WaitForSeconds s_Delay = new (0.05f);
    // private static readonly WaitForSeconds s_StartDelay = new (2f);
    private Coroutine _coroutine;
    private bool _isCasting = false;
    [field: SerializeField]
    public int SunCost { get; private set; } = 20;


    public override void OnUse(PlayerData player)
    {
        if (!CanCast)
        {
            MessageController.Display("You do not have enough Sun energy.");
            return;
        }
        if (_isCasting) { return; }
        _coroutine = GameManager.Instance.StartCoroutine(Cast(player));
        RegisterInterrupt();
    }

    public void RegisterInterrupt()
    {
        PlayerMovementController.Instance.OnPositionChange += CancelOnPositionChange;
        PlayerMovementController.Instance.OnDirectionChange += CancelOnDirectionChange;
    }

    private void CancelOnPositionChange(Position p) => InterruptCasting();
    private void CancelOnDirectionChange(Direction f) => InterruptCasting();

    public void InterruptCasting()
    {
        if (_coroutine != null)
        {
            MessageController.Display("You lose concentration...");
            GameManager.Instance.StopCoroutine(_coroutine);
            _coroutine = null;
        }
        FinishCasting();
    }

    private IEnumerator Cast(PlayerData player)
    {
        _isCasting = true;
        MessageController.Display("You begin to concentrate...");
        for (int steps = 0; steps < SunCost; steps++)
        {   
            if (player.Stats.Stat(Stat.Sun) < (SunCost - steps))
            {
                MessageController.Display("You don't have enough Sun energy.");
                FinishCasting();
                yield break;
            }         
            yield return s_Delay;
            player.Stats.Stat(DualStat.SunMoon).Value--;
        }
        MessageController.Display("A ball of light is hovering with you.");
        FinishCasting();
    }

    private void FinishCasting()
    {
        _isCasting = false;
        PlayerMovementController.Instance.OnPositionChange -= CancelOnPositionChange;
        PlayerMovementController.Instance.OnDirectionChange -= CancelOnDirectionChange;
    }
    private bool CanCast => GameManager.Instance.PlayerStats.Stat(Stat.Sun) >= SunCost;

}
