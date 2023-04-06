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
        PlayerMovementController.Instance.OnPositionChange += (_) => HideOverlay();
        PlayerMovementController.Instance.OnDirectionChange += (_) => HideOverlay();
    }

    void CheckDisplayMiniMap(AbilityDefinition ability)
    {
        if (ability is BirdsEyeAbility)
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
        if (Player.Effects.HasFlag(PlayerEffect.SunsEye))
        {
            Player.Effects &= ~PlayerEffect.SunsEye;
            MessageController.Display("You open your eyes.");
            StartCoroutine(FadeOut());
        }
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
