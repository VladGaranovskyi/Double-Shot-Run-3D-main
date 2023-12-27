using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveTrigger : MonoBehaviour
{
    [SerializeField] private Vector3 _direction;

    private void OnTriggerEnter(Collider other)
    {
        CharacterController player;
        if(other.TryGetComponent<CharacterController>(out player))
        {
            player.Move(_direction);
        }
    }
}
