using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.HasKey("Level"))
        {
            if(PlayerPrefs.GetInt("Level") < 3) 
            {
                SceneManager.LoadSceneAsync($"Level {PlayerPrefs.GetInt("Level")}");
            }
            else
            {
                SceneManager.LoadSceneAsync("Level N");
            }
        }
        else
        {
            SceneManager.LoadSceneAsync("Level 1");
        }
    }
}
