using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Create variable to store the rigidbody of the player
    private Rigidbody rb;

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
    }
}
