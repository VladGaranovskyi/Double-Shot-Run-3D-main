using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class MovingObstacle : MonoBehaviour
{
    [SerializeField] private float _distance;
    [SerializeField] private float _speed;
    private Rigidbody _rb;
    private Vector3 _startPos;
    private float multiplier = 1f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _distance = _distance * _distance;
        _startPos = transform.position;
    }

    private void FixedUpdate()
    {
        if ((transform.position - _startPos).magnitude > _distance)
        {
            multiplier = -multiplier;
        }
        _rb.MovePosition(transform.position + transform.up * Time.deltaTime * _speed * multiplier);
    }
}
