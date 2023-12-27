using DG.Tweening;
using GameModes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shooter : Enemy
{
    [SerializeField] private float _maxSpread = 15f;
    [SerializeField] private int _reward = 10;
    [SerializeField] private RunNShootGameMode _gm;
    public int Reward { get => _reward; }
    public RunNShootGameMode runNShootGameMode { get => _gm; }

    private bool isActive;

    public void ActivateEnemy() { isActive = true; }

    protected override void SetRandomRotation()
    {
        Vector3 playerPos = _gm.GetPlayerPos();
        Vector3 dir = Vector3.Normalize(playerPos - _spine.position);
        _spine.right = dir;
        _spine.Rotate(Vector3.one * Random.Range(0f, _maxSpread));
    }

    private void OnEnable()
    {   
        _enableTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - _enableTime > 1f && IsNotShot && isActive)
        {
            StartCoroutine(ShootCor());
        }
    }
}
