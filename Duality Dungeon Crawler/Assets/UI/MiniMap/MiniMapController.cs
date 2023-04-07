using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MiniMapController : MonoBehaviour
{
    private VisualElement _overlay;
    private PlayerData Player => GameManager.Instance.Player;
    // Start is called before the first frame update
    void Awake()
    {
        _overlay = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("MiniMapOverlay");
        Debug.Assert(_overlay != null, "Could not find MiniMap overlay.");
        Opacity = 0;
    }

    void Start()
    {
        GameManager.Instance.AbilityController.OnAbilityFinished += CheckDisplayMiniMap;
        PlayerMovementController.Instance.OnPositionChange += (_) => CheckFlag();
        PlayerMovementController.Instance.OnDirectionChange += (_) => CheckFlag();
        // GameManager.Instance.Player.OnEffectChange += (_) => CheckFlag();
    }

    void CheckDisplayMiniMap(AbilityDefinition ability)
    {
        if (ability is BirdsEyeAbility || ability is LunsEyeAbility)
        {
            StartCoroutine(FadeIn());
        }
    }

    private IEnumerator FadeIn()
    {
        while (Opacity < 1 && Player.Effects.HasFlag(PlayerEffect.SunsEye))
        {
            Opacity += .1f;
            yield return new WaitForSeconds(0.05f);
        }
        Opacity = 1;
    }

    private IEnumerator FadeOut()
    {
        while (Opacity > 0 && !Player.Effects.HasFlag(PlayerEffect.SunsEye))
        {
            Opacity -= .025f;
            yield return new WaitForSeconds(0.05f);
        }
        Opacity = 0;
    }

    void HideOverlay()
    {
        if (Opacity > 0)
        {
            StartCoroutine(FadeOut());
        }
    }

    private void CheckFlag()
    {
        if (Player.Effects.HasFlag(PlayerEffect.SunsEye))
        {
            Player.Effects &= ~PlayerEffect.SunsEye;
            MessageController.Display("You open your eyes.");
            
        }
        HideOverlay();
    }

    private float Opacity 
    {
        get => _overlay.style.opacity.value;
        set
        {
            _overlay.style.opacity = value;
        }
    }
}
