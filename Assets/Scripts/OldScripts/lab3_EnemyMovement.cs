using UnityEngine;

public class lab3_EnemyMovement : MonoBehaviour
{
    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    private int moveRight = -1;
    private Vector2 velocity;

    private Rigidbody2D enemyBody;

    // for reset button
    public Vector3 startPosition = new Vector3(0.0f, 0.0f, 0.0f);

    // for animation
    public Animator goombaAnimator;
    public AudioSource goombaStompAudio;
    public Collider2D goombaCollider;
    public StompGoomba stompGoomba;

    [System.NonSerialized]
    public bool alive = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        // get the starting position
        originalX = transform.position.x;
        ComputeVelocity();
    }

    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
    }

    void MoveGoomba()
    {
        Vector2 temp = enemyBody.position + velocity * Time.fixedDeltaTime;
        Debug.Log("MoveGoomba called! " + temp);
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        //// Version A: Update
        //if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset)
        //{
        //    // move goomba
        //    MoveGoomba();
        //}
        //else
        //{
        //    // change direction
        //    moveRight *= -1;
        //    ComputeVelocity();
        //    MoveGoomba();
        //}
    }

    // Version B: FixedUpdate
    private void FixedUpdate()
    {
        if (alive)
        {
            if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset)
            {
                // move goomba
                MoveGoomba();
            }
            else
            {
                // change direction
                moveRight *= -1;
                ComputeVelocity();
                MoveGoomba();
            }
    }
}

    public void StompGoomba()
    {
        goombaAnimator.Play("goomba-stomp");
        goombaCollider.enabled = false;
        alive = false;
    }

    public void PlayStompSound()
    {
        goombaStompAudio.PlayOneShot(goombaStompAudio.clip);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
    }

    public void GameRestart()
    {
        goombaCollider.enabled = true;
        transform.localPosition = startPosition;
        originalX = transform.position.x;
        moveRight = -1;
        ComputeVelocity();
        goombaAnimator.SetTrigger("gameRestart");
        stompGoomba.GameRestart();
        alive = true;
    }
}
