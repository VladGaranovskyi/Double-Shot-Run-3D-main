using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LevelDisplayer : MonoBehaviour
{
    private Text _text;
    private int Level
    {
        get => PlayerPrefs.HasKey("Level") ? PlayerPrefs.GetInt("Level") : 1;
    }

    private void Start()
    {
        _text = GetComponent<Text>();
        _text.text = "Level " + Level.ToString();
    }
}
