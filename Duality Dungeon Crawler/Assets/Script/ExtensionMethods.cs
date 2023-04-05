using UnityEngine.UIElements;
using System.Collections.Generic;
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
    private Action _onFinish;

    private DialogChain(string message, string continueText) => (_message, _continueText) = (message, continueText);
    public static DialogChain Dialog(string message, string continueText = "Continue")
    {
        DialogChain link = new (message, continueText);
        return link;
    }

    public DialogChain AndThen(string message, string continueText = "Continue")
    {
        if (_first == null) { _first = this; }
        _next = Dialog(message, continueText);
        _next._first = _first;
        return _next;
    }

    public DialogChain OnFinish(Action finishAction) 
    {
        _first._onFinish += finishAction;
        return this;
    }

    public void Display() 
    {
        
        _first ??= this;
        _first.DisplayNext();
    }

    private void DisplayNext()
    {
        var diag = DialogController.Instance;
        diag.DisplayDialog(_message);
        Action nextAction = _next == null ? Finish : _next.DisplayNext;
        diag.SetOptions((_continueText, nextAction));
        diag.IsVisible = true;
    }

    private void Finish()
    {
        DialogController.Instance.IsVisible = false;
        _first._onFinish?.Invoke();
    }
}