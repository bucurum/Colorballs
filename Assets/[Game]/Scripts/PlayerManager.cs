using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Transform ball;
    private Vector3 startMousePos, startBallPos;
    private bool moveTheBall;
    [Range(0f, 1f)] [SerializeField] float maxSpeed;
    [Range(0f, 1f)] [SerializeField] float camSpeed;
    [Range(0f, 60f)] [SerializeField] float pathSpeed;
    private float velocity;
    private float camVelocityX;
    private float camVelocityY;
    private Camera mainCamera;
    public Transform path; 
    private Rigidbody rb;
    private Collider coll;
    private Renderer ballRenderer;
    public ParticleSystem collideParticle;
    public ParticleSystem airEffect;
    public ParticleSystem dustEffect;
    public ParticleSystem ballTrail;
    public Material[] ballMaterials = new Material[2];
    

    void Start()
    {
        ball = transform;
        ballTrail.Play();
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        ballRenderer = ball.GetChild(1).GetComponent<Renderer>();
        PlayerPrefs.SetInt("score", 0); //every time game starts set the score to 0
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && MenuManager.MenuManagerInstance.isGameActive)
        {
            moveTheBall = true;

            Plane newPlan = new Plane(Vector3.up, 0f);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (newPlan.Raycast(ray, out var distance))
            {
                startMousePos= ray.GetPoint(distance);
                startBallPos = ball.position;
            }
        }else if (Input.GetMouseButtonUp(0))
        {
            moveTheBall = false;
        }
        if (moveTheBall)
        {
            Plane newPlan = new Plane(Vector3.up, 0f);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (newPlan.Raycast(ray, out var distance))
            {
                Vector3 mouseNewPos = ray.GetPoint(distance);
                Vector3 MouseNewPos = mouseNewPos - startMousePos;
                Vector3 DesireBallPos = MouseNewPos + startBallPos;

                DesireBallPos.x = Mathf.Clamp(DesireBallPos.x, -1.5f, 1.5f);

                ball.position = new Vector3(Mathf.SmoothDamp(ball.position.x, DesireBallPos.x, ref velocity, maxSpeed),
                ball.position.y, ball.position.z);
            }
        }

        if (MenuManager.MenuManagerInstance.isGameActive)
        {
            var pathNewPos = path.position;
            path.position = new Vector3(pathNewPos.x, pathNewPos.y, Mathf.MoveTowards(pathNewPos.z, -1000f, pathSpeed * Time.deltaTime));

            ball.GetChild(1).Rotate(Vector3.right * 700f * Time.deltaTime);
        }

    }

    void LateUpdate()
    {
        if (rb.isKinematic)
        {
            var CameraNewPos = mainCamera.transform.position;
            mainCamera.transform.position = new Vector3(Mathf.SmoothDamp(CameraNewPos.x, ball.transform.position.x, ref camVelocityX, camSpeed)
            , Mathf.SmoothDamp(CameraNewPos.y, ball.transform.position.y + 3f, ref camVelocityY, camSpeed), CameraNewPos.z);
  
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("obstacle"))
        {
            gameObject.SetActive(false);
            MenuManager.MenuManagerInstance.isGameActive = false;
            MenuManager.MenuManagerInstance.menuElement[2].SetActive(true);
            MenuManager.MenuManagerInstance.menuElement[2].transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Game Over";
            MenuManager.MenuManagerInstance.menuElement[2].transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().text = PlayerPrefs.GetInt("score").ToString();
        }
        //when player ball collide with other ball, player`s and player`s trail color will be change that which ball`s color hitted
        switch (other.tag)
        {
            case "red":
            other.gameObject.SetActive(false);
            ballMaterials[1] = other.GetComponent<Renderer>().material;
            ballRenderer.material = ballMaterials[1];
            var NewParticleRed = Instantiate(collideParticle, transform.position, Quaternion.identity);
            NewParticleRed.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
            var ballTrailColorRed = this.ballTrail.trails;
            ballTrailColorRed.colorOverLifetime = other.GetComponent<Renderer>().material.color;
            break;
            case "green":
            other.gameObject.SetActive(false);
            ballMaterials[1] = other.GetComponent<Renderer>().material;
            ballRenderer.material = ballMaterials[1];
            var NewParticleGreen = Instantiate(collideParticle, transform.position, Quaternion.identity);
            NewParticleGreen.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
            var ballTrailColorGreen = this.ballTrail.trails;
            ballTrailColorGreen.colorOverLifetime = other.GetComponent<Renderer>().material.color;
            break;
            case "blue":
            other.gameObject.SetActive(false);
            ballMaterials[1] = other.GetComponent<Renderer>().material;
            ballRenderer.material = ballMaterials[1];
            var NewParticleBlue = Instantiate(collideParticle, transform.position, Quaternion.identity);
            var ballTrailColorBlue = this.ballTrail.trails;
            ballTrailColorBlue.colorOverLifetime = other.GetComponent<Renderer>().material.color;
            break;
            case "yellow":
            other.gameObject.SetActive(false);
            ballMaterials[1] = other.GetComponent<Renderer>().material;
            ballRenderer.material = ballMaterials[1];
            var NewParticleYellow = Instantiate(collideParticle, transform.position, Quaternion.identity);
            NewParticleYellow.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
            var ballTrailColorYellow = this.ballTrail.trails;
            ballTrailColorYellow.colorOverLifetime = other.GetComponent<Renderer>().material.color;
            break;
        }
        // when player ball collide with other ball increase the score 1 and display the score.
        if (other.gameObject.name.Contains("ColorBall"))
        {
            PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + 1);
            MenuManager.MenuManagerInstance.menuElement[1].GetComponent<UnityEngine.UI.Text>().text = PlayerPrefs.GetInt("score")
            .ToString();
        }
    }
    void OnTriggerExit(Collider other) //when the player finished a plane it will jump and play an animations and change the variables
    {
        if (other.CompareTag("path"))
        {
            rb.isKinematic = false;
            coll.isTrigger = false;
            rb.velocity = new Vector3(0f, 8f, 0f);
            pathSpeed = pathSpeed *2 ;

            var airEffectMain = airEffect.main;
            airEffectMain.simulationSpeed = 10f;
            ballTrail.Stop();
        }
    }
    void OnCollisionEnter(Collision other) // when the player land a plane set variables to normal
    {
        if (other.collider.CompareTag("path"))
        {
            rb.isKinematic = true;
            coll.isTrigger = true;
            pathSpeed = pathSpeed / 2;

            var airEffectMain = airEffect.main;
            airEffectMain.simulationSpeed = 4f;

            dustEffect.transform.position = other.contacts[0].point + new Vector3(0f, 0.3f, 0f);
            dustEffect.GetComponent<Renderer>().material = ballRenderer.material;
            dustEffect.Play();
            ballTrail.Play();
        }
    }
}
