using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float shootForce = 10f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float maxDistance = 10f;

    [SerializeField] private ARSessionOrigin arSessionOrigin;

    private Camera arCamera;
    private List<GameObject> activeBalls = new List<GameObject>();

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        arCamera = arSessionOrigin != null ? arSessionOrigin.camera : Camera.main;
    }

    void Update()
    {

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            ShootBall();
        }

  
        CheckBallsDistance();
    }

    void ShootBall()
    {
        if (ballPrefab == null)
        {
            Debug.LogError("no hay prefab de la bola");
            return;
        }

        Vector3 spawnPosition = cameraTransform.position;
        Quaternion rotation = Quaternion.Euler(15, 40, 90);
        GameObject newBall = Instantiate(ballPrefab, spawnPosition, rotation);


        Rigidbody ballRigidbody = newBall.GetComponent<Rigidbody>();
        if (ballRigidbody != null)
        {
            ballRigidbody.velocity = cameraTransform.forward * shootForce;
            activeBalls.Add(newBall);
        }
        else
        {
            Debug.LogWarning("No existe rigidbody");
            Destroy(newBall);
        }
    }

    void CheckBallsDistance()
    {
        List<GameObject> ballsToRemove = new List<GameObject>();

        foreach (GameObject ball in activeBalls)
        {
            if (ball == null)
            {
                ballsToRemove.Add(ball);
                continue;
            }

            float distance = Vector3.Distance(cameraTransform.position, ball.transform.position);
            if (distance > maxDistance)
            {
                ballsToRemove.Add(ball);
                Destroy(ball);
            }
        }

        foreach (GameObject ball in ballsToRemove)
        {
            activeBalls.Remove(ball);
        }
    }
}