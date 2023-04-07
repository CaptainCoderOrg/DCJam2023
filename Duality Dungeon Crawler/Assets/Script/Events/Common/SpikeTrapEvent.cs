using UnityEngine;
public class SpikeTrapEvent : MapEvent
{

    /// <summary>
    /// Returns true if this interaction should stop propagation of further events.
    /// </summary>
    public override bool OnEnter()
    {
        return false;
    }
    
    /// <summary>
    /// Returns true if this interaction should stop propagation of further events.
    /// </summary>
    public override bool OnExit()
    {
        return false;
    }

}