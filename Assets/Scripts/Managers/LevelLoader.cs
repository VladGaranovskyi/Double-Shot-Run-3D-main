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
                SceneManager.LoadScene($"Level {PlayerPrefs.GetInt("Level")}");
            }
            else
            {
                SceneManager.LoadScene("Level N");
            }
        }
        else
        {
            SceneManager.LoadScene("Level 1");
        }
    }
}
