using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    public static EquipManager Instance;
    public Equip CurrentEquip;
    public Transform EquipParent;

    public GameObject walkmanOverlay;
    private bool isWalkmanEquipped = false;

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
        NotifyEquippedItem(CurrentEquip.gameObject.tag);
        if (CurrentEquip.gameObject.CompareTag("Walkman"))
        {
            isWalkmanEquipped = true;
            walkmanOverlay.SetActive(true);
        }
    }

    public void EquipNew(ItemData item)
    {
        UnEquipItem();
        CurrentEquip = Instantiate(item.EquipPrefab, EquipParent).GetComponent<Equip>();
        NotifyEquippedItem(CurrentEquip.gameObject.tag);
        if (CurrentEquip.gameObject.CompareTag("Walkman"))
        {
            isWalkmanEquipped = true;
            walkmanOverlay.SetActive(true);
        }
    }

    public void UnEquipItem()
    {
        if (CurrentEquip != null)
        {
            NotifyUnequippedItem(CurrentEquip.gameObject.tag);
            if (CurrentEquip.gameObject.CompareTag("Walkman"))
            {
                isWalkmanEquipped = false;
                walkmanOverlay.SetActive(false);
            }
            Destroy(CurrentEquip.gameObject);
            CurrentEquip = null;
        }
    }

    void NotifyEquippedItem(string tag)
    {
        Debug.Log("Equipped item tag: " + tag);
    }

    void NotifyUnequippedItem(string tag)
    {
        Debug.Log("Unequipped item tag: " + tag);
    }
}



