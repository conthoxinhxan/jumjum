using UnityEngine;

public class WorldSpaceUI : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _player;
    [SerializeField] private float _showDistance = 3f;
    [SerializeField] private GameObject _panel;

    private void Start() {
        _camera = Camera.main;
        _player = FindAnyObjectByType<PlayerMovement>().transform;
    }

    private void LateUpdate()
    {
        if (_camera == null || _player == null)
        {
            return;
        }

        float distance = Vector3.Distance(_player.position, transform.position);

        bool isNearby = distance <= _showDistance;

        _panel.SetActive(isNearby);

        if (isNearby)
        {
            transform.rotation = _camera.transform.rotation;
        }
    }
}