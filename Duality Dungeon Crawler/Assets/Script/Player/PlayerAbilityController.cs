using UnityEngine;
using CaptainCoder.Core;
using System;

public class PlayerAbilityController : MonoBehaviour
{
    public event Action<AbilityDefinition> OnAbilityFinished;
    public bool IsCasting { get; private set; }

    private Coroutine _coroutine;
    private AudioSource _currentClip;

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
        PlayAudio(ability);
        _coroutine = StartCoroutine(ability.OnUse(GameManager.Instance.Player, () => FinishCasting(ability)));
        RegisterInterrupt();
    }

    private void PlayAudio(AbilityDefinition ability)
    {
        if (ability.CastSound == null) { Debug.LogWarning($"No cast sound on {ability.Name}");}
        _currentClip = ability.CastSound?.Play();
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
            _currentClip?.Stop();
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