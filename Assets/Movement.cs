using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] float WalkSpeed = 5f; // Adjust this to set the walking speed
    [SerializeField] float SprintSpeed = 10f; // Adjust this to set the sprinting speed
    [SerializeField] float CrouchSpeed = 3f; // Adjust this to set the crouch speed
    [SerializeField] float JumpHeight = 8f; // Adjust this to set the jump force
    [SerializeField] float SprintMaxTime = 8f; // 
    [SerializeField] float StaminaRegain = 4f; // variable for how much regains 
    [SerializeField] float CurrentStamina; // players current stamina variable
    [SerializeField] float MaxStamina = 100f; // maximum value of stamina
    [SerializeField] float CrouchHeight = 0.5f; // crouchingheight
    [SerializeField] float StandingHeight = 1.0f; // the normal height of the player


    public Image PlayerStaminaBar; // stamina bar image reference
    private Rigidbody RB; // Reference to the Rigidbody component
    private CapsuleCollider PlayersCapsule; // players capsule collider variable
    private bool CanJump = false; // private bool for checking if player can jump 
    private bool IsMoving = false; // bool checker for player movement
    private bool CanRegenerateStamina = true; // bool check for when stamina regen can occur
    private bool canStartRegeneration = true; // starts the regen of stamina 
    private bool IsCrouching = false; // crouching bool


    [Header("Look Variables")]
    public Transform CameraOBJ; // reference our camera gameobj in our scene
    public float MinXTurn; // minimum the player can turn in the x 
    public float MaxXTurn; // maximum the player can turn in the x
    public float LookSens; // the sensitivity multiplier 

    private float CamCurrentXrotation; // tracks and stores the current x value the mouse is going in
    private Vector2 MouseDelta; // Store mouse delta here

    // Update is called once per frame
    void Update()
    {
        PlayerLook();
        PlayerJump();
        CrouchToggled();
        PlayerMovement();
    }

    void FixedUpdate()
    {

    }

    void Start()
    {
        PlayersCapsule = GetComponent<CapsuleCollider>(); // get the capsulecollider
        RB = GetComponent<Rigidbody>(); // Get the Rigidbody component
        CurrentStamina = MaxStamina; // sets the players stamina to the max value when the program is ran
        UpdateStaminaBar(); // stamina bar update method is ran at start
    }

    void PlayerLook()
    {
        // Capture mouse movement delta
        MouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // gets the y and x axis of our mouse and stores it inside "mouseDelta"

        CamCurrentXrotation += MouseDelta.y * LookSens; // currentcam location is added by the vertical movement of the mouse multiplied by the look sensitivity 
        CamCurrentXrotation = Mathf.Clamp(CamCurrentXrotation, MinXTurn, MaxXTurn); // the current cam is CLAMPED by the minxturn and maxxturn variables
        CameraOBJ.localEulerAngles = new Vector3(-CamCurrentXrotation, 0, 0); // the rotation of the cameraobj is defined by the camcurrentxrotation variable to rotate the character and zero out the rest
        transform.eulerAngles += new Vector3(0, MouseDelta.x * LookSens, 0);
    }

    void PlayerMovement() // player movement method 
    {
        IsMoving = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)));
        float horizontalInput = 0f; // cache variable for the left and right
        float verticalInput = 0f; // local variable for the forwards and backwards

        if (Input.GetKey(KeyCode.W)) // if the key is pressed or held
        {
            verticalInput = 1f; // move forwards
        }
        else if (Input.GetKey(KeyCode.S))
        {
            verticalInput = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
        }

        if (IsCrouching)
        {
            WalkSpeed = CrouchSpeed; // Set WalkSpeed to CrouchSpeed when crouching
        }
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && CurrentStamina > 0)  // Sprint logic when Shift + W is pressed
        {
            verticalInput = 1f; // Always move forward
            WalkSpeed = SprintSpeed; // Set the WalkSpeed to SprintSpeed
            DecreaseStamina(Time.deltaTime); // decrease stamina method is activated with the parameters of timedeltatime
            CanRegenerateStamina = true; // regeneration is now allowed 
        }
        else
        {
            WalkSpeed = 5f; // Reset WalkSpeed to its default value
        }

        if (!CanRegenerateStamina) // Always attempt to regenerate stamina, regardless of crouching state
        {
            StartCoroutine(DelayStaminaRegeneration()); // Delay regeneration when stamina reaches zero
        }
        else if (CanRegenerateStamina) // Regenerate stamina if allowed
        {
            RegenerateStamina(Time.deltaTime);
        }

        Vector3 movement = transform.right * horizontalInput + transform.forward * verticalInput;
        movement.Normalize();

        transform.Translate(movement * WalkSpeed * Time.deltaTime, Space.World);
    }


    void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CanJump == true) // if the space key is pressed and canjump is true
        {
            RB.AddForce(new Vector3(0, JumpHeight, 0), ForceMode.Impulse); // add the force to the rb
            CanJump = false; // false afterwards
            IsMoving = true;
            IsCrouching = false;
            PlayersCapsule.height = StandingHeight;
        }
    }

    void CrouchToggled() 
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)) // checks if left ctrl is pressed
        {
            IsCrouching = !IsCrouching; // the bool is set to either true or false

            if (IsCrouching)  // when its true 
            {
                PlayersCapsule.height = CrouchHeight; // the height will be the crouch height
            }
            else // otherwise when the bool is falsed
            {
                PlayersCapsule.height = StandingHeight; // player capsule is set to normal height
            }
        }
    }

    void DecreaseStamina(float amount)
    {
        CurrentStamina -= amount; // Decrease stamina by a certain amount
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0f, MaxStamina); // Clamp stamina to not go below zero

        if (CurrentStamina <= 0f && canStartRegeneration)
        {
            canStartRegeneration = false;
            StartCoroutine(DelayStaminaRegeneration());
        }
        UpdateStaminaBar();
    }

    void RegenerateStamina(float deltaTime)
    {
        if (canStartRegeneration && CurrentStamina < MaxStamina)
        {
            // Regenerate stamina when allowed and not at max
            CurrentStamina += StaminaRegain * deltaTime;
            CurrentStamina = Mathf.Clamp(CurrentStamina, 0f, MaxStamina);
            UpdateStaminaBar();
        }
    }

    void UpdateStaminaBar()
    {
        // Update the UI for the stamina bar
        float fillAmount = CurrentStamina / MaxStamina;
        PlayerStaminaBar.fillAmount = fillAmount;
    }

    IEnumerator DelayStaminaRegeneration()
    {
        Debug.Log("Stamina regeneration delay started");
        yield return new WaitForSeconds(4f); // Change the delay duration as needed
        canStartRegeneration = true; // Enable stamina regeneration after the delay
        Debug.Log("Stamina regeneration enabled again");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) // Check if the collision is with an object in the ground layer named "Ground"
        {
            CanJump = true; // Allow jumping if there's a collision with an object in the ground layer
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) // Reset CanJump when the player leaves the ground
        {
            CanJump = false; // Prevent jumping if the player is not colliding with an object in the ground layer
        }
    }
}