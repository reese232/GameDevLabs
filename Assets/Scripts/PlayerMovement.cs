using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // move mario
    public float speed = 10;
    private Rigidbody2D marioBody; // reference to RigidBody2D
    // stop mario
    public float maxSpeed = 20;
    // for jump
    public float upSpeed = 10;
    private bool onGroundState = true;
    // to flip mario
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    // for restart button
    public TextMeshProUGUI scoreText;
    public GameObject enemies;
    // to reset score
    public JumpOverGoomba jumpOverGoomba;
    // for game over screen
    public GameManagerScript gameManager;
    public TextMeshProUGUI finalScoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        marioSprite = GetComponent<SpriteRenderer>(); // for flip mario
        // set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // toggle mario direction
        if (Input.GetKeyDown("a") && faceRightState) // face left
        {
            faceRightState = false; 
            marioSprite.flipX = true;
        }
        if (Input.GetKeyDown("d") && !faceRightState) // face right
        {
            faceRightState = true;
            marioSprite.flipX = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground")) onGroundState = true;
    }

    // FixedUpdate is called 50 times a second
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        //Vector2 movement = new Vector2(moveHorizontal, 0);
        //marioBody.AddForce(movement * speed);

        // move mario
        if (Mathf.Abs(moveHorizontal) > 0)
        {
            Vector2 movement = new Vector2(moveHorizontal, 0);
            // check if it doesn't go beyond maxSpeed
            if (marioBody.linearVelocity.magnitude < maxSpeed)
                marioBody.AddForce(movement * speed);
        }

        // stop mario
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
        {
            // stop
            marioBody.linearVelocity = Vector2.zero;
        }

        // jump
        if (Input.GetKeyDown("space") && onGroundState)
        {
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with Goomba!");
            Time.timeScale = 0.0f; // freezes time -> "stops" game

            finalScoreText.text = jumpOverGoomba.scoreText.text;
            gameManager.GameOver(); // game over screen
        }
    }

    public void RestartButtonCallback(int input)
    {
        Debug.Log("Restart!");
        // reset everything
        ResetGame();
        Debug.Log("ResetGame() called!");
        // resume time
        Time.timeScale = 1.0f;
    }

    private void ResetGame()
    {
        // reset position
        marioBody.transform.position = new Vector3(-3.45f, 0.86f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        // reset score
        scoreText.text = "Score: 0";
        jumpOverGoomba.score = 0;
        // reset Goomba
        foreach (Transform eachChild in enemies.transform)
        {
            eachChild.transform.localPosition = eachChild.GetComponent<EnemyMovement>().startPosition;
        }
    }
}
