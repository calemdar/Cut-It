using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void StartGame()
    {
        PlayerPrefs.SetInt("GameOver", 0);
        SceneManager.LoadScene("MainScene");
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("GameOver", 0);
    }
}
