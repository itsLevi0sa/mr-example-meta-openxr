using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion;
using System;
using RootMotion.FinalIK;

public class RetargetJoints : MonoBehaviour
{
    public FingerRig fingerRig;
    public int fingerNum;
    public Transform bone2;
    public Transform bone3;
    public bool bendFinger;

    private void LateUpdate()
    {

        if (bendFinger == false)
        {
            fingerRig.fingers[fingerNum].bone2 = bone3;
            fingerRig.fingers[fingerNum].bone3 = null;
        }
        else
        {
            fingerRig.fingers[fingerNum].bone2 = bone2;
            fingerRig.fingers[fingerNum].bone3 = bone3;
        }

    }

}
