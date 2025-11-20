using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; set; }

    public GameObject ItemInfoUI;

    // Food Tab Logic //
    public GameObject FoodTabUI;
    public List<GameObject> slotListFood = new List<GameObject>();
    public List<String> itemListFood = new List<String>();

    // Equipment Tab Logic //
    public GameObject EquipmentTabUI;
    public List<GameObject> slotListEquipment = new List<GameObject>();
    public List<String> itemListFoodEquipment = new List<String>();

    // Materials Tab Logic //
    public GameObject MaterialsTabUI;
    public List<GameObject> slotListMaterials = new List<GameObject>();
    public List<String> itemListMaterials = new List<String>();

    // Abilities Tab Logic //
    public GameObject AbilitiesTabUI;
    public List<GameObject> slotListAbilities = new List<GameObject>();
    public List<String> itemListAbilities = new List<String>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        PopulateSlotListFood();
        PopulateSlotListEquipment();
        PopulateSlotListMaterials();
        PopulateSlotListAbilities();
    }

    private void PopulateSlotListFood()
    {
        foreach (Transform child in FoodTabUI.transform)
        {
            if (child.CompareTag("FoodSlot"))
            {
                slotListFood.Add(child.gameObject);
            }
        }
    }

    private void PopulateSlotListEquipment()
    {
        foreach (Transform child in EquipmentTabUI.transform)
        {
            if (child.CompareTag("EquipmentSlot"))
            {
                slotListEquipment.Add(child.gameObject);
            }
        }
    }

    private void PopulateSlotListMaterials()
    {
        foreach (Transform child in MaterialsTabUI.transform)
        {
            if (child.CompareTag("MaterialsSlot"))
            {
                slotListMaterials.Add(child.gameObject);
            }
        }
    }

    private void PopulateSlotListAbilities()
    {
        foreach (Transform child in AbilitiesTabUI.transform)
        {
            if (child.CompareTag("AbilitiesSlot"))
            {
                slotListAbilities.Add(child.gameObject);
            }
        }
    }

    public void AddToInventory(string itemName, string category)
    {
        List<GameObject> slotList = GetSlotList(category);
        List<string> itemList = GetItemList(category);

        GameObject slot = FindNextEmptySlot(slotList);

        if (slot == null)
        {
            Debug.Log("No empty slots in " + category);
            return;
        }

        GameObject newItem = Instantiate(Resources.Load<GameObject>(itemName), slot.transform);
        newItem.transform.localPosition = Vector3.zero;

        itemList.Add(itemName);

        Debug.Log(itemName + " added to " + category);
    }

    public bool CheckIfFull(string category)
    {
        List<GameObject> slotList = GetSlotList(category);

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return false;
            }
        }
        return true;
    }

    private GameObject FindNextEmptySlot(List<GameObject> slotList)
    {
        foreach(GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }

        return null;
        //return new GameObject;
    }

    private List<GameObject> GetSlotList(string category)
    {
        if (category == "Food")
        {
            return slotListFood;
        }
        else if (category == "Equipment")
        {
            return slotListEquipment;
        }
        else if (category == "Materials")
        {
            return slotListMaterials;
        }
        else if (category == "Abilities")
        {
            return slotListAbilities;
        }
        else
        {
            Debug.LogWarning("Unknown category: " + category + "default to Abilities list");
            return slotListAbilities;
        }
    }

    public List<string> GetItemList(string category)
    {
        if (category == "Food")
        {
        return itemListFood;
        }
        else if (category == "Equipment")
        {
            return itemListFoodEquipment;
        }
        else if (category == "Materials")
        {
            return itemListMaterials;
        }
        else if (category == "Abilities")
        {
            return itemListAbilities;
        }
        else
        {
            Debug.LogWarning("Unknown category: " + category + " — defaulting to Materials list.");
            return itemListMaterials;
        }
    }
}
