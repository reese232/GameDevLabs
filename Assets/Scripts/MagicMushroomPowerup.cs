using UnityEngine;
using System.Collections;

public class MagicMushroomPowerup : BasePowerup
{
    private Collider2D mushroomCollider;
    public AudioSource appearSound;
    public AudioSource powerupSound;

    // setup this object's type
    // instantiate variables
    protected override void Start()
    {
        base.Start(); // call base class Start()
        this.type = PowerupType.MagicMushroom;
        // frozen and non-colliding until it has spawned
        mushroomCollider = GetComponent<Collider2D>();
        mushroomCollider.enabled = false;
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
        mushroomCollider.enabled = false;
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
        mushroomCollider.enabled = true;
        rigidBody.bodyType = RigidbodyType2D.Dynamic;

        StartCoroutine(MoveAtNextFrame());
        //rigidBody.AddForce(Vector2.right * 3, ForceMode2D.Impulse); // move to the right

        // play the sound
        appearSound.PlayOneShot(appearSound.clip);
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
        Debug.Log("Apply magic mushroom powerup!");
        if (i is PlayerMovement player)
            player.SetSpeedBoost();
    }
}
