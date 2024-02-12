using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType 
{
    Equipable,
    Consumable,
    Resource,
    Lore
}

public enum ConsumableType 
{
    Power,
    Stamina
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("ItemInfo")]
    public string ItemName;
    public string ItemDesc;
    public ItemType ItemsType;
    public Sprite ItemIcon;
    public GameObject Prefab;

    [Header("Stackable items")]
    public bool CanStack;
    public int MaxStack;


    [Header("Cosnumable")]
    public ConsumableData[] consumable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
[System.Serializable]
public class ConsumableData 
{
    public ConsumableType Type;
    public float value;
}