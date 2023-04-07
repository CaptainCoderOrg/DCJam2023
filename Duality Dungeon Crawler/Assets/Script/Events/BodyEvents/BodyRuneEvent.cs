using UnityEngine;
using System;

[CreateAssetMenu(fileName = "BodyRuneEvent", menuName = "BodyMind/Events/Body/Body Rune")]
public class BodyRuneEvent : MapEvent
{
    private PlayerData Player => GameManager.Instance.Player;
    private RuneData BodyRune => GameManager.Instance.Runes.Body;
    private DialogController Dialog => DialogController.Instance;

    public override bool OnInteract()
    {
        if (!Player.Runes.HasRune(BodyRune))
        {
            Dialog.DisplayDialog(@"
            This majestic statue stands tall, with its arms extended in a sign of welcome. 
            Its face is set in a gentle expression of peace and serenity. 
            Above its hands, a mysterious rune floats, spinning gracefully in the air.".TrimMultiLine());
            Dialog.SetOptions(
                ("Take Rune", TakeRune),
                ("Leave", Leave.ThenCloseDialog())
            );
            Dialog.IsVisible = true;
        }
        else
        {
           Dialog.DisplayDialog(@"
            This majestic statue stands tall, with its arms extended in a sign of welcome. 
            Its face is set in a gentle expression of peace and serenity.".TrimMultiLine());
            Dialog.SetOptions(
                ("Leave", Leave.ThenCloseDialog())
            );
            Dialog.IsVisible = true; 
        }
        return false;
    }

    private Action TakeRune => () => {
        Dialog.DisplayDialog(@"
        You reach out for the rune, and as your fingers touch it, it begins to spin faster and vibrate. 
        A bright, blinding light envelopes you. 
        You feel a surge of white energy coursing through your body. 
        After a moment, your vision returns and the rune is gone.
        ".TrimMultiLine());
        Dialog.SetOptions(("Continue", GiveRune.ThenCloseDialog()));
    };

    private Action GiveRune => () =>
    {
        MessageController.Display("You now know the power of the Body rune.");
        Player.Runes.AddRune(BodyRune);
    };

    private Action Leave => () => {
    };

}