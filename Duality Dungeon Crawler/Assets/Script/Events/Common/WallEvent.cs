using CaptainCoder.Core;
using UnityEngine;

[CreateAssetMenu(fileName = "WallEvent", menuName = "BodyMind/Events/Wall Event")]
public class WallEvent : MapEvent
{
    [field: SerializeField]
    public Direction Facing { get; private set; }
    [field: SerializeField]
    public string Message { get; private set; }

    public override bool OnEnter()
    {
        CheckDirection();
        PlayerMovementController.Instance.OnDirectionChange += CheckDirection;
        return false;
    }

    public override bool OnExit()
    {
        PlayerMovementController.Instance.OnDirectionChange -= CheckDirection;
        return false;
    }

    private void CheckDirection(Direction _) => CheckDirection();

    public void CheckDirection()
    {
        if (PlayerMovementController.Instance.Facing == Facing)
        {
            MessageController.Display(Message);
        }
    }
}