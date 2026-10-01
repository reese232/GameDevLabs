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
    // for animation
    public Animator marioAnimator;
    // for audio
    public AudioSource marioAudio;
    // for death
    public AudioClip marioDeath;
    public float deathImpulse = 15;
    // for reset camera
    public Transform gameCamera;
    // for LayerMask
    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);

    // state
    [System.NonSerialized]
    public bool alive = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        marioSprite = GetComponent<SpriteRenderer>(); // for flip mario
        // set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();

        // update animator state
        marioAnimator.SetBool("onGround", onGroundState);
    }

    // Update is called once per frame
    void Update()
    {
        // toggle mario direction
        if (Input.GetKeyDown("a") && faceRightState) // change to face left
        {
            faceRightState = false; 
            marioSprite.flipX = true;
            if (marioBody.linearVelocity.x > 0.1f)
                marioAnimator.SetTrigger("onSkid");
        }
        if (Input.GetKeyDown("d") && !faceRightState) // change to face right
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.linearVelocity.x < -0.1f)
                marioAnimator.SetTrigger("onSkid");
        }
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.linearVelocity.x));
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //if (col.gameObject.CompareTag("Ground")) onGroundState = true;

        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState)
        {
            onGroundState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }

    // FixedUpdate is called 50 times a second
    void FixedUpdate()
    {
        if (alive)
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
                // update animator state
                marioAnimator.SetBool("onGround", onGroundState);
            }
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && alive)
        {
            Debug.Log("Collided with Goomba!");
            //Time.timeScale = 0.0f; // freezes time -> "stops" game

            finalScoreText.text = jumpOverGoomba.scoreText.text;

            // play death animation
            marioAnimator.Play("mario-die");
            marioAudio.PlayOneShot(marioDeath);
            alive = false;
        }
    }

    void PlayDeathImpulse()
    {
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }

    void GameOverScene()
    {
        // stop time
        Time.timeScale = 0.0f;
        // set game over screen
        gameManager.GameOver(); 
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
        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;
        // reset camera position
        gameCamera.position = new Vector3(0, 3, -10);
    }

    void PlayJumpSound()
    {
        // play jump sound
        marioAudio.PlayOneShot(marioAudio.clip);
    }
}
