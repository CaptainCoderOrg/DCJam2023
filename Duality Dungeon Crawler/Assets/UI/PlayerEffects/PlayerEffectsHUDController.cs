using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class PlayerEffectsHUDController : MonoBehaviour
{
    public List<PlayerEffectData> PlayerEffectsData;
    private PlayerEffect _lastEffects;
    private VisualElement _effectsContainer;

    void Awake()
    {
        _effectsContainer = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("EffectsContainer");
        Debug.Assert(_effectsContainer != null, "Could not find effects container.");
    }
    
    // Update is called once per frame
    void Update()
    {
        PlayerEffect current = GameManager.Instance.Player.Effects;
        if (_lastEffects == current) { return; }
        _effectsContainer.Clear();
        foreach (PlayerEffectData data in PlayerEffectsData)
        {
            if (current.HasFlag(data.Effect))
            {
                VisualElement container = new ();
                container.AddToClassList("effect-container");
                _effectsContainer.Add(container);
                Image effect = new (){ sprite = data.Sprite };
                container.Add(effect);
                
                AddLabel(container, data);

            }
        }
        _lastEffects = current;    
    }

    private void AddLabel(VisualElement effect, PlayerEffectData data)
    {
        Label name = new (){ text = data.Description };
        name.AddToClassList("shadow-label");
        name.AddToClassList("effect-tool-tip");
        name.style.opacity = 0;
        effect.Add(name);
        effect.RegisterCallback<PointerMoveEvent>((_) => name.style.opacity = 1f);
        effect.RegisterCallback<PointerLeaveEvent>((_) => name.style.opacity = 0f);
    }
}
