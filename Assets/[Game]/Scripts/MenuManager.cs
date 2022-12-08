using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager MenuManagerInstance;
    public bool isGameActive;
    public GameObject[] menuElement = new GameObject[2];
    void Start()
    {
        isGameActive = false;
        MenuManagerInstance = this;
    }

    void Update()
    {
        
    }

    public void StartTheGame()
    {
        isGameActive = true;   
        menuElement[0].SetActive(false);
        GameObject.FindWithTag("particle").GetComponent<ParticleSystem>().Play();
    }
}
