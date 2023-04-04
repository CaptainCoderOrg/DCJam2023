using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CaptainCoder.Core;

[CreateAssetMenu(fileName = "Balance", menuName = "BodyMind/Ability/Balance")]
public class BalanceAbility : AbilityDefinition
{
    private static readonly WaitForSeconds s_Delay = new (0.1f);
    private Coroutine _coroutine;
    private bool _isRegistered = false;

    
    public override void OnUse(PlayerData player)
    {
        _coroutine = GameManager.Instance.StartCoroutine(Balance(player));
        RegisterInterrupt();
    }

    public void RegisterInterrupt()
    {
        PlayerMovementController.Instance.OnPositionChange += CancelOnPositionChange;
        PlayerMovementController.Instance.OnDirectionChange += CancelOnDirectionChange;
    }

    private void CancelOnPositionChange(Position p) => InterruptBalance();
    private void CancelOnDirectionChange(Direction f) => InterruptBalance();

    public void InterruptBalance()
    {
        if (_coroutine != null)
        {
            MessageController.Display("You lose concentration...");
            GameManager.Instance.StopCoroutine(_coroutine);
            _coroutine = null;
        }
        PlayerMovementController.Instance.OnPositionChange -= CancelOnPositionChange;
        PlayerMovementController.Instance.OnDirectionChange -= CancelOnDirectionChange;
    }

    private IEnumerator Balance(PlayerData player)
    {
        if (IsBalanced) 
        { 
            MessageController.Display("You feel balanced.");
            yield break;
        }
        MessageController.Display("You take a deep breath and begin to center yourself.");
        while (true)
        {
            yield return s_Delay;
            bool stop = true;
            foreach(PlayerStat stat in player.Stats.Stats)
            {
                if (stat.Value != 0)
                {
                    stop = false;
                    stat.Value -= (int)Mathf.Sign(stat.Value);
                }
            }
            if (stop) { break; }
        }
        MessageController.Display("You feel balanced.");
        PlayerMovementController.Instance.OnPositionChange -= CancelOnPositionChange;
        PlayerMovementController.Instance.OnDirectionChange -= CancelOnDirectionChange;
    }

    public void OnEnable()
    {
        _isRegistered = false;
    }

    private bool IsBalanced
    {
        get
        {
            foreach(PlayerStat stat in GameManager.Instance.Player.Stats.Stats)
            {
                if (stat.Value != 0) { return false; }
            }
            return true;
        }
    }
    
}
