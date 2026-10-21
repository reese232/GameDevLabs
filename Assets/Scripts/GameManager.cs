using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

//public class GameManager : MonoBehaviour
public class GameManager : Singleton<GameManager>
{
    // events
    public UnityEvent gameStart;
    public UnityEvent gameRestart;
    public UnityEvent<int> scoreChange;
    public UnityEvent gameOver;
    public UnityEvent gamePause;

    private int score = 0;
    // using scriptable object
    public IntVariable gameScore;

    public UnityEvent<BasePowerup> powerupAffectsPlayer = new UnityEvent<BasePowerup>();
    public UnityEvent<BasePowerup> powerupAffectsGame = new UnityEvent<BasePowerup>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameStart.Invoke();
        Time.timeScale = 1.0f;

        // subscribe to scene manager scene change
        SceneManager.activeSceneChanged += SceneSetup;

        powerupAffectsGame.AddListener(RequestPowerupEffect);
    }

    public void RequestPowerupEffect(BasePowerup p)
    {
        p.ApplyPowerup(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SceneSetup(Scene current, Scene next)
    {
        gameStart.Invoke();
        //SetScore(score);
        SetScore();
    }

    public void GameRestart()
    {
        // reset score
        //score = 0;
        gameScore.Value = 0;
        //SetScore(0);
        SetScore();
        gameRestart.Invoke();
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void IncreaseScore(int increment)
    {
        Debug.Log("IncreaseScore " + increment + ", gameScore = " + gameScore.Value);
        //score += increment;
        //SetScore(score);
        gameScore.ApplyChange(increment);
        SetScore();
    }

    //public void SetScore(int score)
    //{
    //    scoreChange.Invoke(score);
    //}
    public void SetScore()
    {
        scoreChange.Invoke(gameScore.Value);
    }

    public void GamePause()
    {
        Time.timeScale = 0.0f;
        gamePause.Invoke();
    }

    public void GameStart()
    {
        Time.timeScale = 1.0f;
        gameStart.Invoke();
    }

    public void GameOver()
    {
        Time.timeScale = 0.0f;
        gameOver.Invoke();
    }
}
