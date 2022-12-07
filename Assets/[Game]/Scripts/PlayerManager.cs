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
    

    void Start()
    {
        ball = transform;
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        ballRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && MenuManager.MenuManagerInstance.GameState)
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

        if (MenuManager.MenuManagerInstance.GameState)
        {
            var pathNewPos = path.position;
            path.position = new Vector3(pathNewPos.x, pathNewPos.y, Mathf.MoveTowards(pathNewPos.z, -1000f, pathSpeed * Time.deltaTime));
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
            MenuManager.MenuManagerInstance.GameState = false;
        }

        switch (other.tag)
        {
            case "red":
            other.gameObject.SetActive(false);
            ballRenderer.material = other.GetComponent<Renderer>().material;
            var NewParticleRed = Instantiate(collideParticle, transform.position, Quaternion.identity);
            NewParticleRed.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
            break;
            case "green":
            other.gameObject.SetActive(false);
            ballRenderer.material = other.GetComponent<Renderer>().material;
            var NewParticleGreen = Instantiate(collideParticle, transform.position, Quaternion.identity);
            NewParticleGreen.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
            break;
            case "blue":
            other.gameObject.SetActive(false);
            ballRenderer.material = other.GetComponent<Renderer>().material;
            var NewParticleBlue = Instantiate(collideParticle, transform.position, Quaternion.identity);
            NewParticleBlue.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
            break;
            case "yellow":
            other.gameObject.SetActive(false);
            ballRenderer.material = other.GetComponent<Renderer>().material;
            var NewParticleYellow = Instantiate(collideParticle, transform.position, Quaternion.identity);
            NewParticleYellow.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
            break;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("path"))
        {
            rb.isKinematic = false;
            coll.isTrigger = false;
            rb.velocity = new Vector3(0f, 8f, 0f);
            pathSpeed = pathSpeed *2 ;

        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("path"))
        {
            rb.isKinematic = true;
            coll.isTrigger = true;
            pathSpeed = pathSpeed / 2;
        }
    }
}
