using UnityEngine;
using System.Collections.Generic;
using System.Linq;

internal class BulletVectors
{
    private float sinOfAngle;
    private float cosOfAngle;

    public BulletVectors(float angle)
    {
        sinOfAngle = Mathf.Sin((Mathf.PI * angle)/ 180f);
        cosOfAngle = Mathf.Cos((Mathf.PI * angle) / 180f);
    }

    internal List<Vector3> GetVectors(int count, Vector3 dir)
    {
        List<Vector3> outputVectors = new List<Vector3>();
        void AddVectorsToOutput(float sinus, float cosinus)
        {
            outputVectors.Add(
                new Vector3(0f, -dir.z * sinus + dir.y * cosinus,
                dir.z * cosinus + dir.y * sinus).normalized
                );
            outputVectors.Add(
                new Vector3(0f, -dir.z * -sinus + dir.y * cosinus,
                dir.z * cosinus + dir.y * -sinus).normalized
                );
        }
        switch (count)
        {
            case 2:
                AddVectorsToOutput(sinOfAngle, cosOfAngle);
                break;
            case 3:
                AddVectorsToOutput(sinOfAngle, cosOfAngle);
                outputVectors.Add(dir);
                break;
            case 4:
                AddVectorsToOutput(sinOfAngle, cosOfAngle);
                AddVectorsToOutput(2 * sinOfAngle * cosOfAngle,
                    Mathf.Pow(cosOfAngle, 2) - Mathf.Pow(sinOfAngle, 2));
                break;
            case 5:
                AddVectorsToOutput(sinOfAngle, cosOfAngle);
                AddVectorsToOutput(2 * sinOfAngle * cosOfAngle,
                    Mathf.Pow(cosOfAngle, 2) - Mathf.Pow(sinOfAngle, 2));
                outputVectors.Add(dir);
                break;
        }
        return outputVectors;
    }
}

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _speed;
    [SerializeField] private LayerMask _layer;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private string _currentTag;
    [SerializeField] private float _firstSplitAngle;
    [SerializeField] private Transform _bulletPrefab;
    [SerializeField] private AudioSource _hitSound;
    private Queue<GameObject> _particles = new Queue<GameObject>();
    private float _startDamage;
    public PlayerController playerController { private get; set; }
    public Enemy enemy { private get; set; }
    public float Damage { get => _damage; set => _damage = value; }
    
    private Vector3 _surfaceNormal;
    private BulletVectors _bulletVectors;
    private IDamageable _damageable;
    public void SetBulletTag(string newTag) => _currentTag = newTag;

    public string GetBulletTag() => _currentTag;
    private List<string> blockedMultipliersNames = new List<string>();

    private void Awake()
    {
        _startDamage = _damage;
    }

    public bool IsMultiplierNotBlocked(string multiplierName)
    {
        if (blockedMultipliersNames.Count == 0) return true;       
        return !blockedMultipliersNames.Contains(multiplierName);
    }

    private void OnEnable()
    {
        if (GMController.instance.currentGameMode != null && (playerController != null || enemy != null))
        {
            ChangeBulletCount(1);
            _bulletVectors = new BulletVectors(_firstSplitAngle);
        }
    }

    private void OnDisable()
    {
        if(GMController.instance.currentGameMode != null && (playerController != null || enemy != null))
        {
            ChangeBulletCount(-1);
        }
        _damage = _startDamage;
        blockedMultipliersNames = new List<string>();
    }

    private void ChangeBulletCount(int c)
    {
        switch (_currentTag)
        {
            case "EnemyBullet":
                enemy.bulletsCount += c;
                break;
            case "PlayerBullet":
                playerController.shootingState.bulletsCount += c;
                break;
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(transform.position + transform.forward * Time.deltaTime * _speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Bullet>() == null)
        {
            if (collision.collider.TryGetComponent<IDamageable>(out _damageable))
            {
                _damageable.ChangeHealth(-_damage);
                ParticleSystem parts = ObjectPool.instance.GetPooledObject<ParticleSystem>("BulletPartsPool");
                parts.transform.position = transform.position;
                parts.gameObject.SetActive(true);
                _particles.Enqueue(parts.gameObject);
                parts.Play();
                Invoke("DequeuePart", parts.main.startLifetime.constant);
                gameObject.SetActive(false);
            }
            else
            {
                _hitSound.Play();
                transform.forward = Vector3.Reflect(transform.forward, collision.contacts[0].normal);
            }
        }
    }

    public void SplitIntoNBullets(int count, string multiplierName)
    {
        List<Vector3> normals = _bulletVectors.GetVectors(count, transform.forward);
        blockedMultipliersNames.Add(multiplierName);
        foreach(var normal in normals)
        {
            Bullet bullet = ObjectPool.instance.GetPooledObject<Bullet>(_currentTag + "Pool");
            bullet.transform.position = transform.position;
            bullet.transform.forward = normal;
            bullet.blockedMultipliersNames = blockedMultipliersNames.Select(e => e).ToList<string>();
            bullet.playerController = playerController;
            bullet.enemy = enemy;
            bullet.Damage = _damage;
            bullet.gameObject.SetActive(true);
        }
        gameObject.SetActive(false);
    }

    public void SpreadIntoNBullets(int count, string multiplierName)
    {
        blockedMultipliersNames.Add(multiplierName);
        for(int i = 1; i < count; i++)
        {
            Bullet bullet = ObjectPool.instance.GetPooledObject<Bullet>(_currentTag + "Pool");
            bullet.transform.position = transform.position;
            bullet.transform.forward = transform.forward;
            bullet.transform.position += transform.right * Random.Range(-1f, 1f);
            bullet.transform.position += transform.up * Random.Range(-1f, 1f);
            bullet.blockedMultipliersNames = blockedMultipliersNames.Select(e => e).ToList<string>();
            bullet.playerController = playerController;
            bullet.enemy = enemy;
            bullet.Damage = _damage;
            bullet.gameObject.SetActive(true);
        }
    }

    private void DequeuePart()
    {
        GameObject part = _particles.Dequeue();
        part.SetActive(false);
    }
}
