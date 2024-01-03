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

    // Updates every frame
    void Update()
    {
        // Move forward
        if (Input.GetKey(KeyCode.W))
        {
            if ((Vector3.Dot(rb.velocity.normalized, transform.forward.normalized) > 0.5f) && rb.velocity.magnitude < 100f)
            {
                rb.AddForce(transform.forward * 100);
            } else if (rb.velocity.magnitude == 0)
            {
                rb.velocity = transform.forward * 10;
            }
        }

        // Move backward
        if (Input.GetKey(KeyCode.S))
        {
            if (rb.velocity.magnitude == 0 || Vector3.Dot(rb.velocity.normalized, transform.forward.normalized) > 0.5f)
            {
                rb.AddForce(-transform.forward);
            }
            else if ((Vector3.Dot(rb.velocity.normalized, -transform.forward.normalized) > 0.5f) && rb.velocity.magnitude < 10f)
            {
                rb.AddForce(-transform.forward * 10);
            }
        }

        // Turns left
        if (Input.GetKeyDown(KeyCode.A) && !rotatingLeft)
        {
            rotatingLeft = true;
            StartCoroutine(RotatePlayer(transform.up * -45f, 0.1f, () => rotatingLeft = false));
        }

        // Turns right
        if (Input.GetKeyDown(KeyCode.D) && !rotatingRight)
        {
            rotatingRight = true;
            StartCoroutine(RotatePlayer(transform.up * 45f, 0.1f, () => rotatingRight = false));
        }

        // If the players moving drag is applied 
        if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) && rb.velocity.magnitude > 0)
        {
            rb.AddForce(-rb.velocity.normalized);
        }
    }

    // Rotation function
    IEnumerator RotatePlayer(Vector3 rotationAmount, float duration, System.Action onComplete = null)
    {
        // Intialises the elapsed time
        float elapsed = 0f;

        // Initialises the initial rotation
        Quaternion initialRotation = transform.rotation;

        // Calculate the rotation after the the rotationAmount is applied
        Quaternion targetRotation = Quaternion.Euler(rotationAmount) * initialRotation;

        // Loops until the time reaches the duration
        while (elapsed < duration)
        {
            // Increment the time
            elapsed += Time.deltaTime;

            // Calculate the interpolation factor based on the ratio of time elapsed and the total duration
            float t = Mathf.Clamp01(elapsed / duration);

            // Rotate the player to the target rotation
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);

            // Transfer velocity into the new direction
            if (Vector3.Dot(rb.velocity.normalized, transform.forward.normalized) > 0.5f)
            {
                rb.velocity = transform.forward * rb.velocity.magnitude;
            }
            else
            {
                rb.velocity = -transform.forward * rb.velocity.magnitude;
            }

            // Waits for the next frame
            yield return null;
        }

        // Notifies the code that the rotation is complete
        if (onComplete != null)
        {
            onComplete.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(RotatePlayer(transform.right * -90f, 0.1f));
    }



}
