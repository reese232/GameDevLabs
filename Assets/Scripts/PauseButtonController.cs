using UnityEngine;
using UnityEngine.UI;

public class PauseButtonController : MonoBehaviour, IInteractiveButton
{
    private bool isPaused = false;
    public Sprite pauseIcon;
    public Sprite playIcon;
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonClick()
    {
        Time.timeScale = isPaused ? 1.0f : 0.0f;
        isPaused = !isPaused;
        if (isPaused)
        {
            GameManager.instance.GamePause();
            image.sprite = playIcon;
            AudioListener.pause = true;
        }
        else
        {
            GameManager.instance.GameStart();
            image.sprite = pauseIcon;
            AudioListener.pause = false;
        }
    }
}
