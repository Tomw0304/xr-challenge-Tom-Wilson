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
            if (rb.velocity.magnitude < 5f || rb.velocity.magnitude = 100f)
            {
                rb.velocity = transform.forward * 100;
            }
            // Accelerate inbetween the start and reaching a top speed
            else 
            {
                rb.AddForce(transform.forward * 10000);
            }
        }
    }
}
