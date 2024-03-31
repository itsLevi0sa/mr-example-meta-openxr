using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject xrKeyboard;
    [SerializeField] Transform target;
    //[SerializeField] Vector3 offsetFromTarget;

    [ContextMenu("Spawn XR Keyboard")]
    public void XRKeyboardSpawner()
    {
        //xrKeyboard.transform.position = target.transform.position+offsetFromTarget;
        xrKeyboard.SetActive(true);
        xrKeyboard.GetComponent<Animator>().SetTrigger("SpawnObject");
        xrKeyboard.GetComponent<AudioSource>().Play();
    }

    [ContextMenu("Despawn XR Keyboard")]
    public void XRKeyboardDespawner()
    {
        //xrKeyboard.transform.position = target.transform.position+offsetFromTarget;
        
        xrKeyboard.GetComponent<Animator>().SetTrigger("DespawnObject");
        xrKeyboard.GetComponent<AudioSource>().Play();
        xrKeyboard.SetActive(false);
    }
}
