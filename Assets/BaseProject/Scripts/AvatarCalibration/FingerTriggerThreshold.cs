using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerTriggerThreshold : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FingerThreshold")
        {
            Debug.Log("Entered finger threshold!");
            other.GetComponent<RetargetJoints>().bendFinger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "FingerThreshold")
        {
            other.GetComponent<RetargetJoints>().bendFinger = false;
            Debug.Log("Exited finger threshold!");
        }
    }
}
