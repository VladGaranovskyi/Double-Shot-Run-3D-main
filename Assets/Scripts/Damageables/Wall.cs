using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Wall : MonoBehaviour, IDamageable
{
    [SerializeField] private Transform _enemy;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _damageMultiplier = 1f;
    private Transform _player;
    private float health = 50f;
    private float StepDivider;
    private float _step;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>().transform;
    }

    private void OnEnable()
    {
        StepDivider = health * 2f;
    }

    private void Update()
    {
        _step = Mathf.Abs(_player.position.z - _enemy.position.z) / StepDivider;
    }

    public void ChangeHealth(float val)
    {
        float i = val > 0 ? 1f : -1f;
        health += i * _damageMultiplier;
        transform.position += Vector3.forward * i * _step;
    }

    public float GetHealth() => health;
}
