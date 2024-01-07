using UnityEngine;
using UnityEngine.UI;

public class HealthBooster : MonoBehaviour
{
    [SerializeField] private int _defaultHealth;
    [SerializeField] private Text _healthCount;
    [SerializeField] private Text _price;
    private int Health
    {
        get => PlayerPrefs.GetInt("Health");
        set => PlayerPrefs.SetInt("Health", value);
    }

    private int Price
    {
        get => 200 * (Health - 2);
    }

    private PlayerHealth _playerHealth;

    private void RefreshData()
    {
        _playerHealth.SetDefaultHealth(Health);
        _price.text = Price.ToString();
        _healthCount.text = Health.ToString();
    }

    private void Start()
    {
        _playerHealth = FindObjectOfType<PlayerHealth>();
        if(!PlayerPrefs.HasKey("Health")) PlayerPrefs.SetInt("Health", _defaultHealth);
        RefreshData();
    }

    public void AddHealth()
    {
        if (PlayerPrefs.HasKey("Coins") && PlayerPrefs.GetInt("Coins") > Price)
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - Price);
            Health += 1;
            RefreshData();
        }
    }
}
