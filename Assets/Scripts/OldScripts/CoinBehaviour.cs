using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    public Animator coinAnimator;
    //bool coinCollected = false;
    public AudioSource coinAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //coinCollected = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnCollisionEnter2D(Collision2D col)
    //{
    //    if (!coinCollected)
    //    {
    //        coinAnimator.SetTrigger("onBrickHit");
    //        coinCollected = true;
    //    }
    //}

    public void CoinAppear()
    {
        coinAnimator.SetTrigger("onBrickHit");
    }

    void PlayCoinAudio()
    {
        coinAudio.PlayOneShot(coinAudio.clip);
    }
}
