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

    public float StaminUpTimer = 4.5f;

    public float BatteryRestoreDuration = 2.0f;
   
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

    private int currentEquipIndex = -1;

    public void OpenTheMenuButton()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleMenu();

            if (!MenuIsOpen() && currentEquipIndex != -1)
            {
                PlayerPrefs.SetInt("CurrentEquipIndex", currentEquipIndex);
                PlayerPrefs.Save();
            }
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
            ClearSelectedItemWindow();
            PlayerMovement.ToggleCursor(true);

            for (int i = 0; i < ItemUISlots.Length; i++)
            {
                if (ItemUISlots[i].CurrentlyEquipped)
                {
                    CurrentEquipIndex = i;
                    break;
                }
            }
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
        ItemIndex = index; 
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
        if (ItemSelected.Item.ItemsType == ItemType.Consumable) 
        {
            for (int v = 0; v < ItemSelected.Item.consumable.Length; v++) 
            {
                switch (ItemSelected.Item.consumable[v].Type) 
                {
                    case ConsumableType.Stamina:
                        StartCoroutine(PlayerMovement.TemporaryStaminaBoost(StaminUpTimer));
                        break;
                    case ConsumableType.Power:
                        Walkman walkman = GetComponent<Walkman>();
                        if( walkman != null) 
                        {
                            walkman.OnBatteryUsed(BatteryRestoreDuration);
                        }
                        break;
                }
            }
        }
        RemoveSelectedItem(); 
    }

    public void OnEquipButton()
    {

        if (ItemUISlots[ItemIndex].CurrentlyEquipped)
        {
            Unequip(ItemIndex);
        }
        else
        {
            if (CurrentEquipIndex != -1)
            {
                Unequip(CurrentEquipIndex);
            }

            ItemUISlots[ItemIndex].CurrentlyEquipped = true;
            CurrentEquipIndex = ItemIndex;

            EquipManager.Instance.EquipNew(ItemSelected.Item);
            UpdateUI();
            SelectItem(ItemIndex);
        }
    }

    public void OnUnEquipButton() 
    {
        Unequip(ItemIndex);
    }

    public void OnDropButton() 
    {
        ThrowItem(ItemSelected.Item);
        RemoveSelectedItem();
    }

    void Unequip(int index)
    {
        ItemUISlots[index].CurrentlyEquipped = false;
        EquipManager.Instance.UnEquipItem();
        UpdateUI();

        if(ItemIndex == index) 
        {
            SelectItem(index);
        }
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