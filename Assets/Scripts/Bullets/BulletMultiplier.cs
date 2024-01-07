using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMultiplier : MonoBehaviour
{
    [SerializeField] private int n;
    private Bullet _currentBullet;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Bullet>(out _currentBullet))
        {
            if(_currentBullet.IsMultiplierNotBlocked(name)) 
                _currentBullet.SplitIntoNBullets(n, name);
        }
    }
}
