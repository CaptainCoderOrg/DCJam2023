using System.Collections;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

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
        DialogChain.Dialog(@"The Necromancer opens his eyes. ""Ah, you have done well. 
        I sense you have found harmony in the runes. A task that I was never able to accomplish.""".TrimMultiLine())
        .AndThen(@"""I now recall how this quarrel began. It was over their love for me. Now, the only way for this to end is for one
        of the three of us to perish.""".TrimMultiLine()).OnFinish(Choice).Display();        
        
    }

    public void Choice()
    {
        
        DialogController.Instance.DisplayDialog("Are you prepared to free the Necromancer from this mortal coil?");
        DialogController.Instance.SetOptions(
            ("Leave", () => { DialogController.Instance.IsVisible = false; }),
            ("Kill the Necromancer", Final)
        );
        DialogController.Instance.IsVisible = true;
    }

    public void Final()
    {
        DialogChain.Dialog(@"You swallow hard and muster all of your courage. You focus all of your energies into the Necromancer.
        ".TrimMultiLine())
        .AndThen("The Necromancer smiles as his body vanishes.")
        .OnFinish(Credits)
        .Display();

    }

    public static void Credits()
    {
        SceneManager.LoadScene("Credits");
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
