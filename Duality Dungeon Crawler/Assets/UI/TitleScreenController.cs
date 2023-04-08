using CaptainCoder.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TitleScreenController : MonoBehaviour
{
    public AudioClip Music;
    public OptionsMenuController Options;
    public void Awake()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        Button newGame = root.Q<Button>("NewGameButton");
        newGame.clicked += StartGame;

        Button options = root.Q<Button>("OptionsButton");
        options.clicked += OptionsMenu;

        MusicController.Instance.StartTrack(Music);
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("YinYangTemple");
    }

    public void ContinueGame()
    {

    }

    public void OptionsMenu()
    {
        Options.ToggleMenu();
    }
}