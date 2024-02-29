using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public GameObject target;
    public Vector3 positionOffset;
    public Quaternion rotationOffset;

    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.position = target.transform.position+positionOffset;
        this.transform.rotation = target.transform.rotation;
    }
}
