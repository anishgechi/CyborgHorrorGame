using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CamBob : MonoBehaviour
{
    public Transform PlayerTransform;
    public float BobAmount = 0.11f;
    public float BobSpeed = 7.5f;
    public float SprintingBobSpeed = 20f; 
    public float ReturnOffsetSpeed = 3f;

    private Vector3 OriginalPosition;
    private bool isMoving;

    void Start()
    {
        OriginalPosition = transform.localPosition;
    }

    void Update()
    {
        isMoving = PlayerTransform.GetComponent<Movement>().IsMoving;

        float bobSpeed = PlayerTransform.GetComponent<Movement>().IsSprinting ? SprintingBobSpeed : BobSpeed;

        float bobOffset = Mathf.Sin(Time.time * bobSpeed) * BobAmount;

        Vector3 targetPosition = OriginalPosition + Vector3.up * bobOffset;

        transform.localPosition = isMoving ? targetPosition : Vector3.Lerp(transform.localPosition, OriginalPosition, Time.deltaTime * ReturnOffsetSpeed);
    }
}


