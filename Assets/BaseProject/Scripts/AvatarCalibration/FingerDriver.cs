using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerDriver : MonoBehaviour
{
    public GameObject parent1;
    public GameObject parent2;
    public GameObject parent3;
    public Vector3 parent1WorldPosition;
    public Quaternion parent1WorldRotation;
    public Vector3 distanceVector;
    private bool parent1bool1=false;
    private void Start()
    {
        parent1WorldPosition = parent1.transform.position;
        parent1WorldRotation = parent1.transform.rotation;
    }

    [ContextMenu("Parent1")]
    public void Parent1()
    {
        /*
        this.gameObject.transform.SetParent(parent1.transform);
        Debug.Log("Expected World Position: " + parent1WorldPosition);
        Debug.Log("Actual World Position After Reparenting: " + this.gameObject.transform.position);
        distanceVector = parent1WorldPosition - this.gameObject.transform.position;
        this.gameObject.transform.position = this.gameObject.transform.position - distanceVector;
        */
        parent1bool1 = true;
    }

    private void Update()
    {
        if (parent1bool1 == true)
        {
            ReparentGameObjectWithWorldTransform(this.transform.gameObject, parent1.transform);
        }
    }

    [ContextMenu("Parent2")]
    public void Parent2()
    {
        this.gameObject.transform.SetParent(parent2.transform);
    }

    [ContextMenu("Parent3")]
    public void Parent3()
    {
        this.gameObject.transform.SetParent(parent3.transform);
    }
    // Call this function with the GameObject you want to re-parent and the new parent transform.
    public void ReparentGameObjectWithWorldTransform(GameObject gameObjectToReparent, Transform newParent)
    {
        // Store current world transform
        Vector3 currentWorldPosition = gameObjectToReparent.transform.position;
        Quaternion currentWorldRotation = gameObjectToReparent.transform.rotation;
        Vector3 currentWorldScale = gameObjectToReparent.transform.lossyScale;

        // Set the new parent (without preserving world position just yet)
        gameObjectToReparent.transform.SetParent(newParent, false);

        // Restore the original world transform
        RestoreWorldTransform(gameObjectToReparent, currentWorldPosition, currentWorldRotation, currentWorldScale, newParent);
    }

    private void RestoreWorldTransform(GameObject gameObject, Vector3 worldPosition, Quaternion worldRotation, Vector3 worldScale, Transform newParent)
    {
        Debug.Log("---------------------------Restore world transform!");
        // Set world position and rotation directly
        gameObject.transform.position = worldPosition;
        gameObject.transform.rotation = worldRotation;
    }
}
