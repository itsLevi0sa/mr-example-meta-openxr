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
            handOffsetManager.UsingControllers = true;
            handOffsetManager.UsingHandTracking = false;
        } 
        else if (isHand==true)
        {
            handOffsetManager.CopyHandTransform();
            handOffsetManager.UsingControllers = false;
            handOffsetManager.UsingHandTracking = true;
        } 
    }
}
