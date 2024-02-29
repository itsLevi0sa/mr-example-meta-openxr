using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion;
using System;
using RootMotion.FinalIK;

public class RetargetJoints : MonoBehaviour
{
    public FingerRig fingerRig;
    public Transform rotationValue;
    public float rotationThreshold = 90f;
    public int fingerNum;
    public Transform bone2;
    public Transform bone3;
    public bool passedThreshold;
    public float rotationValueX;

    private void Update()
    {
        rotationValueX = rotationValue.eulerAngles.x;
        if(rotationValue.eulerAngles.x>rotationThreshold)
        {
            passedThreshold = true;
            fingerRig.fingers[fingerNum].bone2 = bone3;
            fingerRig.fingers[fingerNum].bone3 = null;
        }
        else
        {
            passedThreshold = false;
            fingerRig.fingers[fingerNum].bone2 = bone2;
            fingerRig.fingers[fingerNum].bone3 = bone3;
        }
    }
}
