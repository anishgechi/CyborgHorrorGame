using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    public static EquipManager Instance;
    public Equip CurrentEquip;
    public Transform EquipParent;

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
    }


    public void EquipNew(ItemData item)
    {
        UnEquipItem();
        CurrentEquip = Instantiate(item.EquipPrefab, EquipParent).GetComponent<Equip>();
    }

    public void UnEquipItem() 
    {
        if (CurrentEquip != null) 
        {
            Destroy(CurrentEquip.gameObject);
            CurrentEquip = null;
        }
    }
}

