using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class LevelManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private GameObject _menuUI;
    [SerializeField] private GameObject _gameUI;
    [SerializeField] private GameObject _finishUI;
    [SerializeField] private GameObject _looseUI;   
    [SerializeField] private Text _earnedCoinsText;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GameObject _playerDoll;
    [SerializeField] private string _androidAdUnitId = "Interstitial_Android";
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
        LoadAd();
    }

    private void GameEnd()
    {
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + Mathf.RoundToInt(_playerController.Coins * Multiplier));
        if (PlayerPrefs.HasKey("Level"))
        {
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
            if (PlayerPrefs.GetInt("Level") > 2)
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

    private void LoadAd()
    {
        Debug.Log("Loading Ad: " + _androidAdUnitId);
        Advertisement.Load(_androidAdUnitId, this);
    }

    // Show the loaded content in the Ad Unit:
    public void ShowAd()
    {
        // Note that if the ad content wasn't previously loaded, this method will fail
        Debug.Log("Showing Ad: " + _androidAdUnitId);
        Advertisement.Show(_androidAdUnitId, this);
    }

    // Implement Load Listener and Show Listener interface methods: 
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        ShowAd();
    }

    public void OnUnityAdsFailedToLoad(string _adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {_adUnitId} - {error.ToString()} - {message}");
        GameEnd();
    }

    public void OnUnityAdsShowFailure(string _adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {_adUnitId}: {error.ToString()} - {message}");
        GameEnd();
    }

    public void OnUnityAdsShowStart(string _adUnitId) { }
    public void OnUnityAdsShowClick(string _adUnitId) { }
    public void OnUnityAdsShowComplete(string _adUnitId, UnityAdsShowCompletionState showCompletionState) 
    {
        GameEnd();
    }
}
