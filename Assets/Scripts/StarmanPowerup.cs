using System.Collections;
using UnityEngine;

public class StarmanPowerup : BasePowerup
{
    private Collider2D starCollider;
    public AudioSource appearSound;
    public AudioSource powerupSound;

    // setup this object's type
    // instantiate variables
    protected override void Start()
    {
        base.Start(); // call base class Start()
        this.type = PowerupType.StarMan;
        // frozen and non-colliding until it has spawned
        starCollider = GetComponent<Collider2D>();
        starCollider.enabled = false;
        rigidBody.bodyType = RigidbodyType2D.Static;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && spawned)
        {
            spawned = false;
            // TODO: do something when colliding with Player
            PowerupManager.instance.powerupCollected.Invoke(this);
            powerupSound.PlayOneShot(powerupSound.clip);
            StartCoroutine(DestroyAfterSound());
            //// then destroy powerup (optional)
            //DestroyPowerup();
        }
        else if (col.gameObject.layer == 10) // else if hitting Pipe, flip travel direction
        {
            if (spawned)
            {
                goRight = !goRight;
                rigidBody.AddForce(Vector2.right * 3 * (goRight ? 1 : -1), ForceMode2D.Impulse);

            }
        }
    }

    private IEnumerator DestroyAfterSound()
    {
        starCollider.enabled = false;
        rigidBody.bodyType = RigidbodyType2D.Static;
        foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
            sr.enabled = false;

        yield return new WaitForSeconds(powerupSound.clip.length);
        DestroyPowerup();
    }

    // interface implementation, called by animator
    public override void SpawnPowerup()
    {
        spawned = true;

        // enable collider
        starCollider.enabled = true;
        rigidBody.bodyType = RigidbodyType2D.Dynamic;

        appearSound.PlayOneShot(appearSound.clip);

        StartCoroutine(MoveAtNextFrame());
        //rigidBody.AddForce(Vector2.right * 3, ForceMode2D.Impulse); // move to the right
    }

    private IEnumerator MoveAtNextFrame()
    {
        yield return new WaitForFixedUpdate();
        rigidBody.AddForce(Vector2.right * 3, ForceMode2D.Impulse); // move to the right
    }


    // interface implementation
    public override void ApplyPowerup(MonoBehaviour i)
    {
        // TODO: do something with the object
        Debug.Log("Apply starman powerup!");
    }
}
