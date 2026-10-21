using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PowerupManager : Singleton<PowerupManager>
{
    public UnityEvent<BasePowerup> powerupCollected = new UnityEvent<BasePowerup>();

    void Start()
    {
        powerupCollected.AddListener(FilterAndCastPowerup);
    }

    void FilterAndCastPowerup(BasePowerup p)
    {
        Debug.Log("filterandcastpowerup!");
        switch (p.type)
        {
            case PowerupType.MagicMushroom:
                GameManager.instance.powerupAffectsPlayer.Invoke(p);
                break;
            case PowerupType.OneUpMushroom:
                GameManager.instance.powerupAffectsGame.Invoke(p);
                break;
            case PowerupType.Coin:
                Debug.Log("filterandcastpowerup.coin!");
                GameManager.instance.powerupAffectsGame.Invoke(p);
                break;
            case PowerupType.StarMan:
                GameManager.instance.powerupAffectsPlayer.Invoke(p);
                break;
        }
    }
}
