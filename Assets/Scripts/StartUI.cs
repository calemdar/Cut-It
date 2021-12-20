using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    public Text gameTitleText;
    public Text madeByText;
    void Start()
    {
        bool isGameOver = PlayerPrefs.GetInt("GameOver") == 1 ? true : false;
        if (isGameOver)
        {
            gameTitleText.text = "Game Over!";
            madeByText.text = "Try again?";
        }
    }
}
