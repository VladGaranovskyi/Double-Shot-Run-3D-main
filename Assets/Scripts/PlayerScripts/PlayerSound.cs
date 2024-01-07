using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private AudioSource _run;
    [SerializeField] private AudioSource _death;
    [SerializeField] private AudioSource _fall;
    [SerializeField] private AudioSource _jump;
    [SerializeField] private AudioSource _shootSound;

    public void PlayRun() => _run.Play();
    public void PlayFall() => _fall.Play();
    public void PlayDeath() => _death.Play();
    public void PlayJump() => _jump.Play();
    public void PlayShoot() => _shootSound.Play();
    public void StopRun() => _run.Stop();
}
