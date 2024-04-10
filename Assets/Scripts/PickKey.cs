using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickKey : MonoBehaviour
{
    public Component doorcolliderhere;
    public GameObject keygone;

    void OnTriggerStay()
    {
        if (Input.GetKey(KeyCode.E))
            doorcolliderhere.GetComponent<BoxCollider>().enabled = true;

        if (Input.GetKey(KeyCode.E))
            keygone.SetActive(false);

    }
}
