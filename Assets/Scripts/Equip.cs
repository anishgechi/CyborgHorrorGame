using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip : MonoBehaviour
{
    private EquipManager equipManager;
    private bool isPlaying = false;

    private void Start()
    {
        equipManager = EquipManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (equipManager.IsWalkmanEquipped)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (isPlaying)
                {
                    Debug.Log("Not playing");
                    isPlaying = false;
                }
                else
                {
                    Debug.Log("Playing");
                    isPlaying = true;
                }
            }
        }
    }
}



