using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationConstraint : MonoBehaviour
{
    public float minRotationLimit;
    void Update()
    {
        // Get the current rotation in Euler angles
        Vector3 currentRotation = transform.eulerAngles;

        // Clamp the x rotation to not fall below 20 degrees
        currentRotation.x = ClampAngle(currentRotation.x, minRotationLimit, 360f);

        // Apply the clamped rotation back to the object
        transform.eulerAngles = currentRotation;
    }

    // Helper method to clamp the angle between a minimum and a maximum value
    // This method also handles the 360-degree wrap-around.
    float ClampAngle(float angle, float min, float max)
    {
        // Ensure the angle is within 0 to 360 range
        angle = NormalizeAngle(angle);

        // Clamp the angle within the specified range
        return Mathf.Clamp(angle, min, max);
    }

    // Normalize angles to be within 0 to 360 degrees
    float NormalizeAngle(float angle)
    {
        while (angle > 360) angle -= 360;
        while (angle < 0) angle += 360;
        return angle;
    }
}
