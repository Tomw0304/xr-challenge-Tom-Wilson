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
        float angleDifferenceX = -0f;
        float angleDifferenceY = 0f;

        // Checks if the collision object has a transform and a rigibody
        if (collisionObject.transform != null && rb != null)
        {
            if(collisionObject.transform.rotation.y == 1f)
            {
                collisionObject.transform.rotation = Quaternion.Euler(
                    collisionObject.transform.rotation.eulerAngles.x,
                    -collisionObject.transform.rotation.eulerAngles.y,
                    collisionObject.transform.rotation.eulerAngles.z
                );
            }
            //Calculates the angleDifference based on the angle the object is colliding with the wall
            if (collisionObject.transform.rotation.z < 0 || collisionObject.transform.rotation.y < 0)
            {
                angleDifferenceX = 90f;
            }
            else if (collisionObject.transform.rotation.y == 0)
            {
                angleDifferenceX = -90f;
            }

            Vector3 currentVelocity = rb.velocity;
            collisionObject.transform.rotation = Quaternion.Euler(angleDifferenceX, angleDifferenceY, 0f) * collisionObject.transform.rotation;
            rb.velocity = Quaternion.Euler(angleDifferenceX, angleDifferenceY, 0f) * rb.velocity;
        }
    }
}
