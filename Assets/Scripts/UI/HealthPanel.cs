using UnityEngine;

public class HealthPanel : MonoBehaviour
{
    public int MaxHealth = 3;
    public GameObject HeartPrefab;
    public Transform HeartContainer;

    private GameObject[] _hearts;

    public void Initialize(int maxHealth)
    {
        if (_hearts != null)
        {
            foreach(var heart in _hearts)
            {
                Destroy(heart);
            }
        }

        _hearts = new GameObject[maxHealth];
        for (int i = 0; i < maxHealth; i++)
        {
            _hearts[i] = Instantiate(HeartPrefab, HeartContainer);
        }
    }

    public void UpdateHealth(int currentHealth)
    {
        for (int i = 0; i < _hearts.Length; i++)
        {
            _hearts[i].SetActive(i < currentHealth);
        }
    }
}