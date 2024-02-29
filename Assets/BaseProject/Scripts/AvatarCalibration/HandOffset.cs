using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class HandOffset : MonoBehaviour
{
    [SerializeField] GameObject hand;
    [SerializeField] GameObject controller;
    [SerializeField] GameObject controllerOffset;
    [SerializeField] GameObject handOffset;
    [SerializeField] GameObject referenceOffset;
    public bool usingControllers=false;
    public bool usingHandTracking = false;
    public bool showSystemHands;
    [SerializeField] SkinnedMeshRenderer leftHandVisualizer;
    [SerializeField] SkinnedMeshRenderer rightHandVisualizer;


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
        leftHandVisualizer.enabled = true;
        rightHandVisualizer.enabled = true;
    }

    public void HideHandVisualizers()
    {
        leftHandVisualizer.enabled = false;
        rightHandVisualizer.enabled = false;
    }
}
