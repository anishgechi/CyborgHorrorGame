using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Walkman : MonoBehaviour
{
    [Header("BatteryCanvas")]
    [SerializeField] Image BatteryBars;
    [SerializeField] float Drainage = 5.0f;

    public float BatteryPower = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (BatteryBars == null)
        {
            BatteryBars = GameObject.Find("InnerBattery")?.GetComponent<Image>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (BatteryBars != null)
        {
            BatteryBars.fillAmount = BatteryPower;
        }
    }

    void BatteryDrainage()
    {
        if (BatteryPower > 0.0f)
        {
            BatteryPower -= 0.25f;
        }
    }

    public void StartDrain()
    {
        InvokeRepeating("BatteryDrainage", Drainage, Drainage);
    }

    public void StopDrain()
    {
        CancelInvoke("BatteryDrainage");
    }
}

