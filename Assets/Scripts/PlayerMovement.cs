using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//public class PlayerMovement : Singleton<PlayerMovement>
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D marioBody; // reference to RigidBody2D

    // from scriptable object
    public GameConstants gameConstants;
    float deathImpulse;
    float upSpeed;
    float maxSpeed;
    float speed;

    //// move mario
    //public float speed = 10;
    //// stop mario
    //public float maxSpeed = 20;
    //// for jump
    //public float upSpeed = 10;
    private bool onGroundState = true;
    // to flip mario
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    // for restart button
    //public TextMeshProUGUI scoreText;
    //public GameObject enemies;
    //// to reset score
    //public JumpOverGoomba jumpOverGoomba;
    //// for game over screen
    //public GameManager gameManager;
    //public TextMeshProUGUI finalScoreText;

    // for animation
    public Animator marioAnimator;
    // for audio
    public AudioSource marioAudio;
    // for death
    public AudioSource marioDeathAudio;
    //public float deathImpulse = 15;
    // for reset camera
    public Transform gameCamera;
    // for LayerMask
    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);

    // state
    [System.NonSerialized]
    public bool alive = true;

    Collider2D marioCollider;

    // for observer pattern
    private bool moving = false;
    private bool jumpedState = false;

    private Coroutine speedBoostRoutine;

    void Awake()
    {
        // subscribe to Game Restart event
        GameManager.instance.gameRestart.AddListener(GameRestart);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set constants
        speed = gameConstants.speed;
        maxSpeed = gameConstants.maxSpeed;
        deathImpulse = gameConstants.deathImpulse;
        upSpeed = gameConstants.upSpeed;

        marioSprite = GetComponent<SpriteRenderer>(); // for flip mario
        // set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();

        // update animator state
        marioAnimator.SetBool("onGround", onGroundState);

        marioCollider = GetComponent<Collider2D>();

        //// subscribe to scene manager scene change
        //SceneManager.activeSceneChanged += SetStartingPosition;

        GameManager.instance.powerupAffectsPlayer.AddListener(RequestPowerupEffect);
    }

    // Update is called once per frame
    void Update()
    {
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.linearVelocity.x));
    }

    public void RequestPowerupEffect(BasePowerup p)
    {
        p.ApplyPowerup(this);
    }

    void FlipMarioSprite(int value)
    {
        if (value == -1 && faceRightState) // change to face left
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.linearVelocity.x > 0.05f)
            {
                marioAnimator.SetTrigger("onSkid");
            }
        }

        else if (value == 1 && !faceRightState) // change to face right
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.linearVelocity.x < -0.05f)
            {
                marioAnimator.SetTrigger("onSkid");
            }
        }
    }

    //public void SetStartingPosition(Scene current, Scene next)
    //{
    //    if (next.name == "World 1-2")
    //    {
    //        // change the position accordingly in your World-1-2 case
    //        this.transform.position = new Vector3(-7.32f, 0.33f, -0.1f);
    //    }
    //}

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
        if (alive && moving)
        {
            Move(faceRightState == true ? 1 : -1);
        }
    }

    void Move(int value)
    {
        Vector2 movement = new Vector2(value, 0);
        // check if it doesn't go beyond max speed
        if (marioBody.linearVelocity.magnitude < maxSpeed)
        {
            marioBody.AddForce(movement * speed);
        }
    }

    public void MoveCheck(int value)
    {
        if (value == 0)
        {
            moving = false;
        }
        else
        {
            FlipMarioSprite(value);
            moving = true;
            Move(value);
        }
    }

    public void Jump()
    {
        if (alive && onGroundState)
        {
            // jump
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpedState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }

    public void JumpHold()
    {
        if (alive && jumpedState)
        {
            // jump higher
            marioBody.AddForce(Vector2.up * upSpeed * 30, ForceMode2D.Force);
            jumpedState = false;
        }
    }

    public void SetSpeedBoost()
    {
        // restart the timer if a second mushroom is collected
        if (speedBoostRoutine != null) StopCoroutine(speedBoostRoutine);
        speedBoostRoutine = StartCoroutine(SpeedBoostTimer());
    }

    private IEnumerator SpeedBoostTimer()
    {
        // compute from the constants, so boosts never stack
        speed = gameConstants.speed * gameConstants.speedBoostMultiplier;
        maxSpeed = gameConstants.maxSpeed * gameConstants.speedBoostMultiplier;

        yield return new WaitForSeconds(gameConstants.speedBoostDuration);

        ResetSpeed();
        speedBoostRoutine = null;
    }

    private void ResetSpeed()
    {
        speed = gameConstants.speed;
        maxSpeed = gameConstants.maxSpeed;
    }

    //public void Death()
    //{
    //    Debug.Log("Collided with Goomba!");
    //    // play death animation
    //    marioAnimator.Play("mario-die");
    //    marioDeathAudio.PlayOneShot(marioDeathAudio.clip);
    //    alive = false;
    //}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && alive)
        {
            //StartCoroutine(CheckEnemyHit(other));
            Debug.Log("Collided with Goomba!");
            //Time.timeScale = 0.0f; // freezes time -> "stops" game

            //finalScoreText.text = jumpOverGoomba.scoreText.text;

            marioCollider.enabled = false;
            // play death animation
            marioAnimator.Play("mario-die");
            marioDeathAudio.PlayOneShot(marioDeathAudio.clip);
            alive = false;
        }
    }

    //private IEnumerator CheckEnemyHit(Collider2D other)
    //{
    //    yield return null;
    //    if (!alive || other == null) yield break;

    //    var enemy = other.GetComponentInParent<EnemyMovement>();
    //    if (enemy != null && !enemy.alive) yield break; // it was stomped, Mario is safe

    //    Debug.Log("Collided with Goomba!");
    //    marioAnimator.Play("mario-die");
    //    marioDeathAudio.PlayOneShot(marioDeathAudio.clip);
    //    alive = false;
    //}

    void PlayDeathImpulse()
    {
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }

    void GameOverScene()
    {
        // stop time
        Time.timeScale = 0.0f;
        // set game over screen
        //gameManager.GameOver();
        GameManager.instance.GameOver();
    }


    void PlayJumpSound()
    {
        // play jump sound
        marioAudio.PlayOneShot(marioAudio.clip);
    }

    public void GameRestart()
    {
        marioCollider.enabled = true;

        // reset position
        marioBody.transform.position = new Vector3(-3.45f, 0.86f, -0.1f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;

        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;

        // reset camera position
        gameCamera.position = new Vector3(0, 3, -10);

        // reset speed
        if (speedBoostRoutine != null) StopCoroutine(speedBoostRoutine);
        speedBoostRoutine = null;
        ResetSpeed();
    }
}
