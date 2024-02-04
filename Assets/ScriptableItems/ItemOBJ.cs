using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOBJ : MonoBehaviour, IInteractable
{
    public ItemData ItemDataSelected;

    public string GetInteractionPrompt()
    {
        return string.Format("PickUp {0}", ItemDataSelected.ItemName);
    }

    public void OnInteraction()
    {
        Destroy(gameObject);
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
