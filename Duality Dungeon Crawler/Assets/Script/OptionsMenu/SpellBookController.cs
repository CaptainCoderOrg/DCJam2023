using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(UIDocument))]
public class SpellBookController : MonoBehaviour
{

    private PlayerControls _controls;
    private UIDocument _document;
    public VisualTreeAsset _spellEntryTemplate;
    public VisualElement _root;
    public VisualElement _spellContainer;

    public void Awake()
    {
        _document = GetComponent<UIDocument>();
        _document.enabled = true;
        _root = _document.rootVisualElement.Q<VisualElement>("ClickOut");
        _root.RegisterCallback<PointerDownEvent>(ToggleSpellBook);
        _spellContainer = _root.Q<VisualElement>("SpellContainer");
        _root.style.display = DisplayStyle.None;

    }

    public void ToggleSpellBook()
    {
        DisplayStyle current = _root.style.display.value;
        _root.style.display = current == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
        if (_root.style.display.value == DisplayStyle.Flex)
        {
            _spellContainer.Clear();
            if (GameManager.Instance.Player.Runes.Count == 0)
            {
                Label message = new Label() { text = "You do not currently know any spells." };
                message.AddToClassList("shadow-label");
                message.AddToClassList("menu-title");
                _spellContainer.Add(message);
            }
            else
            {

                foreach (AbilityDefinition ability in GameManager.Instance.AbilityManifest.Abilities)
                {
                    if (!CanCast(ability)) { continue; }
                    VisualElement spellEntry = _spellEntryTemplate.Instantiate();
                    Label description = spellEntry.Q<Label>("SpellDescription");
                    description.text = $"{ability.Name}: {ability.Description}";
                    VisualElement runePhrase = spellEntry.Q<VisualElement>("RunePhrase");
                    IEnumerable<RuneData> runes = GameManager.Instance.Runes.FromPhrase(ability.RunePhrase);
                    foreach (RuneData rune in runes)
                    {
                        VisualElement runeElement = new ();
                        runeElement.AddToClassList("spell-book-rune-element");
                        runeElement.AddToClassList("shadow-label");
                        Image runeImage = new (){sprite = rune.sprite};
                        runeElement.Add(runeImage);
                        Label runeName = new (){text = rune.Description};
                        runeElement.Add(runeName);
                        runePhrase.Add(runeElement);                        
                    }
                    _spellContainer.Add(spellEntry);
                }

            }

        }
    }

    private bool CanCast(AbilityDefinition ability)
    {
        foreach (RuneData rune in GameManager.Instance.Runes.FromPhrase(ability.RunePhrase))
        {
            if (!GameManager.Instance.Player.Runes.HasRune(rune)) { return false; }
        }
        return true;
    }

    private void ToggleSpellBook(PointerDownEvent ctx) => ToggleSpellBook();

}