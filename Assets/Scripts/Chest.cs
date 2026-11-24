using UnityEngine;
using UnityEngine.InputSystem;

public class Chest : MonoBehaviour
{
    private PlayerController _nearbyPlayer;
    private bool _isOpened;
    private Animator _chestAnimator;
    private WorldSpaceUI _chestUI;

    private void Start()
    {
        _chestAnimator = GetComponent<Animator>();
        _chestUI = GetComponentInChildren<WorldSpaceUI>();
    }

    private void Update()
    {
        if (!_isOpened && _nearbyPlayer != null && Keyboard.current?.fKey.wasPressedThisFrame == true)
            OpenChest();
    }

    private void OpenChest()
    {
        _isOpened = true;
        _nearbyPlayer.ItemUse.AddHealingItem();
        _chestUI?.Hide();

        if (_chestAnimator != null)
            _chestAnimator.SetTrigger("Open");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.TryGetComponent(out PlayerController player))
            _nearbyPlayer = player;
    }

    private void OnCollisionExit(Collision other)
    {
        if (_nearbyPlayer != null && other.transform == _nearbyPlayer.transform)
            _nearbyPlayer = null;
    }
}