using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUISlots : MonoBehaviour
{
    public Button Button;
    public Image ItemIcon;
    public TextMeshProUGUI AmountText;
    public int Itemindex;
    public bool CurrentlyEquipped;

    private ItemSlot CurrentSlot;
    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        outline.enabled = CurrentlyEquipped;
    }

    public void Set(ItemSlot slot)
    {
        CurrentSlot = slot;
        ItemIcon.gameObject.SetActive(true);
        ItemIcon.sprite = slot.Item.ItemIcon;

        AmountText.text = slot.ItemQuantity > 1 ?slot.ItemQuantity.ToString() : string.Empty;
        
        if (slot.ItemQuantity > 1 ) 
        {
            AmountText.text = slot.ItemQuantity.ToString();
        }
        else 
        {
            AmountText.text = string.Empty;
        }
    
        if (outline != null) 
        {
            outline.enabled = CurrentlyEquipped;
        }
    }

    public void ClearSlot() 
    {
        CurrentSlot = null;
        ItemIcon.gameObject.SetActive(false);
        AmountText.text = string.Empty;
    }

    public void OnButtonClick() 
    {
        InventorySc.Instance.SelectItem(Itemindex);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
