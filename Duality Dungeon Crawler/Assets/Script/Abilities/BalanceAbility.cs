using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CaptainCoder.Core;

[CreateAssetMenu(fileName = "Balance", menuName = "BodyMind/Ability/Balance")]
public class BalanceAbility : AbilityDefinition
{
    private static readonly WaitForSeconds s_Delay = new (0.05f);
    private static readonly WaitForSeconds s_StartDelay = new (2f);
    private Coroutine _coroutine;
    private bool _isBalancing = false;

    
    public override void OnUse(PlayerData player)
    {
        if (_isBalancing) { return; }
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
        FinishBalance();
    }

    private IEnumerator Balance(PlayerData player)
    {
        if (IsBalanced) 
        { 
            MessageController.Display("You feel balanced.");
            yield break;
        }
        _isBalancing = true;
        MessageController.Display("You take a deep breath and focus inward.");
        yield return s_StartDelay;
        MessageController.Display("You energies begin to balance...");
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
        FinishBalance();
    }

    private void FinishBalance()
    {
        _isBalancing = false;
        PlayerMovementController.Instance.OnPositionChange -= CancelOnPositionChange;
        PlayerMovementController.Instance.OnDirectionChange -= CancelOnDirectionChange;
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
