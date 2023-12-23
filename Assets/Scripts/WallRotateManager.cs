using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRotateManager : MonoBehaviour
{
    // Runs when a collision occurs
    void OnCollisionEnter(Collision collisionObject)
    {
        // Create a new variable to store the rigidbody of the collision object
        Rigidbody rb = collisionObject.collider.GetComponent<Rigidbody>();

        // Checks if the collision object has a transform and a rigibody
        if (collisionObject.transform != null && rb != null)
        {
            Vector3 currentVelocity = rb.velocity;
            collisionObject.transform.rotation = Quaternion.Euler(-90f, 0f, 0f) * collisionObject.transform.rotation;
            rb.velocity = Quaternion.Euler(-90f, 0f, 0f) * rb.velocity;
        }
    }
}
