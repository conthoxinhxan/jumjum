using UnityEngine.UI;
using UnityEngine;

public class HealthPanel : MonoBehaviour
{
    public Slider HealthSlider;

    public void Initialize(PlayerHealth player)
    {
        HealthSlider.minValue = 0f;
        HealthSlider.maxValue = Mathf.Max(1, player.MaxHealth);
        HealthSlider.value = HealthSlider.maxValue;

        player.HealthChanged += UpdateHealth;
    }

    public void UpdateHealth(float currentHealth)
    {
        HealthSlider.value = Mathf.Clamp(currentHealth, HealthSlider.minValue, HealthSlider.maxValue);
    }
}