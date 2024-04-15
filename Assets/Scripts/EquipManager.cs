using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    public static EquipManager Instance;
    public Equip CurrentEquip;
    public Transform EquipParent;

    public GameObject walkmanOverlay;
    public bool isWalkmanEquipped = false;

    public bool IsWalkmanEquipped => isWalkmanEquipped;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EquipItem(ItemData item)
    {
        UnEquipItem();
        CurrentEquip = Instantiate(item.EquipPrefab, EquipParent).GetComponent<Equip>();
        UpdateWalkmanState();
    }

    public void EquipNew(ItemData item)
    {
        UnEquipItem();
        CurrentEquip = Instantiate(item.EquipPrefab, EquipParent).GetComponent<Equip>();
        UpdateWalkmanState();
    }

    public void UnEquipItem()
    {
        if (CurrentEquip != null)
        {
            if (CurrentEquip.gameObject.CompareTag("Walkman"))
            {
                isWalkmanEquipped = false;
                walkmanOverlay.SetActive(false);
            }
            Destroy(CurrentEquip.gameObject);
            CurrentEquip = null;
        }
    }

    void UpdateWalkmanState()
    {
        if (CurrentEquip != null && CurrentEquip.gameObject.CompareTag("Walkman"))
        {
            isWalkmanEquipped = true;
            walkmanOverlay.SetActive(true);
        }
        else
        {
            isWalkmanEquipped = false;
            walkmanOverlay.SetActive(false);
        }
    }
}




