using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private TextMeshPro _hpDisplayer;
    private int _defaultHealth;
    private int _health;
    public int Health { get => _health; }
    public GameObject HealthDisplayer => _hpDisplayer.gameObject;

    public void SetDefaultHealth(int health)
    {
        _defaultHealth = health;
        _health = _defaultHealth;
        _hpDisplayer.text = Health.ToString();
    }

    public void ChangeHealth(float val)
    {
        _health -= 1;
        _hpDisplayer.text = Health.ToString();
    }

    public float GetHealth() => _health;

    public void RestoreHealth()
    {
        _health = _defaultHealth;
        _hpDisplayer.text = Health.ToString();
    }
}
