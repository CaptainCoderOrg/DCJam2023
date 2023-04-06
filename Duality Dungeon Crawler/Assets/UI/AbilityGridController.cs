using System;
using System.Collections;
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
    private static VisualElement _helpLabelContainer;
    private static Label _helpLabel;
    private static int _dragCount = 0;
    private static bool _castLight = false;
    private static bool _castShade = false;
    private static bool _castHarmony = false;
    private static bool _castFloat = false;
    private static bool _castSphere = false;
    private static bool _castEye = false;
    private static bool _castShadeEye = false;
    // private static Label _abilityName;
    private static Label _abilityDescription;
    private static Label _runeName;


    public void Awake()
    {
        _dragCount = 0;
        _root = GetComponent<UIDocument>().rootVisualElement;
        VisualElement gridContainer = _root.Q<VisualElement>("GridContainer");
        _selectedRunes = _root.Q<VisualElement>("SelectedRunes");
        // _abilityName = _root.Q<Label>("AbilityName");
        _abilityDescription = _root.Q<Label>("AbilityDescription");
        _helpLabelContainer = _root.Q<VisualElement>("DragHelpLabel");
        _helpLabel = _root.Q<Label>("HelpLabel");
        _runeName = _root.Q<Label>("RuneName");
        _root.RegisterCallback<PointerDownEvent>(HandleClick);
        _root.RegisterCallback<PointerUpEvent>(HandleRelease);
        _root.RegisterCallback<PointerLeaveEvent>(HandleLeave);
    }

    public void Start()
    {
        GameManager.Instance.AbilityController.OnAbilityFinished += CheckSpells;
    }

    private void CheckSpells(AbilityDefinition ability)
    {
        if (ability is BalanceAbility) { _castHarmony = true; }
        if (ability is LightAbility) { _castLight = true; }
        if (ability is ShadeAbility) { _castShade = true; }
        if (ability is FloatAbility) { _castFloat = true; }
        if (ability is LunsEyeAbility) { _castShadeEye = true; }
        if (ability is MoonBallAbility) { _castSphere = true; }
        if (ability is BirdsEyeAbility) { _castEye = true; }
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

    private static IEnumerator HidePhrase()
    {
        yield return new WaitForSeconds(5f);
        while (RunePhrase == string.Empty && _selectedRunes.style.opacity.value > 0)
        {
            _selectedRunes.style.opacity = _selectedRunes.style.opacity.value - 0.05f;
            _abilityDescription.style.opacity = _abilityDescription.style.opacity.value - 0.05f;
        }        
    }

    private static void DisplayPhraseInfo(string phrase)
    {
        if (phrase == string.Empty)
        {
            GameManager.Instance.StartCoroutine(HidePhrase());
            return;
        }
        if (phrase.Length > 1)
        {
            _dragCount++;
            _helpLabelContainer.style.visibility = Visibility.Hidden;
        }
        _selectedRunes.Clear();
        foreach(RuneData rune in GameManager.Instance.Runes.FromPhrase(phrase))
        {
            Debug.Log(rune.name);
            Image image = new Image() { sprite = rune.sprite };
            image.AddToClassList("rune-phrase-element");
            _selectedRunes.Add(image);
            _selectedRunes.style.opacity = 1;
            
        }
        if (AbilityManifest.TryLookup(RunePhrase, out AbilityDefinition definition))
        {
            // _abilityName.text = definition.Name;
            _abilityDescription.text = $"{definition.Name}: {definition.Description}";
            _abilityDescription.style.opacity = 1;
        }
        else
        {
            // _abilityName.text = "";
            _abilityDescription.text = "";
        }
    }
    internal static void DisplayRuneName(RuneData rune)
    {
        _runeName.text = rune.Description;
        _selectedRunes.Clear();
        _abilityDescription.text = "";
    }

    internal static void DisplayHelpText(RuneData rune)
    {
        DisplayRuneName(rune);
        if (rune.Description == "Sun" && !_castLight)
        {
            ShowHelpLabel("Click to Cast Light");
        }
        else if (rune.Description == "Sun" && !_castFloat)
        {
            ShowHelpLabel("Drag to Combine with Harmony");
        }
        else if (rune.Description == "Harmony" && !_castHarmony)
        {
            ShowHelpLabel("Click to Cast Harmony");
        }
        else if (rune.Description == "Harmony" && _castLight && !_castEye && GameManager.Instance.Player.Effects.HasFlag(PlayerEffect.Light))
        {
            ShowHelpLabel("Drag to Combine with Sun");
        }
        else if (rune.Description == "Harmony" && _castShade && !_castShadeEye && GameManager.Instance.Player.Effects.HasFlag(PlayerEffect.Shade))
        {
            ShowHelpLabel("Drag to Combine with Moon");
        }
        else if (rune.Description == "Moon" && !_castShade)
        {
            ShowHelpLabel("Click to Cast Shade");
        }
        else if (rune.Description == "Moon" && !_castSphere)
        {
            ShowHelpLabel("Drag to Combine with Harmony");
        }
    }

    private static void ShowHelpLabel(string text)
    {
        _helpLabelContainer.style.visibility = Visibility.Visible;
        _helpLabel.text = text;
    }

    internal static void HideHelpText()
    {
        _helpLabelContainer.style.visibility = Visibility.Hidden;
        _runeName.text = string.Empty;
    }
}