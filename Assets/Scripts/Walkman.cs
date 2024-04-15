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
    public float BatteryRestoreDuration = 2.0f;


    private SanityKiller sanityKiller;

    Coroutine batteryRestoreCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        if (BatteryBars == null)
        {
            BatteryBars = GameObject.Find("InnerBattery")?.GetComponent<Image>();
        }
        sanityKiller = FindObjectOfType<SanityKiller>();
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

            if (BatteryPower % 0.25f == 0)
            {
                if (batteryRestoreCoroutine == null)
                {
                    batteryRestoreCoroutine = StartCoroutine(RestoreSanityOverTime());
                }
            }
        }
    }

    IEnumerator RestoreSanityOverTime()
    {
        float elapsedTime = 0f;
        float startSanity = sanityKiller.CurrentSanity;
        float endSanity = Mathf.Min(sanityKiller.CurrentSanity + 25f, sanityKiller.MaxSanity); 

        while (elapsedTime < BatteryRestoreDuration)
        {
            float t = elapsedTime / BatteryRestoreDuration;
            sanityKiller.CurrentSanity = Mathf.Lerp(startSanity, endSanity, t);
            sanityKiller.UpdateSanityBar();
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        sanityKiller.CurrentSanity = endSanity;
        sanityKiller.UpdateSanityBar();

        batteryRestoreCoroutine = null;
    }

    public void StartDrain()
    {
        InvokeRepeating("BatteryDrainage", Drainage, Drainage);
    }

    public void StopDrain()
    {
        CancelInvoke("BatteryDrainage");
    }

    IEnumerator RestoreBattery(float duration)
    {
        int numIncrements = (int)(duration / 1.0f / 0.25f); 

        float incrementInterval = duration / numIncrements;

        for (int i = 0; i < numIncrements; i++)
        {
            yield return new WaitForSeconds(incrementInterval); 
            BatteryPower += 0.25f;
            BatteryPower = Mathf.Clamp01(BatteryPower);
       
            if (BatteryBars != null)
            {
                BatteryBars.fillAmount = BatteryPower;
            }
        }

        BatteryPower = 1.0f;
    }

    public void OnBatteryUsed(float restoreDuration)
    {
        if (batteryRestoreCoroutine != null)
        {
            StopCoroutine(batteryRestoreCoroutine);
        }
        batteryRestoreCoroutine = StartCoroutine(RestoreBattery(restoreDuration));

        batteryRestoreCoroutine = null;
    }
}



