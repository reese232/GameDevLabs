using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StompGoomba : MonoBehaviour
{
    GameManager gameManager;
    public EnemyMovement goomba;
    public Collider2D stompCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            goomba.StompGoomba();
            gameManager.IncreaseScore(1);
            stompCollider.enabled = false;
            Debug.Log("Goomba stomped!");
        }
    }

    public void GameRestart()
    {
        stompCollider.enabled = true;
    }

}
