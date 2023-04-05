using UnityEngine.UIElements;
using System.Linq;
using System;

public static class ExtensionMethods
{

    public static string TrimMultiLine(this string toTrim)
    {
        return string.Join(" ", toTrim.Split("\n").Select(s => s.Trim()).Where(s => s != string.Empty));
    }

    public static Action ThenCloseDialog(this Action action)
    {
        return () =>
        {
            action.Invoke();  
            DialogController.Instance.IsVisible = false;
        };        
    }
    public static Action AndThen(this Action current, string message, string buttonText = "Continue")
    {
        return () =>
        {
            current.Invoke();
            DialogController.Instance.SetOptions((buttonText, () => DialogController.Instance.DisplayDialog(message)));
        };
    }

    

}

public class DialogChain
{
    private string _message;
    private string _continueText;
    private DialogChain _next;
    private DialogChain _first;
    private DialogChain(string message) => _message = message;
    public static DialogChain Dialog(string message)
    {
        DialogChain link = new (message);
        return link;
    }

    public DialogChain AndThen(string message, string continueText = "Continue")
    {
        if (_first == null) { _first = this; }
        _next = Dialog(message);
        _continueText = continueText;
        _next._first = _first;
        return _next;
    }

    public void Display() => _first.DisplayNext();

    private void DisplayNext()
    {
        var diag = DialogController.Instance;
        diag.DisplayDialog(_message);
        Action nextAction = _next == null ? Finish : _next.DisplayNext;
        diag.SetOptions((_continueText, nextAction));
        diag.IsVisible = true;
    }

    private static void Finish()
    {
        DialogController.Instance.IsVisible = false;
    }
}