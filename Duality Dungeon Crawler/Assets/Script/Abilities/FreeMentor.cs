using System.Collections;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "FreeMentor", menuName = "BodyMind/Ability/FreeMentor")]
public class FullHarmony : AbilityDefinition
{

    public override IEnumerator OnUse(PlayerData player, Action OnFinish)
    {
        
        Teleport();
        OnFinish();
        yield break;

    }

    public void Teleport()
    {

    }

    public override bool CanCast(PlayerData player, out string message)
    {
        if (PlayerMovementController.Instance.CurrentMap == GameManager.Instance.EntranceMap &&
            PlayerMovementController.Instance.Position == (2, 2))
            {
                message= string.Empty;
                return true;
            }
        
            
        message = "Nothing happens... perhaps you should speak with the Necromancer.";
        return false;
    }
}
