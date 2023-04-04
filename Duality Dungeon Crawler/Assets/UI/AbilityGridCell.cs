using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AbilityGridCellElement : VisualElement
{
    public readonly static string AbilityGridElementStyle = "ability-grid-element";
    public readonly static string AbilityGridElementSelectedStyle = "ability-grid-element-selected";
    private bool _selected;
    private bool _isAvailable;
    public AbilityGridCellElement()
    {
        AddToClassList(AbilityGridElementStyle);
        RegisterCallback<PointerEnterEvent>(HandleDrag);
        RegisterCallback<PointerDownEvent>(HandleClick);
        AbilityGridController.OnDragEnd += () => IsSelected = false;
        Debug.Log(GameManager.Instance);
        GameManager.Instance.Player.Runes.OnRuneAdded += HandleRuneAdded;
        GameManager.Instance.Player.Runes.OnReady += HandleReady;
    }

    public bool IsSelected
    {
        get => _selected;
        private set
        {
            _selected = value;
            System.Action<string> action = _selected ? AddToClassList : RemoveFromClassList;
            action.Invoke(AbilityGridElementSelectedStyle);
        }
    }

    private void HandleClick(PointerDownEvent evt)
    {
        Select();
    }

    private void HandleDrag(PointerEnterEvent evt)
    {
        if (AbilityGridController.IsDragging)
        {
            Select();
        }
    }

    private void Select()
    {
        if (!_isAvailable) { return; }
        AbilityGridController.RunePhrase += Rune;
        IsSelected = true;
    }

    private void HandleRuneAdded(RuneData newRune)
    {
        if (newRune.RuneIndex.ToString() == Rune)
        {
            EnableImage(newRune);
        }
    }

    private void EnableImage(RuneData rune)
    {
        Clear();
        Image img = new() { sprite = rune.sprite };
        img.AddToClassList("rune-image");
        _isAvailable = true;
        Add(img);
    }


    private void HandleReady(PlayerRunes runes)
    {
        if (runes.HasRune(Rune, out RuneData rune))
        {
            EnableImage(rune);
        }
        else
        {
            _isAvailable = false;
            Clear();
        }
    }

    public void Init(string rune) => Rune = rune;

    public string Rune { get; set; }

    public sealed new class UxmlFactory : UxmlFactory<AbilityGridCellElement, UxmlTraits> { }

    public sealed new class UxmlTraits : VisualElement.UxmlTraits
    {
        private UxmlStringAttributeDescription _rune = new() { name = "rune", defaultValue = string.Empty };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var gridCell = ve as AbilityGridCellElement;
            string AsString(UxmlStringAttributeDescription e) => e.GetValueFromBag(bag, cc);
            gridCell.Init(AsString(_rune));
        }
    }
}
