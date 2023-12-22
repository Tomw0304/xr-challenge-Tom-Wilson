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
            // Use velovity at lower speed to create an instant start and at the max speed to cap the speed
            if (rb.velocity.magnitude < 1f)
            {
                rb.velocity = transform.forward;
            }
            // Accelerate inbetween the start and reaching a top speed
            else if (rb.velocity.magnitude < 100f)
            {
                rb.AddForce(transform.forward * 100);
            }
        }

        // Move backward
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.forward * 1000);
        }

        // Turns left
        if (Input.GetKeyDown(KeyCode.A) && !rotatingLeft)
        {
            rotatingLeft = true;
            StartCoroutine(RotatePlayer(Vector3.up * -45f, 0.1f, () => rotatingLeft = false));
        }

        // Turns right
        if (Input.GetKeyDown(KeyCode.D) && !rotatingRight)
        {
            rotatingRight = true;
            StartCoroutine(RotatePlayer(Vector3.up * 45f, 0.1f, () => rotatingRight = false));
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

            // Waits for the next frame
            yield return null;
        }

        // Notifies the code that the rotation is complete
        if (onComplete != null)
        {
            onComplete.Invoke();
        }
    }
}
