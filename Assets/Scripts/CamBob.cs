using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CamBob : MonoBehaviour
{
    [Header("CameraBobbing settings")]
    [SerializeField] Transform PlayerTransform;
    [SerializeField] float BobAmount = 0.11f;
    [SerializeField] float BobSpeed = 7.5f;
    [SerializeField] float SprintingBobSpeed = 20f; 
    [SerializeField] float ReturnOffsetSpeed = 3f;

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


