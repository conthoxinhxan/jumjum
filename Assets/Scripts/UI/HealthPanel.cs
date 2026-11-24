using UnityEngine.UI;
using UnityEngine;

public class HealthPanel : MonoBehaviour
{
    public Slider HealthSlider;
    private PlayerHealth _player;

    public void Initialize(PlayerHealth player)
    {
        if (_player != null)
            _player.HealthChanged -= UpdateHealth;

        _player = player;
        HealthSlider.minValue = 0f;
        HealthSlider.maxValue = Mathf.Max(1, player.MaxHealth);
        UpdateHealth(player.CurrentHealth);
        _player.HealthChanged += UpdateHealth;
    }

    private void OnDestroy()
    {
        if (_player != null)
            _player.HealthChanged -= UpdateHealth;
    }

    public void UpdateHealth(float currentHealth)
    {
        HealthSlider.value = Mathf.Clamp(currentHealth, HealthSlider.minValue, HealthSlider.maxValue);
    }
}
