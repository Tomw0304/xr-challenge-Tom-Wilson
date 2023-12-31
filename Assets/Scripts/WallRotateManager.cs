using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRotateManager : MonoBehaviour
{
    // Runs when a collision occurs
    void OnTriggerEnter(Collider collisionObject)
    {
        // Create a new variable to store the rigidbody of the collision object
        Rigidbody rb = collisionObject.GetComponent<Rigidbody>();

        //stores the angle that the object needs to be rotated by
        float angleDifferenceX = 0f;
        float angleDifferenceY = 0f;
        float angleDifferenceZ = 0f;

        // Checks if the collision object has a transform and a rigibody
        if (collisionObject.transform != null && rb != null)
        {
            // Checks if the object has returned to the original rotation with a negative value
            if (collisionObject.transform.rotation == new Quaternion(0f, 0f, 0f, -1f) || collisionObject.transform.rotation == new Quaternion(0f, 1f, 0f, 0f) || collisionObject.transform.rotation == new Quaternion(0f, 0.7071068f, 0f, -0.7071068f) || collisionObject.transform.rotation == new Quaternion(0f, -0.7071068f, 0f, -0.7071068f))
            {
                // reverse the negation of the rotation
                collisionObject.transform.rotation = new Quaternion(-collisionObject.transform.rotation.x, -collisionObject.transform.rotation.y, -collisionObject.transform.rotation.z, -collisionObject.transform.rotation.w);
            }
            //Calculates the angleDifference based on the angle the object is colliding with the wall
            if (collisionObject.transform.rotation == new Quaternion(0f,-0.7071068f,0f,0.7071068f) || collisionObject.transform.rotation == new Quaternion(-0.5f, -0.5f, -0.5f, 0.5f) || collisionObject.transform.rotation == new Quaternion(-0.7071068f, 0f, -0.7071068f, 0f) || collisionObject.transform.rotation == new Quaternion(-0.5f, 0.5f, -0.5f, -0.5f))
            {
                angleDifferenceZ = -90f;
            }
            else if (collisionObject.transform.rotation == new Quaternion(0f, 0.7071068f, 0f, 0.7071068f) || collisionObject.transform.rotation == new Quaternion(-0.5f, 0.5f, 0.5f, 0.5f) || collisionObject.transform.rotation == new Quaternion(-0.7071068f, 0f, 0.7071068f, 0f) || collisionObject.transform.rotation == new Quaternion(-0.5f, -0.5f, 0.5f, -0.5f))
            {
                angleDifferenceZ = 90f;
            }
            else if (collisionObject.transform.rotation.z < 0 || collisionObject.transform.rotation.y < 0)
            {
                angleDifferenceX = 90f;
            }
            else if (collisionObject.transform.rotation.y == 0)
            {
                angleDifferenceX = -90f;
            }

            // applies the angle differences to the object transform and velocity
            collisionObject.transform.rotation = Quaternion.Euler(angleDifferenceX, angleDifferenceY, angleDifferenceZ) * collisionObject.transform.rotation;
            rb.velocity = Quaternion.Euler(angleDifferenceX, angleDifferenceY, angleDifferenceZ) * rb.velocity;
        }
    }
}
