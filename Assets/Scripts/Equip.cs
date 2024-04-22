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
            walkman.StartDrain();
        else
            walkman.StopDrain();
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
}


