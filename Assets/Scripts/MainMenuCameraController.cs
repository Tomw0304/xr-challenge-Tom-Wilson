using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraController : MonoBehaviour
{
    // Radius variable
    private float r = 2f;

    // Angle variable
    private float angle = 0f;

    // Variable to store the player transfom
    public Transform playerTransform;

    // Update is called once per frame
    void Update()
    {
        // Change the angle using the speed and the time
        angle += 0.2f * Time.deltaTime;

        // Calculate the new x and z coordinates
        float x = Mathf.Cos(angle) * r;
        float z = Mathf.Sin(angle) * r;

        // Changes the position using the new coordinates
        transform.position = new Vector3(x, transform.position.y, z);

        // Make the camera face the player
        transform.LookAt(playerTransform);
    }
}
