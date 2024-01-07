using States;
using UnityEngine;

namespace GameModes
{
    public class RunNShootGameMode : MonoBehaviour, GameMode
    {
        [SerializeField] private GameObject[] _enemies;
        [SerializeField] private Transform _destination;
        [SerializeField] private Transform _floor;
        private PlayerController _playerController;

        public Transform Floor => _floor;

        private void Start()
        {
            _playerController = FindObjectOfType<PlayerController>();
        }

        public void EndGame()
        {
            foreach (GameObject enemy in _enemies)
            {
                if(enemy != null)
                {
                    enemy.SetActive(false);
                }
            }
            _playerController.stateMachine.ChangeState(_playerController.runningState);
            _playerController.playerAnimator.SwitchToNormalLayer();
            _playerController.stateDrivenCameraAnimator.Play("Default");
            _playerController.playerHealth.HealthDisplayer.SetActive(false);
        }

        public void RewardPlayerForElimination(int reward)
        {
            _playerController.AddCoins(reward);
        }

        public Vector3 GetPlayerPos() => _playerController.transform.position;

        public GameObject GetGameObject() => gameObject;

        public State_Type GetModeState() => State_Type.RunNScopingState;

        public void InitializeGame()
        {
            foreach (GameObject enemy in _enemies)
            {
                enemy.SetActive(true);
            }
            _playerController.playerHealth.RestoreHealth();
            _playerController.playerHealth.HealthDisplayer.SetActive(true);
        }

        public bool IsGameLost() => _playerController.playerHealth.Health <= 0;

        public bool IsGameWon() => _playerController.transform.position.z > _destination.position.z;
    }
}
