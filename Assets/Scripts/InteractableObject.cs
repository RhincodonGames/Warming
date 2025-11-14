using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string ItemName;

    public string GetItemName()
    {
        return ItemName;
    }

    //can make virtual void to ovveride the method in a child class later
    public void Interact()
    {
        //Debug.Log("Interacted with: " + ItemName);
        Debug.Log(ItemName + "placed in inventory");
        Destroy(gameObject);
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
