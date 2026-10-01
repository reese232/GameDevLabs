using UnityEngine;

public class BrickCoin : MonoBehaviour
{
    public Animator brickAnimator;
    public CoinBehaviour coin;
    bool isCoinCollected = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        brickAnimator.SetTrigger("onHit");

        if (!isCoinCollected)
        {
            coin.CoinAppear();
            isCoinCollected = true;
        }
    }
}