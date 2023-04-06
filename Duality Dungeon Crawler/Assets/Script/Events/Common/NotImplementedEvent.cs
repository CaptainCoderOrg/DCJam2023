using CaptainCoder.Core;
using UnityEngine;

[CreateAssetMenu(fileName = "NotImplemented", menuName = "BodyMind/Events/Not Implemented")]
public class NotImplementedEvent : MapEvent
{

    public override bool OnEnter()
    {
        DialogChain.Dialog("This area is not yet implemented.").Display();
        return false;
    }
}