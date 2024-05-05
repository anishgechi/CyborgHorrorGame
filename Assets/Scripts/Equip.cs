using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Equip : MonoBehaviour
{
    private EquipManager equipManager;
    private bool isPlaying = false;
    private Walkman walkman;
    public List<TextMeshProUGUI> playingTexts;
    public List<TextMeshProUGUI> notPlayingTexts;
    public AudioSource audioSource;
    public AudioClip[] Clips;


    // Start is called before the first frame update
    void Start()
    {
        equipManager = EquipManager.Instance;
        walkman = GetComponent<Walkman>();
        UpdateTextVisibility();
    }

    // Update is called once per frame
    void Update()
    {
        if (equipManager.isWalkmanEquipped)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                TogglePlayback();
            }

            if (isPlaying && walkman.BatteryPower <= 0)
            {
                Debug.Log("Battery depleted. Stopping playback.");
                TogglePlayback();
            }
        }

        UpdateTextVisibility();
    }

    private void TogglePlayback()
    {
        isPlaying = !isPlaying;

        if (isPlaying)
        {
            Debug.Log("Playback started.");
            walkman.StartDrain();

            if (Clips.Length > 0)
            {
                AudioClip randomClips = Clips[Random.Range(0, Clips.Length)];
                audioSource.clip = randomClips;
                Debug.Log("Attempting to play audio: " + randomClips.name);
                audioSource.Play();
                StartCoroutine(WaitForAudioToEnd(randomClips.length));
            }
        }
        else
        {
            Debug.Log("Playback stopped.");
            walkman.StopDrain();
            audioSource.Pause();
            StopAllCoroutines();
        }
    }

    private void UpdateTextVisibility()
    {
        foreach (TextMeshProUGUI textObject in playingTexts)
        {
            textObject.gameObject.SetActive(equipManager.isWalkmanEquipped && isPlaying);
        }

        foreach (TextMeshProUGUI textObject in notPlayingTexts)
        {
            textObject.gameObject.SetActive(equipManager.isWalkmanEquipped && !isPlaying);
        }
    }

    private IEnumerator WaitForAudioToEnd(float clipLength)
    {
        float fadeDuration = 3f; 
        float fadeStart = clipLength - fadeDuration; 
        while (true)
        {
            float startTime = Time.time;

            while (Time.time - startTime < clipLength)
            {
                float elapsed = Time.time - startTime;
                float remaining = clipLength - elapsed;

                if (remaining <= fadeStart)
                {
                    float fadeFactor = Mathf.Clamp01((fadeDuration - remaining) / fadeDuration);

                    audioSource.volume = 1f - fadeFactor;
                }

                yield return null;
            }

            if (Clips.Length == 1)
            {
                audioSource.Stop();
                audioSource.Play();
            }
            else
            {
                AudioClip randomClip = Clips[Random.Range(0, Clips.Length)];
                audioSource.clip = randomClip;
                audioSource.Play();
            }

            audioSource.volume = 1f;
        }
    }
}


