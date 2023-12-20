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

    // Update is called once per frame
    void Update()
    {
        
    }
}
