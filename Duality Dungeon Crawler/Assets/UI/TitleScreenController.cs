using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TitleScreenController : MonoBehaviour
{

    public OptionsMenuController Options;
    public void Awake()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        Button newGame = root.Q<Button>("NewGameButton");
        newGame.clicked += StartGame;

        Button options = root.Q<Button>("OptionsButton");
        options.clicked += OptionsMenu;
        
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