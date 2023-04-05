using UnityEngine;
using System;

[CreateAssetMenu(fileName = "YinRuneEvent", menuName = "BodyMind/Events/Yin/Yin Rune")]
public class YinRuneEvent : MapEvent
{
    private PlayerData Player => GameManager.Instance.Player;
    private RuneData YinRune => GameManager.Instance.Runes.Yin;
    private DialogController Dialog => DialogController.Instance;

    public override bool OnInteract()
    {
        if (!Player.Runes.HasRune(YinRune))
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
        When your vision returns, the rune is gone.
        ".TrimMultiLine());
        Dialog.SetOptions(("Continue", GiveRune.ThenCloseDialog()));
    };

    private Action GiveRune => () =>
    {
        MessageController.Display("You now know the power of the Yin rune.");
        Player.Runes.AddRune(YinRune);
    };

    private Action Leave => () => {
        PlayerMovementController.Instance.Position = (14, 4);
        PlayerMovementController.Instance.Facing = Direction.East;
        MessageController.Display("You step away from the statue.");
    };

}