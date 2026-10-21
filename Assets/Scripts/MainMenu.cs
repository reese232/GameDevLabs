using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject highscoreText;
    public IntVariable gameScore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetHighscore();
    }

    public void GoToLoadScene()
    {
        SceneManager.LoadSceneAsync("Loading Scene", LoadSceneMode.Single);
    }

    void SetHighscore()
    {
        // see https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings
        highscoreText.GetComponent<TextMeshProUGUI>().text = "TOP- " + gameScore.previousHighestValue.ToString("D6");

    }

    public void ResetHighscore()
    {
        // reset selection after highscore is pressed
        GameObject eventSystem = GameObject.Find("EventSystem");
        eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

        gameScore.ResetHighestValue();
        // set highscore text
        SetHighscore();
    }
}
