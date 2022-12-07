using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Transform ball;
    private Vector3 startMousePos, startBallPos;
    private bool moveTheBall;
    private float maxSpeed;

    void Start()
    {
        ball = transform;
        maxSpeed = 0.5f;
    }

    void Update()
    {
        if (Input.GetmouseButtonDown(0))
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
    }
}
