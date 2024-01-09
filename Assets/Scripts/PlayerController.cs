using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Create variable to store the rigidbody of the player
    private Rigidbody rb;

    // Initialise two boolean variables to store whether the player is rotating left or right
    private bool rotatingLeft = false;
    private bool rotatingRight = false;
    private bool rotatingWall = false;

    // Initialise two variables to store the time since the start of the rotation left or right
    private float rotatingTimeLeft = 0.0f;
    private float rotatingTimeRight = 0.0f;

    // Stores whether the player is facing forward (any of the multiple 90) or facing left or right (-45 or 45 degrees from a mulitple of 90 respectively)
    // The key: forward (side = 0), left (side = -1), right (side = 1)
    private int side;

    // Points UI variable
    public TextMeshProUGUI pointsText;

    // Points variable
    private int points;

    // Start is called before the first frame update
    void Start()
    {
        // Initialise the rigidbody variable
        rb = GetComponent<Rigidbody>();

        // Initialise the side to be the forward value
        side = 0;

        // Intialise the points
        points = 0;

        // Intialise the points text
        pointsText.text = "Points: " + points.ToString();
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

        // locks the side to side controls when traversing a wall
        if (!rotatingWall)
        {
            // Turns left
            if (Input.GetKeyDown(KeyCode.A) && !rotatingLeft && !rotatingRight)
            {
                rotatingLeft = true;
                StartCoroutine(RotatePlayer(transform.up * -45f, 0.1f, () => rotatingLeft = false));
                if (side == -1 || side == 1)
                {
                    side = 0;
                }
                else
                {
                    side = -1;
                }
            }

            // Turns right
            if (Input.GetKeyDown(KeyCode.D) && !rotatingRight && !rotatingLeft)
            {
                rotatingRight = true;
                StartCoroutine(RotatePlayer(transform.up * 45f, 0.1f, () => rotatingRight = false));
                if (side == -1 || side == 1)
                {
                    side = 0;
                }
                else
                {
                    side = 1;
                }
            }
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

    // When entering the trigger collider of the wall
    private void OnTriggerEnter(Collider other)
    {
        // Checks if the collided object is has the floor
        if (other.CompareTag("Floor"))
        {
            // set the boolean storing whether the function is running
            rotatingWall = true;

            // round the rotation to the nearest 45
            transform.rotation = Quaternion.Euler(RoundToNearest45(transform.rotation.eulerAngles.x), RoundToNearest45(transform.rotation.eulerAngles.y), RoundToNearest45(transform.rotation.eulerAngles.z));

            // Check if either is close to 45 degrees (within a threshold)
            if (side == -1)
            {
                StartCoroutine(RotatePlayer(transform.up * 45f, 0.01f, () => StartCoroutine(RotatePlayer(transform.right * -90f, 0.01f, () => StartCoroutine(RotatePlayer(transform.up * -45f, 0.01f, () => side = 1))))));
            }
            else if (side == 1)
            {
                StartCoroutine(RotatePlayer(transform.up * -45f, 0.01f, () => StartCoroutine(RotatePlayer(transform.right * -90f, 0.01f, () => StartCoroutine(RotatePlayer(transform.up * 45f, 0.01f, () => side = -1))))));
            }
            else if (side == 0)
            {
                StartCoroutine(RotatePlayer(transform.right * -90f, 0.1f));
            }

            // run the corountine to reset the rotatingwall bool
            StartCoroutine(ResetRotatingWall());
        } 
        else if (other.CompareTag("Pickup"))
        {
            // Incremenet the points
            points++;

            // Updates the points UI text
            pointsText.text = "Points: " + points.ToString();
        }
    }

    // function to reset the rotatingwall bool
    private IEnumerator ResetRotatingWall()
    {
        // waits 0.1 seconds before reseting the rotatingWall bool
        yield return new WaitForSeconds(0.1f);
        rotatingWall = false;
    }

    // rounds angle to nearest multiple of 45 degrees
    float RoundToNearest45(float angle)
    {
        return Mathf.Round(angle / 45f) * 45f;
    }
}
