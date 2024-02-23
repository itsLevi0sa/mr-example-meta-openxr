using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorTransform : MonoBehaviour
{
    public Transform mirrorPoint = null;
    public Transform transformToMirror = null;

    void Update()
    {
        Quaternion localRot = transformToMirror.localRotation;

        localRot.x *= -1;
        localRot.y *= -1;

        transform.localRotation = localRot;

        var offset = Vector3.Scale(new Vector3(-1f, -1f, 1f), mirrorPoint.position - transformToMirror.position);

        transform.position = mirrorPoint.position + offset;
    }
}
