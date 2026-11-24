using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerInput Input { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerAttack Attack { get; private set; }
    public PlayerHealth Health { get; private set; }

    private void Awake()
    {
        Input = GetComponent<PlayerInput>();
        Movement = GetComponent<PlayerMovement>();
        Attack = GetComponent<PlayerAttack>();
        Health = GetComponent<PlayerHealth>();
    }

    public void Initialize(PlayerData playerData)
    {
        Health.Initialize(playerData.CurrentHealth, playerData.MaxHeath);
    }

    public void SetGameplayEnabled(bool isEnabled)
    {
        // if (Input != null)
        //     Input.enabled = isEnabled;
        // if (Movement != null)
        //     Movement.enabled = isEnabled;
        // if (Attack != null)
        //     Attack.enabled = isEnabled;
    }
}