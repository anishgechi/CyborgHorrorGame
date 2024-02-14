using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SanityKiller : MonoBehaviour
{
    [Header("Player sanity level settings")]
    [SerializeField] float MaxSanity = 100f;
    [SerializeField] float MinSanity = 0f;
    [SerializeField] float CurrentSanity = 100f;
    [SerializeField] float SanityDecreaseRate = 5.5f;
    [SerializeField] float DetectionRadius = 10f;

    public LayerMask PlayerLayer;
    public bool IsPlayerDetected = false;
    public Image SanityBar;

    [Header("Camera controller")]
    [SerializeField] float MaxShakeIntensity = 0.5f;
    [SerializeField] float ShakeFrequency = 20f;
    public Camera PLMainCamera;

    public CamBob Bob;

    [Header("Field of View settings")]
    [SerializeField] float MinFOV = 60f;
    [SerializeField] float MaxFOV = 110f;
    [SerializeField] float FOVChangeRate = 1f;

    private float TargetFOV;

    // Start is called before the first frame update
    void Start()
    {
        UpdateSanityBar();
        TargetFOV = PLMainCamera.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlayerDetected)
        {
            CurrentSanity -= SanityDecreaseRate * Time.deltaTime;
            CurrentSanity = Mathf.Max(CurrentSanity, 0f);
            UpdateSanityBar();
        }

        UpdateFOV();

        if (CurrentSanity != MaxSanity)
        {
            UpdateCameraShake();
        }

        if (CurrentSanity <= 96f)
        {
            Bob.enabled = false;
        }
        else
        {
            Bob.enabled = true;
        }
    }

    void UpdateSanityBar()
    {
        SanityBar.fillAmount = Mathf.Clamp01((CurrentSanity - MinSanity) / (MaxSanity - MinSanity));
    }

    private void OnValidate()
    {
        CurrentSanity = Mathf.Clamp(CurrentSanity, MinSanity, MaxSanity);
        UpdateSanityBar();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & PlayerLayer) != 0)
        {
            IsPlayerDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & PlayerLayer) != 0)
        {
            IsPlayerDetected = false;
        }
    }

    void UpdateCameraShake()
    {
        float shakeIntensity = MaxShakeIntensity * (1f - (CurrentSanity / MaxSanity));

        float offsetX = Mathf.PerlinNoise(Time.time * ShakeFrequency, 0) * shakeIntensity;
        float offsetY = Mathf.PerlinNoise(0, Time.time * ShakeFrequency) * shakeIntensity;
        PLMainCamera.transform.localPosition = new Vector3(offsetX, offsetY, PLMainCamera.transform.localPosition.z);
    }

    void UpdateFOV()
    {
        if (CurrentSanity < 60f)
        {
            float normalizedSanity = Mathf.Clamp01((CurrentSanity - MinSanity) / (60f - MinSanity));
            float targetFOV = Mathf.Lerp(MinFOV, MaxFOV, 1f - normalizedSanity);

            PLMainCamera.fieldOfView = Mathf.Lerp(PLMainCamera.fieldOfView, targetFOV, Time.deltaTime * FOVChangeRate);
        }
        else
        {
            PLMainCamera.fieldOfView = Mathf.Lerp(PLMainCamera.fieldOfView, MinFOV, Time.deltaTime * FOVChangeRate);
        }
    }
}
