using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Create variable to store the rigidbody of the player
    private Rigidbody rb;

    // Initialise two boolean variables to store whether the player is rotated left or right
    private bool rotatingLeft = false;
    private bool rotatingRight = false;

    // Initialise two variables to store the time since the start of the rotation left or right
    private float rotatingTimeLeft = 0.0f;
    private float rotatingTimeRight = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //initialise the rigidbody variable
        rb = GetComponent<Rigidbody>();
    }

    // Update is at 50 times per second
    void FixedUpdate()
    {
        // Move forward
        if (Input.GetKey(KeyCode.W))
        {
            // Use velovity at lower speed to create an instant start and at the max speed to cap the speed
            if (rb.velocity.magnitude < 1f)
            {
                rb.velocity = transform.forward * 10;
            }
            // Accelerate inbetween the start and reaching a top speed
            else if (rb.velocity.magnitude < 100f)
            {
                rb.AddForce(transform.forward * 1000);
            }
        }

        // Move backward
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.forward * 1000);
        }

        // Turns left
        if (Input.GetKey(KeyCode.A))
        {
            rotatingLeft = true;
        }


        // Resets the rotation variable when the rotation is complete
        if (rotatingTimeLeft >= 0.1f)
        {
            rotatingLeft = false;
            rotatingTimeLeft = 0.0f;
        }

        // Rotating the player 45 degrees to the left for a specific time
        if (rotatingLeft && rotatingTimeLeft < 0.1f)
        {
            // Increment the rotatingTime
            rotatingTimeLeft += Time.deltaTime;

            // Calculate the interpolation factor based on the fraction of the rotating time elapsed
            float t = Mathf.Clamp01(rotatingTimeLeft / 0.1f);

            // Initalise the current rotation to the player's current rotation
            Vector3 currentRotation = transform.rotation.eulerAngles;

            // Minus 45 degrees to the Y axis rotation
            currentRotation.y -= 45;

            // Create euler rotation with transformed angle
            Quaternion targetRotation = Quaternion.Euler(currentRotation);

            // Rotate the player to the targetRotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, t);        
        }

        // Turns right
        if (Input.GetKey(KeyCode.D))
        {
            rotatingRight = true;
        }


        // Resets the rotation variable when the rotation is complete
        if (rotatingTimeRight >= 0.1f)
        {
            rotatingRight = false;
            rotatingTimeRight = 0.0f;
        }

        // Rotating the player 45 degrees to the right for a specific time
        if (rotatingRight && rotatingTimeRight < 0.1f)
        {
            // Increment the rotatingTime
            rotatingTimeRight += Time.deltaTime;

            // Calculate the interpolation factor based on the fraction of the rotating time elapsed
            float t = Mathf.Clamp01(rotatingTimeRight / 0.1f);

            // Initalise the current rotation to the player's current rotation
            Vector3 currentRotation = transform.rotation.eulerAngles;

            // Add 45 degrees to the Y axis rotation
            currentRotation.y += 45;

            // Create euler rotation with transformed angle
            Quaternion targetRotation = Quaternion.Euler(currentRotation);

            // Rotate the player to the targetRotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, t);
        }
    }
}
