using UnityEngine;
using UnityEngine.Events;

public class CoinPowerup : BasePowerup
{
    private Collider2D coinCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        this.type = PowerupType.Coin;
        // frozen and non-colliding until it has spawned
        coinCollider = GetComponent<Collider2D>();
        coinCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void SpawnPowerup()
    {
        Debug.Log("Coin spawned");
        spawned = true;
        // play the sound
        AudioSource source = this.GetComponent<AudioSource>();
        source.PlayOneShot(source.clip);
        // invoke PowerupCollectedEvent
        PowerupManager.instance.powerupCollected.Invoke(this);
    }

    // hide the base destroy method
    public new void DestroyPowerup()
    {

    }

    public override void ApplyPowerup(MonoBehaviour i)
    {
        Debug.Log("Coin ApplyPowerup called by " + i);
        // try
        GameManager manager;
        bool result = i.TryGetComponent<GameManager>(out manager);
        
        if (result)
        {
            manager.IncreaseScore(1);
        }
    }
}
