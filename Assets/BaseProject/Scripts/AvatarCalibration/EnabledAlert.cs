using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnabledAlert : MonoBehaviour
{
    public HandOffset handOffsetManager;
    public bool isController;
    public bool isHand;

    private void OnEnable()
    {
        if (isController==true)
        {
            handOffsetManager.CopyControllerTransform();
            handOffsetManager.usingControllers = true;
            handOffsetManager.usingHandTracking = false;
        } 
        else if (isHand==true)
        {
            handOffsetManager.CopyHandTransform();
            handOffsetManager.usingControllers = false;
            handOffsetManager.usingHandTracking = true;
        } 
    }
}
