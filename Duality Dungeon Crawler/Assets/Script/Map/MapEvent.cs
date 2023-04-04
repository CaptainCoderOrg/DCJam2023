using UnityEngine;
public class MapEvent : ScriptableObject
{

    /// <summary>
    /// Returns true if this interaction should stop propogation of further events.
    /// </summary>
    public virtual bool OnEnter()
    {
        return false;
    }

    /// <summary>
    /// Returns true if this interaction should stop propogation of further events.
    /// </summary>
    public virtual bool OnInteract()
    {
        return false;
    }

}