using UnityEngine;
using CaptainCoder.Core;
using System;

public class PlayerAbilityController : MonoBehaviour
{
    public event Action<AbilityDefinition> OnAbilityFinished;
    public bool IsCasting { get; private set; }

    private Coroutine _coroutine;

    public void UseAbility(AbilityDefinition ability)
    {
        if (IsCasting)
        {
            // Cancel previous cast
            InterruptCasting();
            return;
        }
        if (!ability.CanCast(GameManager.Instance.Player, out string message))
        {
            MessageController.Display(message);
            return;
        }
        IsCasting = true;
        _coroutine = StartCoroutine(ability.OnUse(GameManager.Instance.Player, () => FinishCasting(ability)));
        RegisterInterrupt();
    }

    private void CancelOnPositionChange(Position p) => InterruptCasting();
    private void CancelOnDirectionChange(Direction f) => InterruptCasting();
    
    public void RegisterInterrupt()
    {
        PlayerMovementController.Instance.OnPositionChange += CancelOnPositionChange;
        PlayerMovementController.Instance.OnDirectionChange += CancelOnDirectionChange;
    }

    public void InterruptCasting()
    {
        if (_coroutine != null)
        {
            MessageController.Display("You lose concentration...");
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
        FinishCasting(null);
    }

    private void FinishCasting(AbilityDefinition ability)
    {
        IsCasting = false;
        PlayerMovementController.Instance.OnPositionChange -= CancelOnPositionChange;
        PlayerMovementController.Instance.OnDirectionChange -= CancelOnDirectionChange;
        OnAbilityFinished?.Invoke(ability);
    }

}