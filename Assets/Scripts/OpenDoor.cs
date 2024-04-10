using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public Animation hingehere;
    public Component doorcolliderhere;

    void OnTriggerStay()
    {
        if (Input.GetKey(KeyCode.E))
            hingehere.Play();

        if (Input.GetKey(KeyCode.E))
            doorcolliderhere.GetComponent<BoxCollider>().enabled = false;
    }
}
