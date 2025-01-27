using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Create variable to store the rigidbody of the player
    private Rigidbody rb;

    // Create and initialise boolean variables to store whether the player is rotating left or right or up the wall
    private bool rotatingLeft = false;
    private bool rotatingRight = false;
    private bool rotatingWall = false;

    // Stores whether the player is facing forward (any of the multiple 90) or facing left or right (-45 or 45 degrees from a mulitple of 90 respectively)
    // The key: forward (side = 0), left (side = -1), right (side = 1)
    private int side;

    // Points text variable
    public TextMeshProUGUI pointsText;

    // Timer text variable
    public TextMeshProUGUI timerText;

    // Final time text variable
    public TextMeshProUGUI finalTimeText;

    // Quickest time text variable
    public TextMeshProUGUI quickestTimeText;

    // Points variable
    private int points;

    // Particle system for the spotlight 
    public ParticleSystem lightParticles;

    // Spotlight variable
    public Light spotlight;

    // Point Light variable
    public Light pointLight;

    // Stores whether the game is won and initialise it as false
    public static bool won = false;

    // Variables to store the game UI and the winning UI
    public GameObject gameUI;
    public GameObject winningUI;

    // Stores the time the scene starts
    private float sceneStartTime;

    // Variable for the audio source
    public AudioSource audioSourceEngine;
    public AudioSource audioSourcePoints;

    // AudioClips
    public AudioClip oneX;
    public AudioClip twoX;
    public AudioClip quickestSound;

    // stores the last state for the audio clip level
    private int lastState = 0;

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
        pointsText.text = "Stars: " + points.ToString();

        // Spotlight initialises as zero
        spotlight.intensity = 0;

        // Point light is initialises as 5
        pointLight.intensity = 5;

        // Reset the sceneStartTime
        sceneStartTime = Time.time;

        // Initialise pitch
        audioSourceEngine.pitch = 1f;
    }

    // Updates every frame
    void Update()
    {
        // Lets the player move if they haven't won
        if (!won)
        {
            // Update the timer
            timerText.text = (Mathf.Round((Time.time - sceneStartTime) * 100f) / 100f).ToString();

            // Move forward
            if (Input.GetKey(KeyCode.W))
            {
                // Accelerate if the speed is less 100 and above 0.5 else add velocity and moving forward
                if (rb.velocity.magnitude >= 100f)
                {
                    rb.velocity = transform.forward * 100;
                }
                else if ((Vector3.Dot(rb.velocity.normalized, transform.forward.normalized) > 0.5f))
                {
                    rb.AddForce(transform.forward * 100);
                }
                else 
                {
                    rb.velocity = transform.forward * 10;
                }
            }

            // Move backward
            if (Input.GetKey(KeyCode.S))
            {
                // Accelerate if the speed is less 100 and above 0.5 else add velocity and moving backward
                if ((Vector3.Dot(rb.velocity.normalized, -transform.forward.normalized) > 0.5f) && rb.velocity.magnitude < 100f)
                {
                    rb.AddForce(-transform.forward * 100);
                }
                else 
                {
                    rb.velocity = -transform.forward * 10;
                }
            }

            // Update the pitch using the velocity
            audioSourceEngine.pitch = 1 + rb.velocity.magnitude / 100;

            // Change audio clips depending on the speed
            int currentState = rb.velocity.magnitude > 50 ? 2 : 1;
            
            // Changes the clip and plays it once the speed is over a certain range
            if (lastState != currentState)
            {
                audioSourceEngine.clip = currentState == 1 ? oneX : twoX;
                audioSourceEngine.Play();
            }

            // Update the lastState
            lastState = currentState;

            // locks the side to side controls when traversing a wall
            if (!rotatingWall)
            {
                // Turns left
                if (Input.GetKeyDown(KeyCode.A) && !rotatingLeft && !rotatingRight)
                {
                    // Boolean of turning left set
                    rotatingLeft = true;
                    // Rotate the player -45 degrees in 0.1 seconds
                    StartCoroutine(RotatePlayer(transform.up * -45f, 0.1f, () => rotatingLeft = false));
                    // Set the side value
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
                    // Boolean of turning left set
                    rotatingRight = true;
                    // Rotate the player -45 degrees in 0.1 seconds
                    StartCoroutine(RotatePlayer(transform.up * 45f, 0.1f, () => rotatingRight = false));
                    // Set the side value
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
            else if(!rotatingWall)
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
        // Checks if the collided object is has the floor and is not the start of the game
        if (other.CompareTag("Floor") && (Time.time - sceneStartTime) > 0f)
        {
            // set the boolean storing whether the function is running
            rotatingWall = true;
            
            // round the rotation to the nearest 45
            transform.rotation = Quaternion.Euler(RoundToNearest45(transform.rotation.eulerAngles.x), RoundToNearest45(transform.rotation.eulerAngles.y), RoundToNearest45(transform.rotation.eulerAngles.z));

            // variable to store what angle to rotate
            float rotationAngle = 90;

            // changes rotationAngle based on whether the player is moving forward
            if (Vector3.Dot(rb.velocity.normalized, transform.forward.normalized) > 0.5f)
            {
                rotationAngle = -90f;
            }

            // Check if either is close to 45 degrees (within a threshold)
            if (side == -1)
            {
                StartCoroutine(RotatePlayer(transform.up * 45f, 0.01f, () => StartCoroutine(RotatePlayer(transform.right * rotationAngle, 0.01f, () => StartCoroutine(RotatePlayer(transform.up * -45f, 0.01f, () => side = 1))))));
            }
            else if (side == 1)
            {
                StartCoroutine(RotatePlayer(transform.up * -45f, 0.01f, () => StartCoroutine(RotatePlayer(transform.right * rotationAngle, 0.01f, () => StartCoroutine(RotatePlayer(transform.up * 45f, 0.01f, () => side = -1))))));
            }
            else if (side == 0)
            {
                StartCoroutine(RotatePlayer(transform.right * rotationAngle, 0.1f));
            }

            // run the corountine to reset the rotatingwall bool
            StartCoroutine(ResetRotatingWall());
        } 
        // Checks the collider to see if it is a pickup
        else if (other.CompareTag("Pickup"))
        {
            // Plays a pickup sound
            audioSourcePoints.Play();

            // Checks if the points is the range
            if (points < 5)
            {
                // Incremenet the points
                points++;

                // Updates the points UI text
                pointsText.text = "Stars: " + points.ToString();

                // Destroys the pickup 
                Destroy(other.gameObject);

                // Checks if points is at max value
                if (points == 5)
                {
                    // Turns off point light and turns on spotlight with particle effect
                    pointLight.intensity = 0;
                    spotlight.intensity = 1000;
                    lightParticles.Play();
                }
            }
        }
        // Checks if the trigger area is the winning area
        else if (other.CompareTag("Winning area"))
        {
            // Checks if the points has reached the max
            if (points == 5)
            {
                // Stops the audioSource
                audioSourceEngine.Stop();

                // Checks if final time is the quickest and stores it in playerprefs
                if (PlayerPrefs.HasKey("QuickestTime") && PlayerPrefs.GetFloat("QuickestTime") > (Time.time - sceneStartTime))
                {
                    PlayerPrefs.SetFloat("QuickestTime", Mathf.Round((Time.time - sceneStartTime) * 100f) / 100f);
                    other.GetComponent<AudioSource>().clip = quickestSound;
                } 
                // if playerprefs does not exist the time automatically is the quickest
                else if (!PlayerPrefs.HasKey("QuickestTime"))
                {
                    PlayerPrefs.SetFloat("QuickestTime", Mathf.Round((Time.time - sceneStartTime) * 100f) / 100f);
                    other.GetComponent<AudioSource>().clip = quickestSound;
                }

                // Plays the winning sound
                other.GetComponent<AudioSource>().Play();

                // Updates the final time
                finalTimeText.text = "time: " + (Mathf.Round((Time.time - sceneStartTime) * 100f) / 100f).ToString();

                // Sets the quickest time text
                quickestTimeText.text = "Quickest Time: " + PlayerPrefs.GetFloat("QuickestTime").ToString();

                // Sets the won boolean to true
                won = true;

                // Place the player in the centre with no velocity
                transform.position = Vector3.up;
                rb.velocity = Vector3.zero;

                // Disables the gameUI and enables the winningUI
                gameUI.SetActive(false);
                winningUI.SetActive(true);
            }
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

    // when colliding with an object
    private void OnCollisionEnter()
    {
        // round the rotation to the nearest 45 degrees
        transform.rotation = Quaternion.Euler(RoundToNearest45(transform.rotation.eulerAngles.x), RoundToNearest45(transform.rotation.eulerAngles.y), RoundToNearest45(transform.rotation.eulerAngles.z));
    }
}
