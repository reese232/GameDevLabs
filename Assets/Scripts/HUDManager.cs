using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
//public class HUDManager : Singleton<HUDManager>
{
    // first vector is game active, second vector is game over
    private Vector3[] scoreTextPosition = {
        new Vector3(-274, 194, 0),
        new Vector3(17, 11, 0)
        };
    private Vector3[] restartButtonPosition = {
        new Vector3(363, 191, 0),
        new Vector3(18, -75, 0)
    };

    public GameObject scoreText;
    public Transform restartButton;
    public GameObject gameOverScreen;

    public GameObject highscoreText;
    public IntVariable gameScore;

    public Transform pauseButton;
    public Transform playButton;
    public GameObject gamePauseScreen;

    void Awake()
    {
        // subscribe to events
        GameManager.instance.gameStart.AddListener(GameStart);
        GameManager.instance.gameOver.AddListener(GameOver);
        GameManager.instance.gameRestart.AddListener(GameStart);
        GameManager.instance.scoreChange.AddListener(SetScore);
        GameManager.instance.gamePause.AddListener(GamePause);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStart()
    {
        // hide gameover panel
        gameOverScreen.SetActive(false);
        gamePauseScreen.SetActive(false);
        scoreText.transform.localPosition = scoreTextPosition[0];
        restartButton.localPosition = restartButtonPosition[0];
    }

    public void SetScore(int score)
    {
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();
    }

    public void GamePause()
    {
        gamePauseScreen.SetActive(true);
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        scoreText.transform.localPosition = scoreTextPosition[1];
        restartButton.localPosition = restartButtonPosition[1];
        // set highscore
        highscoreText.GetComponent<TextMeshProUGUI>().text = "TOP- " + gameScore.previousHighestValue.ToString("D6");
        // show
        highscoreText.SetActive(true);
    }
}
