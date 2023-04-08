using CaptainCoder.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TitleScreenController : MonoBehaviour
{
    public RuneManifest Runes;
    public PlayerData Player;
    public AudioClip Music;
    public OptionsMenuController Options;
    public void Awake()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        Button newGame = root.Q<Button>("NewGameButton");
        newGame.clicked += StartGame;

        Button continueBtn = root.Q<Button>("ContinueButton");
        continueBtn.clicked += ContinueGame;

        Button options = root.Q<Button>("OptionsButton");
        options.clicked += OptionsMenu;

        MusicController.Instance.StartTrack(Music);
        
    }

    public void StartGame()
    {
        foreach(RuneData rune in Runes.Runes)
        {
            PlayerPrefs.DeleteKey(rune.Description);
        }
        SceneManager.LoadScene("YinYangTemple");
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("YinYangTemple");
    }

    public void OptionsMenu()
    {
        Options.ToggleMenu();
    }
}