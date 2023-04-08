using CaptainCoder.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CreditsController : MonoBehaviour
{    public OptionsMenuController Options;
    public void Awake()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        Button newGame = root.Q<Button>("TitleButton");
        newGame.clicked += () => SceneManager.LoadScene("TitleScreen");
        
    }

}