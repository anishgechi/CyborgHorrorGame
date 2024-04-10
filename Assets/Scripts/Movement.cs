using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] float WalkSpeed = 5f; 
    [SerializeField] float SprintSpeed = 10f; 
    [SerializeField] float CrouchSpeed = 3f; 
    [SerializeField] float JumpHeight = 8f; 
    [SerializeField] float SprintMaxTime = 8f; 
    [SerializeField] float StaminaRegain = 4f;  
    [SerializeField] float CurrentStamina; 
    [SerializeField] float MaxStamina = 100f; 
    [SerializeField] float CrouchHeight = 0.5f; 
    [SerializeField] float StandingHeight = 1.0f;

    [Header("BoolCheckers")]
    public bool IsSprinting = false;
    public bool IsMoving = false;
    public bool IsAudible = false;

    [Header("UI and Audio")]
    public AudioClip[] FootStepFXList;
    public AudioSource FootStepFX;
    public Image PlayerStaminaBar; 
    
    private Rigidbody RB; 
    private CapsuleCollider PlayersCapsule; 
    private bool CanJump = false; 
    private bool CanRegenerateStamina = true; 
    private bool canStartRegeneration = true; 
    private bool IsCrouching = false;
    private bool CanLook = true;
    private bool isStaminaBoosted = false;


    [Header("Look Variables")]
    public Transform CameraOBJ; 
    public float MinXTurn; 
    public float MaxXTurn; 
    public float LookSens;

    private float CamCurrentXrotation; 
    private Vector2 MouseDelta; 

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            if (CanLook == true)
            {
                PlayerLook();
            }
            PlayerJump();
            CrouchToggled();
            PlayerMovement();
            PlayFootStepSound();
        }
    }

    void FixedUpdate()
    {

    }
    
    // Start is called before the first frame update
    void Start()
    {
        PlayersCapsule = GetComponent<CapsuleCollider>(); 
        RB = GetComponent<Rigidbody>(); 
        CurrentStamina = MaxStamina; 
        UpdateStaminaBar(); 
    }

    void PlayerLook()
    {
        MouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); 

        CamCurrentXrotation += MouseDelta.y * LookSens; 
        CamCurrentXrotation = Mathf.Clamp(CamCurrentXrotation, MinXTurn, MaxXTurn); 
        CameraOBJ.localEulerAngles = new Vector3(-CamCurrentXrotation, 0, 0); 
        transform.eulerAngles += new Vector3(0, MouseDelta.x * LookSens, 0);
    }

    void PlayerMovement()
    {
        IsSprinting = (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && CurrentStamina > 0);
        IsMoving = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || IsSprinting || Input.GetKeyDown(KeyCode.Space));
        
        float horizontalInput = 0f;
        float verticalInput = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            verticalInput = 1f;
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
            WalkSpeed = CrouchSpeed;
        }
        else if (IsSprinting)
        {
            verticalInput = 1f;
            WalkSpeed = SprintSpeed;

            if (!isStaminaBoosted)
            {
                DecreaseStamina(Time.deltaTime);
            }
            CanRegenerateStamina = true;
        }
        else
        {
            WalkSpeed = 5f;
        }

        if (!CanRegenerateStamina)
        {
            StartCoroutine(DelayStaminaRegeneration());
        }
        else if (CanRegenerateStamina && !isStaminaBoosted) 
        {
            RegenerateStamina(Time.deltaTime);
        }

        Vector3 movement = transform.right * horizontalInput + transform.forward * verticalInput;
        movement.Normalize();

        transform.Translate(movement * WalkSpeed * Time.deltaTime, Space.World);
    }


    void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CanJump == true) 
        {
            RB.AddForce(new Vector3(0, JumpHeight, 0), ForceMode.Impulse); 
            CanJump = false; 
            IsMoving = true;
            IsCrouching = false;
            PlayersCapsule.height = StandingHeight;
        }
    }

    void CrouchToggled() 
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)) 
        {
            IsCrouching = !IsCrouching; 

            if (IsCrouching)  
            {
                PlayersCapsule.height = CrouchHeight; 
            }
            else 
            {
                PlayersCapsule.height = StandingHeight; 
            }
        }
    }

    void DecreaseStamina(float amount)
    {
        CurrentStamina -= amount; 
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0f, MaxStamina); 

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
            
            CurrentStamina += StaminaRegain * deltaTime;
            CurrentStamina = Mathf.Clamp(CurrentStamina, 0f, MaxStamina);
            UpdateStaminaBar();
        }
    }

    void UpdateStaminaBar()
    {
        float fillAmount = CurrentStamina / MaxStamina;
        PlayerStaminaBar.fillAmount = fillAmount;
    }

    IEnumerator DelayStaminaRegeneration()
    {
        Debug.Log("Stamina regeneration delay started");
        yield return new WaitForSeconds(4f); 
        canStartRegeneration = true; 
        Debug.Log("Stamina regeneration enabled again");
    }

    public IEnumerator TemporaryStaminaBoost(float duration)
    {
        CurrentStamina = MaxStamina;
        UpdateStaminaBar();
        CanRegenerateStamina = false;
        isStaminaBoosted = true;
        yield return new WaitForSeconds(duration);

      
        isStaminaBoosted = false;
        CanRegenerateStamina = true;
        UpdateStaminaBar();
    }

    public void PlayFootStepSound()
    {
        if (IsMoving && !IsCrouching)
        {
            if (!FootStepFX.isPlaying)
            {
                int randomIndex = Random.Range(0, FootStepFXList.Length);
                FootStepFX.clip = FootStepFXList[randomIndex];
                FootStepFX.Play();
                IsAudible = true;
            }
        }
        else
        {
            FootStepFX.Stop();
            IsAudible = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) 
        {
            CanJump = true; 
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) 
        {
            CanJump = false; 
        }
    }

    public void ToggleCursor(bool toggle) 
    {
        Cursor.lockState = toggle?CursorLockMode.None : CursorLockMode.Locked;
        CanLook = !toggle;
    }
}