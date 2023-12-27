using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int _defaultHealth = 3;
    public int health = 3;

    public void ChangeHealth(float val)
    {
        health -= 1;
    }

    public float GetHealth() => health;

    public void RestoreHealth()
    {
        health = _defaultHealth; 
    }
}
