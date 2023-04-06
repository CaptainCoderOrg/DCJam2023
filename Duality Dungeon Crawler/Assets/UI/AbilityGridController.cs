using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class AbilityGridController : MonoBehaviour
{
    public static bool IsDragging { get; private set; }
    public static event Action OnDragEnd;
    private static string _runePhrase = string.Empty;
    public static string RunePhrase 
    { 
        get => _runePhrase; 
        set
        {
            _runePhrase = value;
            DisplayPhraseInfo(_runePhrase);
        }
    }

    private VisualElement _root;
    private static VisualElement _selectedRunes;
    private static VisualElement _helpLabel;
    private static int _dragCount = 0;
    private static Label _abilityName;
    private static Label _abilityDescription;
    private static Label _runeName;


    public void Awake()
    {
        _dragCount = 0;
        _root = GetComponent<UIDocument>().rootVisualElement;
        VisualElement gridContainer = _root.Q<VisualElement>("GridContainer");
        _selectedRunes = _root.Q<VisualElement>("SelectedRunes");
        _abilityName = _root.Q<Label>("AbilityName");
        _abilityDescription = _root.Q<Label>("AbilityDescription");
        _helpLabel = _root.Q<VisualElement>("DragHelpLabel");
        _runeName = _root.Q<Label>("RuneName");
        _root.RegisterCallback<PointerDownEvent>(HandleClick);
        _root.RegisterCallback<PointerUpEvent>(HandleRelease);
        _root.RegisterCallback<PointerLeaveEvent>(HandleLeave);
    }

    private void HandleClick(PointerDownEvent evt)
    {
        IsDragging = true;
    }

    private void HandleLeave(PointerLeaveEvent evt) => Release();

    private void HandleRelease(PointerUpEvent evt) => Release();

    private void Release()
    {
        if (RunePhrase != string.Empty) { GenerateAbility(); }
        RunePhrase = string.Empty;
        IsDragging = false;
        OnDragEnd.Invoke();
    }

    private void GenerateAbility()
    {
        
        if (AbilityManifest.TryLookup(RunePhrase, out AbilityDefinition definition))
        {
            GameManager.Instance.AbilityController.UseAbility(definition);   
        }
        else
        {
            MessageController.Display("Nothing happens...");
        }
    }

    private static void DisplayPhraseInfo(string phrase)
    {
        if (phrase == string.Empty)
        {
            _selectedRunes.Clear();
            _abilityName.text = string.Empty;
        }
        if (phrase.Length > 1)
        {
            _dragCount++;
            _helpLabel.style.visibility = Visibility.Hidden;
        }
        _selectedRunes.Clear();
        foreach(RuneData rune in GameManager.Instance.Runes.FromPhrase(phrase))
        {
            Debug.Log(rune.name);
            Image image = new Image() { sprite = rune.sprite };
            image.AddToClassList("rune-phrase-element");
            _selectedRunes.Add(image);
            
        }
        if (AbilityManifest.TryLookup(RunePhrase, out AbilityDefinition definition))
        {
            _abilityName.text = definition.Name;
            _abilityDescription.text = definition.Description;
        }
        else
        {
            _abilityName.text = "";
            _abilityDescription.text = "";
        }
    }
    internal static void DisplayRuneName(RuneData rune)
    {
        _runeName.text = rune.Description;
    }

    internal static void DisplayHelpText(RuneData rune)
    {
        DisplayRuneName(rune);
        if (_dragCount < 2 && GameManager.Instance.Player.Runes.Count > 1)
        {
            _helpLabel.style.visibility = Visibility.Visible;
        }
    }

    internal static void HideHelpText()
    {
        _helpLabel.style.visibility = Visibility.Hidden;
        _runeName.text = string.Empty;
    }
}