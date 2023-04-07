using UnityEngine;
using System;
using CaptainCoder.Core;

[CreateAssetMenu(fileName = "PedestalEvent", menuName = "BodyMind/Events/Common/PedestalEvent")]
public class PedestalEvent : MapEvent
{

    private PedestalController Controller => PedestalController.Pedestals[PlayerMovementController.Instance.Position];


    public override bool OnEnter()
    {
        
        if (Controller.IsLit)
        {
            MessageController.Display($"This pedestal is lit!");
        }
        else
        {
            MessageController.Display($"A pedestal with a carving of a flame on it is here.");
        }
        return false;
    }

    public override bool OnInteract()
    {
        DialogController.Instance.DisplayDialog("The pedestal can be rotated.");
        DialogController.Instance.SetOptions(
            ("Rotate Pedestal", RotatePedestal.ThenCloseDialog()),
            ("Leave", Nothing.ThenCloseDialog())
        );
        DialogController.Instance.IsVisible = true;
        return false;
    }

    private Action RotatePedestal => () => 
    {
        Controller.Rotate();
    };
    private Action Nothing => () => {};

}