using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager MenuManagerInstance;
    public bool isGameActive;
    public GameObject menuElement;
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
        menuElement.SetActive(false);
        GameObject.FindWithTag("particle").GetComponent<ParticleSystem>().Play();
    }
}
