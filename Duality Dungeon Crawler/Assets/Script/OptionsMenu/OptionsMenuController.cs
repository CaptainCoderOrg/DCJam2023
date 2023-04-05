using UnityEngine;
using UnityEngine.UIElements;
using CaptainCoder.Audio;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(UIDocument))]
public class OptionsMenuController : MonoBehaviour
{

    private PlayerControls _controls;
    private UIDocument _document;

    public void Awake()
    {
        _document = GetComponent<UIDocument>();
        Slider musicVolume = _document.rootVisualElement.Q<Slider>("MusicVolume");
        musicVolume.value = MusicController.Instance.MusicVolume * 100;
        musicVolume.RegisterValueChangedCallback(UpdateMusicVolume);
        Slider soundVolume = _document.rootVisualElement.Q<Slider>("SoundVolume");
        // soundVolume.RegisterValueChangedCallback(UpdateSoundVolume);
        Button quitButton = _document.rootVisualElement.Q<Button>("QuitButton");
    }

    private void UpdateMusicVolume(ChangeEvent<float> evt)
    {
        MusicController.Instance.MusicVolume = evt.newValue / 100;
    }

    public void OnEnable()
    {
        _controls ??= new PlayerControls();
        _controls.Enable();
        _controls.UIControls.ToggleMenu.started += ToggleMenu;
        _document.rootVisualElement.style.display = DisplayStyle.None;
    }

    public void OnDisable()
    {
        _controls.UIControls.ToggleMenu.started -= ToggleMenu;
    }

    private void ToggleMenu(CallbackContext ctx)
    {
        DisplayStyle current = _document.rootVisualElement.style.display.value;
        _document.rootVisualElement.style.display = current == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
    }

}