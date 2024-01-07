using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyBone : MonoBehaviour
{
    [SerializeField] private Enemy_Shooter _enemy;
    [SerializeField] private RagDollController _ragDoll;

    private void OnTriggerEnter(Collider other)
    {
        Bullet b;
        if(other.TryGetComponent<Bullet>(out b))
        {
            if(b.GetBulletTag() == "PlayerBullet")
            {
                _enemy.runNShootGameMode.RewardPlayerForElimination(_enemy.Reward);
                _enemy.enabled = false;
                _ragDoll.DisableBulletCollision(b.GetComponent<Collider>());
                _ragDoll.ApplyForce(b.transform.forward * b.Damage);
                _ragDoll.ChangeRagDollState(true);
                Invoke("ClearBody", 5f);
            }
        }
        else if(other.CompareTag("Prop"))
        {
            _enemy.runNShootGameMode.RewardPlayerForElimination(_enemy.Reward);
            _enemy.enabled = false;
            _ragDoll.ChangeRagDollState(true);
            Invoke("ClearBody", 5f);
        }
    }

    private void ClearBody()
    {
        _ragDoll.Pelvis.gameObject.SetActive(false);
        _enemy.gameObject.SetActive(false);
    }
}
