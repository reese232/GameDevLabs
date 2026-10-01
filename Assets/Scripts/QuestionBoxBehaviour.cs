using System.Net.NetworkInformation;
using UnityEngine;
using System.Threading;

public class QuestionBoxBehaviour : MonoBehaviour
{
    public Animator boxAnimator;
    public Rigidbody2D questionBoxBody;
    bool isUsed = false;
    public CoinBehaviour coin;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {

        if (!isUsed)
        {
            coin.CoinAppear();
            isUsed = true;
            boxAnimator.SetBool("isCollected", true);
        }
    }

    void DisableQuestionBox()
    {
        questionBoxBody.bodyType = RigidbodyType2D.Static;
    }
}
