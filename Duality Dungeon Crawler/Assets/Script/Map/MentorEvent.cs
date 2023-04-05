using CaptainCoder.Core;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "MentorEvent", menuName = "BodyMind/Events/Mentor Event")]
public class MentorEvent : MapEvent
{
    public override bool OnInteract()
    {
        var diag = DialogController.Instance;
        diag.DisplayDialog("Your mentors eyes open and they speak");
        (string, Action) leave = ("Leave", () => diag.IsVisible = false);
        diag.SetOptions(leave);
        diag.IsVisible = true;
        return true;
    }
}