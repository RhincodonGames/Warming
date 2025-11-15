using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string ItemName;
    public string ItemCategory;

    public string GetItemName()
    {
        return ItemName;
    }

    //can make virtual void to overide the method in a child class later
    public void Interact()
    {
        //Debug.Log("Interacted with: " + ItemName);
        Debug.Log(ItemName + "placed in inventory");

        //if the inventory is NOT full
        if (!InventorySystem.Instance.CheckIfFull(ItemCategory))
        {
            InventorySystem.Instance.AddToInventory(ItemName, ItemCategory);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log(ItemCategory + "inventory slots are full");
        }
    }


    //public bool playerInRange;

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        playerInRange = true;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        playerInRange = false;
    //    }
    //}
}
