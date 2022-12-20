using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager MenuManagerInstance;
    public bool isGameActive;
    public GameObject[] menuElement = new GameObject[2];
    void Start() //wait for tap the screen to start the game
    {
        isGameActive = false;
        MenuManagerInstance = this;
    }

    public void StartTheGame()
    {
        isGameActive = true;   
        menuElement[0].SetActive(false);
        GameObject.FindWithTag("particle").GetComponent<ParticleSystem>().Play();
    }
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
