using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChareRemaining : MonoBehaviour
{
    [Header("BatteryChargeup")]
    public float batteryIncreaseAmount = 0.25f; 
    public GameObject playerWalkManOverlay;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player")) 
        {
            Walkman PlayerWalkMan = playerWalkManOverlay.GetComponent<Walkman>(); 

            if (PlayerWalkMan != null && PlayerWalkMan.BatteryPower < 1.0f) 
            {
                PlayerWalkMan.BatteryPower = Mathf.Clamp(PlayerWalkMan.BatteryPower, 0.0f, 1.0f); 
                PlayerWalkMan.BatteryPower += batteryIncreaseAmount; 
                Destroy(gameObject); 
            }
        }
    }

}
