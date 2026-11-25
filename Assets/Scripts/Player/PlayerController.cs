using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerInput Input { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerAttack Attack { get; private set; }
    public PlayerHealth Health { get; private set; }
    public PlayerItemUse ItemUse { get; private set; }

    private void Awake()
    {
        Input = GetComponent<PlayerInput>();
        Movement = GetComponent<PlayerMovement>();
        Attack = GetComponent<PlayerAttack>();
        Health = GetComponent<PlayerHealth>();
        ItemUse = GetComponent<PlayerItemUse>();
    }

    public void Initialize(PlayerData playerData)
    {
        Health.Initialize(playerData.CurrentHealth, playerData.MaxHeath);
        ItemUse.Initialize(playerData.Inventory);

        if (playerData.Position != null)
        {
            transform.position = new Vector3(
                playerData.Position.X,
                playerData.Position.Y,
                playerData.Position.Z);
        }
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
