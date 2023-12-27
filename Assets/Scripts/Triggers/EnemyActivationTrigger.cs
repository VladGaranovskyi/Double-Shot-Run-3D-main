using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivationTrigger : MonoBehaviour
{
    [SerializeField] private Enemy_Shooter[] _enemies;

    private void OnTriggerEnter(Collider other)
    {
        foreach(Enemy_Shooter enemy in _enemies)
        {
            enemy.ActivateEnemy();
        }
    }
}
