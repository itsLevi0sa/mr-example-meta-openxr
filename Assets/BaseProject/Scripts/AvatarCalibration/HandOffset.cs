using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;
using RootMotion.FinalIK;
using System;

public class HandOffset : MonoBehaviour
{
    public event Action OnUsingControllersSetToTrue;
    public event Action OnUsingHandTrackingSetToTrue;

    [SerializeField] GameObject hand;
    [SerializeField] GameObject controller;
    [SerializeField] GameObject controllerOffset;
    [SerializeField] GameObject handOffset;
    [SerializeField] GameObject referenceOffset;
    [SerializeField] GameObject targetPoints;
    [SerializeField] FingerRig handFingerRig;

    private bool usingControllers;
    public bool UsingControllers
    {
        get => usingControllers;
        set
        {
            usingControllers = value;
            // Check if the new value is true and the event has subscribers before invoking
            if (usingControllers && OnUsingControllersSetToTrue != null)
            {
                OnUsingControllersSetToTrue.Invoke();
            }
        }
    }
    private bool usingHandTracking;
    public bool UsingHandTracking
    {
        get => usingHandTracking;
        set
        {
            usingHandTracking = value;
            // Check if the new value is true and the event has subscribers before invoking
            if (usingHandTracking && OnUsingHandTrackingSetToTrue != null)
            {
                OnUsingHandTrackingSetToTrue.Invoke();
            }
        }
    }

    public bool showSystemHands;
    // Previous state of the bool to detect changes
    private bool previousSystemHandsVisibilityState;
    [SerializeField] SkinnedMeshRenderer handVisualizer;

    private void Start()
    {
        showSystemHands = false;
        previousSystemHandsVisibilityState = false;
        HideHandVisualizers();

    }
    private void OnEnable()
    {
        // Subscribe the RespondToBoolTrue method to the OnBoolSetTrue event
        OnUsingControllersSetToTrue += DeactivateFingerControls;
        OnUsingHandTrackingSetToTrue += ActivateFingerControls;
    }

    private void OnDisable()
    {
        // Unsubscribe the RespondToBoolTrue method from the OnBoolSetTrue event
        OnUsingControllersSetToTrue -= DeactivateFingerControls;
        OnUsingHandTrackingSetToTrue -= ActivateFingerControls;
    }

    private void OnValidate()
    {
        // Check if the bool has been changed to true
        if (showSystemHands && !previousSystemHandsVisibilityState)
        {
            ShowHandVisualizers();
        }
        else
        {
            HideHandVisualizers();
        }

        // Update the previous state
       previousSystemHandsVisibilityState=showSystemHands;
    }

    private void Update()
    {
        if (showSystemHands==false)
        {
            HideHandVisualizers();
        }
        else
        {
            ShowHandVisualizers();
        }
    }
    public void CopyHandTransform()
    {
        referenceOffset.transform.SetParent(hand.transform);
        referenceOffset.transform.position = handOffset.transform.position;
        referenceOffset.transform.rotation = handOffset.transform.rotation;
    }

    public void CopyControllerTransform()
    {
        referenceOffset.transform.SetParent(controller.transform);
        referenceOffset.transform.position = controllerOffset.transform.position;
        referenceOffset.transform.rotation = controllerOffset.transform.rotation;
    }

    public void ShowHandVisualizers()
    {
        handVisualizer.enabled = true;
    }

    public void HideHandVisualizers()
    {
        handVisualizer.enabled = false;
    }

    public void DeactivateFingerControls()
    {
        targetPoints.SetActive(false);
        handFingerRig.enabled = false;
    }
    public void ActivateFingerControls()
    {
        targetPoints.SetActive(true);
        handFingerRig.enabled = true;
    }
}
