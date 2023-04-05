using UnityEngine;
using System;

[CreateAssetMenu(fileName = "YangRuneEvent", menuName = "BodyMind/Events/Yang/Yang Rune")]
public class YangRuneEvent : MapEvent
{
    private PlayerData Player => GameManager.Instance.Player;
    private RuneData YangRune => GameManager.Instance.Runes.Yang;
    private DialogController Dialog => DialogController.Instance;

    public override bool OnInteract()
    {
        if (!Player.Runes.HasRune(YangRune))
        {
            Dialog.DisplayDialog(@"            
            The dark statue stands in a silent chamber, its arms extended in a gesture of supplication. 
            Its surface is rough and uneven, a stark contrast to the smooth stone of the chamber walls. 
            A single rune hovers in the air above the statue's hands, spinning slowly and casting an eerie glow across the room. 
            ".TrimMultiLine());
            Dialog.SetOptions(
                ("Take Rune", TakeRune),
                ("Leave", Leave.ThenCloseDialog())
            );
            Dialog.IsVisible = true;
        }
        else
        {
           Dialog.DisplayDialog(@"
            The dark statue stands in a silent chamber, its arms extended in a gesture of supplication. 
            Its surface is rough and uneven, a stark contrast to the smooth stone of the chamber walls. ".TrimMultiLine());
            Dialog.SetOptions(
                ("Leave", Leave.ThenCloseDialog())
            );
            Dialog.IsVisible = true; 
        }
        return false;
    }

    private Action TakeRune => () => {
        Dialog.DisplayDialog(@"
        You reach out to touch the rune, and as soon as your fingers brush against it, 
        it starts to spin faster, vibrating with a strange energy. The darkness of the 
        room quickly envelopes you, and when your vision returns, the rune is gone. 
        You feel a strange and unfamiliar power within your body, a dark energy 
        that you can feel in your bones. Something deep within you is stirring, 
        and you can feel it slowly calling you towards something unknown.
        ".TrimMultiLine());
        Dialog.SetOptions(("Continue", GiveRune.ThenCloseDialog()));
    };

    private Action GiveRune => () =>
    {
        MessageController.Display("You now know the power of the Yang rune.");
        Player.Runes.AddRune(YangRune);
    };

    private Action Leave => () => {
    };

}