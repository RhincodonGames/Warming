using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ItemInfoDisplay : MonoBehaviour
{
    public static ItemInfoDisplay Instance { get; set; }

    public GameObject itemInfoPanel;
    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;
    public TMP_Text itemFunctionalityText;

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

    private void Start()
    {
        // Initially hide all text
        HideItemInfo();
        itemInfoPanel.SetActive(false);
    }

    // Call when hovering/clicking on an item
    public void ShowItemInfo(string itemName, string category)
    {
        // Remove "(Clone)" from the name if present
        itemName = itemName.Replace("(Clone)", "").Trim();

        // Get item data
        ItemData data = GetItemData(itemName, category);

        if (data != null)
        {
            itemNameText.text = data.name;
            itemDescriptionText.text = data.description;
            itemFunctionalityText.text = data.functionality;

            // Show all text objects
            itemNameText.gameObject.SetActive(true);
            itemDescriptionText.gameObject.SetActive(true);
            itemFunctionalityText.gameObject.SetActive(true);
            itemInfoPanel.SetActive(true);
        }
        else
        {
            HideItemInfo();
        }
    }

    public void HideItemInfo()
    {
        itemNameText.gameObject.SetActive(false);
        itemDescriptionText.gameObject.SetActive(false);
        itemFunctionalityText.gameObject.SetActive(false);
    }

    // Check if inventory is empty and display first item if available
    public void DisplayFirstAvailableItem()
    {
        // Determine which tab is currently active
        string activetab = GetActiveTab();

        if (activetab != null)
        {
            GameObject firstItem = FindFirstItemInTab(activetab);
            if (firstItem != null)
            {
                RightClickItem itemScript = firstItem.GetComponent<RightClickItem>();
                if (itemScript != null)
                {
                    ShowItemInfo(firstItem.name, itemScript.category);
                    return;
                }
            }
        }

        // No items found - hide info box
        HideItemInfo();
    }

    private string GetActiveTab()
    {
        // Check which tab GameObject is active
        if (InventorySystem.Instance.FoodTabUI.activeSelf)
            return "Food";
        else if (InventorySystem.Instance.EquipmentTabUI.activeSelf)
            return "Equipment";
        else if (InventorySystem.Instance.MaterialsTabUI.activeSelf)
            return "Materials";
        else if (InventorySystem.Instance.AbilitiesTabUI.activeSelf)
            return "Abilities";

        return null;
    }

    private GameObject FindFirstItemInTab(string category)
    {
        List<GameObject> slotList = InventorySystem.Instance.GetSlotList(category);

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                return slot.transform.GetChild(0).gameObject;
            }
        }
        
        return null;
    }

    // Database of item information
    private ItemData GetItemData(string itemName, string category)
    {
        // Food Items
        if (itemName.Contains("Berries"))
        {
            return new ItemData(
                "Berries",
                "Fresh berries from a berry bush.",
                "Restore 5 Hunger"
            );
        }
        else if (itemName.Contains("Water"))
        {
            return new ItemData(
                "Water",
                "Clean drinking water.",
                "Restore 25 Hydration"
            );
        }
        else if (itemName.Contains("Fish"))
        {
            return new ItemData(
                "Fish",
                "A freshly caught fish.",
                "Restore 15 Hunger"
            );
        }
        else if (itemName.Contains("Meat"))
        {
            return new ItemData(
                "Meat",
                "Freshly hunted seal ribs.",
                "Restore 20 Hunger"
            );
        }
        // Equipment Items
        else if (itemName.Contains("Ice Spiked Stone Helmet"))
        {
            return new ItemData(
                "Ice Spiked Stone Helmet",
                "A stone helmet reinforced with ice spikes.",
                "Damage Reduction: +13\nDamage Increased: +3"
            );
        }
        else if (itemName.Contains("Ice Spiked Wood Helmet"))
        {
            return new ItemData(
                "Ice Spiked Wood Helmet",
                "A wooden helmet reinforced with ice spikes.",
                "Damage Reduction: +8\nDamage Increased: +3"
            );
        }
        else if (itemName.Contains("Stone Helmet"))
        {
            return new ItemData(
                "Stone Helmet",
                "A basic stone helmet.",
                "Damage Reduction: +10"
            );
        }
        else if (itemName.Contains("Ice Helmet"))
        {
            return new ItemData(
                "Ice Helmet",
                "A helmet made of solid ice.",
                "Damage Reduction: +3"
            );
        }
        else if (itemName.Contains("Wood Helmet"))
        {
            return new ItemData(
                "Wood Helmet",
                "A simple wooden helmet.",
                "Damage Reduction: +5"
            );
        }
        else if (itemName.Contains("Wood"))
        {
            return new ItemData(
                "Wood",
                "A piece of wood from a tree.",
                "Crafting Material"
            );
        }
        else if (itemName.Contains("Ice Cube"))
        {
            return new ItemData(
                "Ice Cube",
                "A piece of ice from an iceberg.",
                "Carfting Material"
            );
        }
        else if (itemName.Contains("Stone"))
        {
            return new ItemData(
                "Stone",
                "A piece of stone from a boulder.",
                "Crafting Material"
            );
        }
        // Add more items here as needed

        // Default fallback
        return new ItemData(
            itemName,
            "An item in your inventory.",
            "No special function"
        );
    }

    // Helper class to store item data
    private class ItemData
    {
        public string name;
        public string description;
        public string functionality;

        public ItemData(string name, string description, string functionality)
        {
            this.name = name;
            this.description = description;
            this.functionality = functionality;
        }
    }
}