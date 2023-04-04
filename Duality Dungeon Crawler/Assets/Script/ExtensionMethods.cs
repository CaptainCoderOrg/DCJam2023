using UnityEngine.UIElements;
using System.Linq;
using System;

public static class ExtensionMethods
{

    public static string TrimMultiLine(this string toTrim)
    {
        return string.Join(" ", toTrim.Split("\n").Select(s => s.Trim()));
    }

    public static Action ThenCloseDialog(this Action action)
    {
        return () =>
        {
            action.Invoke();  
            DialogController.Instance.IsVisible = false;
        };        
    }

}