using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject _menuUI;
    [SerializeField] private GameObject _gameUI;
    [SerializeField] private GameObject _finishUI;
    [SerializeField] private GameObject _looseUI;   
    [SerializeField] private Text _earnedCoinsText;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GameObject _playerDoll;
    public static LevelManager instance;

    public float Multiplier { private get; set; }

    private void StartGame()
    {
        _menuUI.SetActive(false);
        _gameUI.SetActive(true);
        _playerController.stateMachine.ChangeState(_playerController.runningState);
        Holder.instance.OnTouch -= StartGame;
    }

    private void Start()
    {
        Holder.instance.OnTouch += StartGame;
        instance = this;
    }

    public void ShowEndUI()
    {
        _gameUI.SetActive(false);
        _finishUI.SetActive(true);
        _earnedCoinsText.text = Mathf.RoundToInt(_playerController.Coins * Multiplier).ToString();
    }

    public void ShowLooseUI()
    {
        _gameUI.SetActive(false);
        _looseUI.SetActive(true);
        _playerDoll.transform.position = _playerController.transform.position;
        _playerController.gameObject.SetActive(false);
        _playerDoll.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EndGame()
    {
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + Mathf.RoundToInt(_playerController.Coins * Multiplier));
        if (PlayerPrefs.HasKey("Level"))
        {
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
            if(PlayerPrefs.GetInt("Level") > 2)
            {
                PlayerPrefs.DeleteKey("Level Config");
            }
        }
        else
        {
            PlayerPrefs.SetInt("Level", 2);
        }
        SceneManager.LoadScene("Loading");
    }
}
