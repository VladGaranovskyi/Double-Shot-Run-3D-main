using UnityEngine;
using UnityEngine.UI;

public class MenuCoinsDisplayer : MonoBehaviour
{
    private Text _coinsText;
    private int Coins
    {
        get
        {
            if (PlayerPrefs.HasKey("Coins"))
            {
                return PlayerPrefs.GetInt("Coins");
            }
            else
            {
                return 0;
            }
        }
    }

    private void Start()
    {
        _coinsText = GetComponent<Text>();
        _coinsText.text = Coins.ToString();
    }
}
