using UnityEngine;
using System;
using CaptainCoder.Core;

[CreateAssetMenu(fileName = "PedestalPressurePlateEvent", menuName = "BodyMind/Events/Common/Pedestal Pressure Plate")]
public class PedestalPressurePlate : MapEvent
{
    public MutablePosition PedestalPosition;
    private PlayerData Player => GameManager.Instance.Player;
    private bool _isHeld = false;
    public AudioClip ClickSound;

    public override bool OnEnter()
    {
        if (_isHeld)
        {
            MessageController.Display("A dense Moon ball sits on this pressure plate.");
        }
        else
        {
            MessageController.Display("Click!");
            SoundEffectController.PlaySFX(ClickSound);
            PedestalController.Pedestals[PedestalPosition.Freeze()].IsFiring = true;
            GameManager.Instance.AbilityController.OnAbilityFinished += CheckForMoonBall;
        }
        return false;
    }

    public override bool OnExit()
    {
        if (!_isHeld)
        {
            MessageController.Display("Click!");
            SoundEffectController.PlaySFX(ClickSound);
            PedestalController.Pedestals[PedestalPosition.Freeze()].IsFiring = false;
            PedestalController.Pedestals[PedestalPosition.Freeze()].DestroyBalls();
        }
        GameManager.Instance.AbilityController.OnAbilityFinished -= CheckForMoonBall;
        return false;
    }

    private void CheckForMoonBall(AbilityDefinition ability)
    {
        if (ability is MoonBallAbility)
        {
            MessageController.Display($"The ball holds the pressure plate in place.");
            GameManager.Instance.AbilityController.OnAbilityFinished += RemoveMoonBall;
            _isHeld = true;
        }
    }

    private void RemoveMoonBall(AbilityDefinition ability)
    {
        if (ability is MoonBallAbility)
        {
            _isHeld = false;
            GameManager.Instance.AbilityController.OnAbilityFinished -= RemoveMoonBall;
            PedestalController.Pedestals[PedestalPosition.Freeze()].IsFiring = false;
            PedestalController.Pedestals[PedestalPosition.Freeze()].DestroyBalls();
        }
    }

    private void OnEnable()
    {
        _isHeld = false;
    }

}