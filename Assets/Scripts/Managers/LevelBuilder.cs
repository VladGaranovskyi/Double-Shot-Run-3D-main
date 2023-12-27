using GameModes;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] private Transform _start;
    [SerializeField] private GameObject _finish;
    [SerializeField] private GameObject[] _levels; 
    [SerializeField] private float _gap = 7.65f;

    private void SetNextFloor(GameObject current, GameObject next) 
    {
        next.transform.position = new Vector3(next.transform.position.x, next.transform.position.y,
            current.transform.position.z + (current.GetComponent<GameMode>().Floor.transform.lossyScale.z / 2f) + _gap + (next.GetComponent<GameMode>().Floor.transform.lossyScale.z / 2f));
    }

    private void Start()
    {
        int countOfLevels = Random.Range(2, 4);
        int[] indexes = new int[countOfLevels];
        for(int i = 0; i  < countOfLevels; i++)
        {
            int idx = Random.Range(0, _levels.Length);
            if (indexes.Contains<int>(idx)){
                i -= 1;
                continue;
            }
            else
            {
                indexes[i] = idx;
            }
        }
        GameObject gameMode = Instantiate(_levels[indexes[0]]);
        gameMode.transform.position = _start.position;
        gameMode.transform.position = new Vector3(gameMode.transform.position.x, gameMode.transform.position.y,
            _start.position.z + (_start.lossyScale.z / 2f) + _gap + (gameMode.GetComponent<GameMode>().Floor.transform.lossyScale.z / 2f));
        for(int i = 1; i < countOfLevels; i++) 
        {
            GameObject gameModeObject = Instantiate(_levels[indexes[i]]);
            gameMode.transform.position = _start.position;
            SetNextFloor(_levels[indexes[i - 1]], gameModeObject);
        }
        SetNextFloor(_levels[indexes.Last<int>()], _finish);
        _start.GetComponentInChildren<ShootTrigger>().NextGameMode = gameMode.transform;
        for (int i = 0; i <= countOfLevels; i++) 
        {
            // forgot to instantiate, need to instantiate and create list of created objects
            _levels[indexes[i]].GetComponentInChildren<ShootTrigger>().NextGameMode = _levels[indexes[i+1]].transform;
        }
    }
}
