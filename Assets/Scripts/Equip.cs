using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip : MonoBehaviour
{
    private EquipManager equipManager;
    private bool isPlaying = false;
    private Walkman walkman;

    private void Start()
    {
        equipManager = EquipManager.Instance;
        walkman = GetComponent<Walkman>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (equipManager.isWalkmanEquipped)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (isPlaying)
                {
                    Debug.Log("Not playing");
                    isPlaying = false;
                    walkman.StopDrain(); 
                }
                else
                {
                    Debug.Log("Playing");
                    isPlaying = true;
                    walkman.StartDrain(); 
                }
            }
        }

        if (equipManager.isWalkmanEquipped && walkman.BatteryPower <= 0)
        {
            Debug.Log("Battery depleted. Stopping playback.");
            isPlaying = false;
            walkman.StopDrain();
        }
    }
}

