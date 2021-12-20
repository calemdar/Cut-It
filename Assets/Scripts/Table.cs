using UnityEngine;
using UnityEngine.SceneManagement;

public class Table : MonoBehaviour
{
    public GameObject cuttableObjectsContainer;
    public float speed;
    [SerializeField] private Knife knife;

    private void OnEnable()
    {
        EventManager.OnGameOver += EndGame;
    }

    private void OnDisable()
    {
        EventManager.OnGameOver -= EndGame;
    }

    private void Update()
    {
        if(cuttableObjectsContainer.transform.childCount <= 1 && cuttableObjectsContainer.transform.GetChild(0).childCount == 0)
        {
            EventManager.GameOver();
        }

        if(knife.state == KnifeState.UP)
            MoveObjects();
    }

    private void MoveObjects()
    {
        Vector3 moveVector = Vector3.left * speed * Time.deltaTime;
        cuttableObjectsContainer.transform.position += moveVector;
    }

    private void EndGame()
    {
        PlayerPrefs.SetInt("GameOver", 1);
        SceneManager.LoadScene("StartScene");
    }
}
