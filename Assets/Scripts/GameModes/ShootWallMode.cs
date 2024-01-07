using System.Collections.Generic;
using UnityEngine;
using States;

namespace GameModes
{
    [SelectionBase]
    public class ShootWallMode : MonoBehaviour, GameMode
    {
        [SerializeField] private List<Transform> _children;
        [Space]
        [SerializeField] private Wall _wall;
        [SerializeField] private State_Type _state;
        [SerializeField] private ParticleSystem[] _transitionParticles;
        [Space]
        [SerializeField] private int reward = 25;
        [SerializeField] private Transform _floor;
        [SerializeField] private AudioSource _winSound;
        [SerializeField] private AudioSource _startSound;
        private PlayerController _playerController;

        public Transform Floor => _floor;

        private void Start()
        {
            _playerController = FindObjectOfType<PlayerController>();
        }

        public State_Type GetModeState() => _state;

        public void InitializeGame()
        {
            _startSound.Play();
            foreach (var part in _transitionParticles) part.Play();
            foreach (Transform child in _children)
            {
                child.gameObject.SetActive(true);
                //child.parent = null;
            }
            for(int i = 0; i < _playerController.playerHealth.Health; i++)
            {
                _wall.ChangeHealth(1f);
            }
        }

        public void EndGame()
        {
            _winSound.Play();
            foreach (var part in _transitionParticles) part.Play();
            foreach (Transform child in _children)
            {
                //child.parent = transform;
                child.gameObject.SetActive(false);
            }
            _playerController.stateMachine.ChangeState(_playerController.runningState);
            _playerController.AddCoins(reward);
        }

        public bool IsGameWon() => _wall.GetHealth() >= 85;

        public bool IsGameLost() => _wall.GetHealth() <= 15;

        public GameObject GetGameObject() => gameObject;
    }
}
