using UnityEngine;
using System;
using CaptainCoder.Core;

[CreateAssetMenu(fileName = "PedestalEvent", menuName = "BodyMind/Events/Common/PedestalEvent")]
public class PedestalEvent : MapEvent
{

    private PedestalController Controller => PedestalController.Pedestals[PlayerMovementController.Instance.Position];


    public override bool OnEnter()
    {
        CheckFacing();
        PlayerMovementController.Instance.OnDirectionChange += CheckFacing;
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

    public override bool OnExit()
    {
        PlayerMovementController.Instance.OnDirectionChange -= CheckFacing;
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
        CheckFacing();
        Controller.Rotate();
    };

    private void CheckFacing(Direction d) => CheckFacing();

    private void CheckFacing()
    {
        if (PlayerMovementController.Instance.Facing != Controller.Direction.RotateClockwise().RotateClockwise())
        {
            PlayerMovementController.Instance.PlayerCollider.gameObject.SetActive(false);
        }
        else
        {
            PlayerMovementController.Instance.PlayerCollider.gameObject.SetActive(true);
        }
    }
    private Action Nothing => () => {};

}