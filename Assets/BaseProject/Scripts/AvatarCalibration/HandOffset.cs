using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class HandOffset : MonoBehaviour
{
    [SerializeField] GameObject hand;
    [SerializeField] GameObject controller;
    [SerializeField] GameObject controllerOffset;
    private GameObject targetObject;

    private bool isActive = false;

    private void Update()
    {
        if (hand.activeSelf==true)
        {
            targetObject = hand;
            CopyTransform();
        }else if (controller.activeSelf==true)
        {
            targetObject = controller;
            CopyTransform();
        }
    }
    void CopyTransform()
    {
        // Copy the transform
        this.transform.SetParent(targetObject.transform);
        this.transform.position = controllerOffset.transform.position;
        this.transform.rotation = controllerOffset.transform.rotation;
    }
}
