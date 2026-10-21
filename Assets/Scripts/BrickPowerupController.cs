using UnityEngine;

public class BrickPowerupController : MonoBehaviour, IPowerupController
{
    public Animator powerupAnimator;
    public BasePowerup powerup;
    public bool isBreakable = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Brick hit by: " + other.gameObject.name + " tag=" + other.gameObject.tag, this);
        if (other.gameObject.tag == "Player")
        {
            if (!powerup.hasSpawned)
            {
                // enable sprite
                this.GetComponent<SpriteRenderer>().enabled = true;
                // bounce
                this.GetComponent<Animator>().SetTrigger("bounce");
                // spawn powerup
                powerupAnimator.SetTrigger("spawned");

                if (!isBreakable)
                {
                    // show disabled sprite if it's not breakable type of brick
                    this.GetComponent<Animator>().SetTrigger("spawned");
                }
            }
            else
            {
                // else if it's breakable type, can still bounce after powerup has spawned
                if (isBreakable)
                {
                    // bounce
                    this.GetComponent<Animator>().SetTrigger("bounce");
                }
            }
        }
    }

    // the brick bouncing movement is not due to spring but due to animation
    // there's no need to set this
    // just implemented to conform to interface
    public void Disable()
    {
        //Debug.Log("this invis block has been disabled");
        //this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        //transform.localPosition = new Vector3(0, 0, 0);
    }
}
