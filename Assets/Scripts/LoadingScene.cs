using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public CanvasGroup c;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        for (float alpha = 1f; alpha >= -0.05f; alpha -= 0.05f)
        {
            c.alpha = alpha;
            yield return new WaitForSecondsRealtime(0.1f);
        }

        // once done, got to next scene
        SceneManager.LoadSceneAsync("World 1-1-B", LoadSceneMode.Single);
    }

    public void ReturnToMain()
    {
        SceneManager.LoadSceneAsync("Main Menu", LoadSceneMode.Single);
    }
}
