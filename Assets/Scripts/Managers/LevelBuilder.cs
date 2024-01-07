using GameModes;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] private Transform _start;
    [SerializeField] private GameObject _finish;
    [SerializeField] private GameObject[] _levels; 
    [SerializeField] private float _gap = 7.65f;

    private List<int> LevelsIndexes
    {
        get => JsonConvert.DeserializeObject<List<int>>(PlayerPrefs.GetString("Level Config"));
        set => PlayerPrefs.SetString("Level Config", JsonConvert.SerializeObject(value));
    }

    private void SetNextFloor(GameObject current, GameObject next) 
    {
        next.transform.position = new Vector3(next.transform.position.x, next.transform.position.y,
            current.transform.position.z + (current.GetComponent<GameMode>().Floor.transform.lossyScale.z / 2f) + _gap + (next.GetComponent<GameMode>().Floor.transform.lossyScale.z / 2f));
        current.GetComponentInChildren<ShootTrigger>().NextGameMode = next.transform;
    }

    private void Awake()
    {
        int countOfLevels = Random.Range(2, 4);
        List<int> indexes;
        if(PlayerPrefs.HasKey("Level Config"))
        {
            indexes = LevelsIndexes;
        }
        else
        {
            indexes = new List<int>();
            for (int i = 0; i < countOfLevels; i++)
            {
                int idx = Random.Range(0, _levels.Length);
                if (indexes.Contains<int>(idx))
                {
                    i -= 1;
                    continue;
                }
                else
                {
                    indexes.Add(idx);
                }
            }
            LevelsIndexes = indexes;
        }
        LinkedList<GameObject> _gmList = new LinkedList<GameObject>();
        GameObject gameMode = Instantiate(_levels[indexes[0]]);
        gameMode.transform.position = _start.position;
        gameMode.transform.position = new Vector3(gameMode.transform.position.x, gameMode.transform.position.y,
            _start.position.z + (_start.lossyScale.z / 2f) + _gap + (gameMode.GetComponent<GameMode>().Floor.transform.lossyScale.z / 2f));
        _gmList.AddFirst(gameMode);
        _start.GetComponentInChildren<ShootTrigger>().NextGameMode = gameMode.transform;
        for (int i = 1; i < indexes.Count; i++) 
        {
            GameObject gameModeObject = Instantiate(_levels[indexes[i]]);
            gameModeObject.transform.position = _start.position;
            SetNextFloor(_gmList.Last.Value, gameModeObject);
            _gmList.Last.Value.GetComponentInChildren<ShootTrigger>().NextGameMode = gameModeObject.transform;
            _gmList.AddLast(gameModeObject);
        }
        SetNextFloor(_gmList.Last.Value, _finish);
        _gmList.Last.Value.GetComponentInChildren<ShootTrigger>().NextGameMode = _finish.transform;
    }
}
