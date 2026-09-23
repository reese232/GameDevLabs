using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    private int moveRight = -1;
    private Vector2 velocity;

    private Rigidbody2D enemyBody;

    // for reset button
    public Vector3 startPosition = new Vector3(0.0f, 0.0f, 0.0f);

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

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
    }
}
