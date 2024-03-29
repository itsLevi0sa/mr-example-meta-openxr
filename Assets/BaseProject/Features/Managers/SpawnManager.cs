using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject xrKeyboard;
    [SerializeField] Transform target;
    [SerializeField] Vector3 offsetFromTarget;

    [ContextMenu("Spawn XR Keyboard")]
    public void XRKeyboardSpawner()
    {
        xrKeyboard.transform.position = target.transform.position+offsetFromTarget;
        xrKeyboard.SetActive(true);
        xrKeyboard.GetComponent<Animator>().SetTrigger("SpawnObject");
        xrKeyboard.GetComponent<AudioSource>().Play();
    }
}
