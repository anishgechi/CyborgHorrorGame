using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class InventorySc : MonoBehaviour
{
    public ItemUISlots[] ItemUISlots;
    public ItemSlot[] InventorySlots;
    public GameObject InventoryMenu;
    public Transform PositionToDrop;

    [Header("Selected Items")]
    private ItemSlot ItemSelected;
    private int ItemIndex;
    private int CurrentEquipIndex;
    public TextMeshProUGUI SelectedItemName;
    public TextMeshProUGUI SelectedItemDesc;
    public GameObject UseButton;
    public GameObject DropButton;
    public GameObject EquipButton;
    public GameObject UnEquipButton;

    [Header("Events")]
    public UnityEvent InventoryOpen;
    public UnityEvent InventoryClose;

    public static InventorySc Instance;

    private Movement PlayerMovement;

    private void Awake()
    {
        Instance = this;
        PlayerMovement = GetComponent<Movement>();
    }

    // Start is called before the first frame update
    void Start()
    {
       InventoryMenu.SetActive(false);
       InventorySlots = new ItemSlot[ItemUISlots.Length];

       for (int i= 0; i < InventorySlots.Length; i++) 
       {
            InventorySlots[i] = new ItemSlot();
            ItemUISlots[i].Itemindex = i;
            ItemUISlots[i].ClearSlot();
       }
        ClearSelectedItemWindow();
    }

    public void OpenTheMenuButton()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu() 
    {
        if (InventoryMenu.activeInHierarchy) 
        {
            InventoryMenu.SetActive(false);
            InventoryClose.Invoke();
            PlayerMovement.ToggleCursor(false);
        }
        else 
        {
            InventoryMenu.SetActive(true);
            InventoryOpen.Invoke();
            ClearSelectedItemWindow ();
            PlayerMovement.ToggleCursor(true);
        }

    }

    public bool MenuIsOpen() 
    {
        return InventoryMenu.activeInHierarchy;
    }

    public void AppendItem(ItemData item) 
    {
        if (item.CanStack) 
        {
            ItemSlot SlotsToStackTo = GetItemStack(item);

            if (SlotsToStackTo != null)
            {
                SlotsToStackTo.ItemQuantity++;
                UpdateUI();
                return;
            }
        }
    
        ItemSlot EmptySlot = GetEmptySlot();

        if (EmptySlot != null)
        {
            EmptySlot.Item = item;
            EmptySlot.ItemQuantity = 1;
            UpdateUI();
            return;
        }

        ThrowItem(item);
    }

    void ThrowItem(ItemData item) 
    {
        Instantiate(item.Prefab, PositionToDrop.position, PositionToDrop.rotation);
    }

    void UpdateUI() 
    {
        for (int x = 0; x < InventorySlots.Length; x++) 
        {
            if (InventorySlots[x].Item != null) 
            {
                ItemUISlots[x].Set(InventorySlots[x]);
            }
            else 
            {
                ItemUISlots[x].ClearSlot();
            }
        
        }
    }

    ItemSlot GetItemStack (ItemData item)
    {
        for (int z = 0; z < InventorySlots.Length; z++) 
        {
            if (InventorySlots[z].Item == item && InventorySlots[z].ItemQuantity < item.MaxStack) 
            {
                return InventorySlots[z];
            }
        }

        return null;
    }

    ItemSlot GetEmptySlot() 
    {
        for (int l = 0; l < InventorySlots.Length; l++)
        {
            if (InventorySlots[l].Item == null)
            {
                return InventorySlots[l];
            }
        }

        return null;
    }

    public void SelectItem(int index) 
    {
        if (InventorySlots[index].Item == null) 
        {
            return;
        }
        
        ItemSelected = InventorySlots[index];
        CurrentEquipIndex = index;
        SelectedItemName.text = ItemSelected.Item.ItemName;
        SelectedItemDesc.text = ItemSelected.Item.ItemDesc;

        UseButton.SetActive(ItemSelected.Item.ItemsType == ItemType.Consumable);
        EquipButton.SetActive(ItemSelected.Item.ItemsType == ItemType.Equipable && !ItemUISlots[index].CurrentlyEquipped);
        UnEquipButton.SetActive(ItemSelected.Item.ItemsType == ItemType.Equipable && ItemUISlots[index].CurrentlyEquipped);
        DropButton.SetActive(true);
    }

    public void ClearSelectedItemWindow() 
    {
        ItemSelected = null;
        SelectedItemName.text = string.Empty;
        SelectedItemDesc.text = string.Empty;

        DropButton.SetActive(false);
        UseButton.SetActive(false);
        EquipButton.SetActive(false);
        UnEquipButton.SetActive(false);
    }

    public void OnUseButton() 
    {
    
    }

    public void OnEquipButton() 
    {
    
    }

    public void OnUnEquipButton() 
    {
    
    }

    public void OnDropButton() 
    {
        ThrowItem(ItemSelected.Item);
        RemoveSelectedItem();
    }

    void Unequip(int index)
    {
        
    }

    void RemoveSelectedItem() 
    {
        ItemSelected.ItemQuantity--;
        
        if (ItemSelected.ItemQuantity == 0) 
        {
            if (ItemUISlots[CurrentEquipIndex].CurrentlyEquipped == true) 
            {
                Unequip(CurrentEquipIndex);
            }
        
            ItemSelected.Item = null;
            ClearSelectedItemWindow();
        }
        UpdateUI();
    }

    public void RemoveItem(ItemData item) 
    {

    }

    public bool PlayerHasItem(ItemData item , int amount) 
    {
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        OpenTheMenuButton();
    }
}

public class ItemSlot 
{
    public ItemData Item;
    public int ItemQuantity;
}