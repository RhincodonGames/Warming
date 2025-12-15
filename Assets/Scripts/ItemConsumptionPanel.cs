using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemConsumptionPanel : MonoBehaviour
{
    public static ItemConsumptionPanel Instance;

    // UI
    public GameObject panel;          // The panel itself
    public Button eatButton;
    public Button equipButton;
    public Button discardButton;
    public Button cancelButton;
    public TMP_Text itemNameText;

    private string selectedItemName;
    private string selectedItemCategory;
    private GameObject selectedItemGO;

    public bool isOpen = false;

    PlayerCombat playerCombat;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        panel.SetActive(false);

        eatButton.onClick.AddListener(OnEat);
        equipButton.onClick.AddListener(OnEquip);
        discardButton.onClick.AddListener(OnDiscard);
        cancelButton.onClick.AddListener(ClosePanel);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isOpen)
        {
            ClosePanel();
        }
    }

    // Opens the panel for a specific item
    public void OpenPanel(string itemName, string category, GameObject itemGO)
    {
        isOpen = true;
        selectedItemName = itemName;
        selectedItemCategory = category;
        selectedItemGO = itemGO;

        itemNameText.text = itemName;

        // Only food items can be eaten
        if (category == "Food")
        {
            eatButton.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(false);
        }
        else if (category == "Equipment")
        {
            equipButton.gameObject.SetActive(true);
            eatButton.gameObject.SetActive(false);

            // Set Equip/Unequip Text
            HelmetType itemHelmet = GetHelmetTypeFromItem(itemName);
            HelmetType equippedHelmet = PlayerCombat.Instance.GetEquippedHelmet();

            TMP_Text equipButtonText = equipButton.GetComponentInChildren<TMP_Text>();

            if (itemHelmet == equippedHelmet && equippedHelmet != HelmetType.None)
            {
                equipButtonText.text = "UNEQUIP";
            }
            else
                equipButtonText.text = "EQUIP";
        }
        else
        {
            eatButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);
        }

        panel.SetActive(true);
    }

    private void ClosePanel()
    {   
        isOpen = false;
        selectedItemName = null;
        selectedItemCategory = null;
        selectedItemGO = null;
        panel.SetActive(false);
    }

    private void OnEat()
    {
        if (selectedItemGO == null) return;

        // Apply stats based on item type
        ApplyItemEffectsConsumable(selectedItemName);

        // Remove item from inventory
        RemoveItemFromInventory(selectedItemName, selectedItemCategory, selectedItemGO);

        ClosePanel();
    }

    private void OnEquip()
    {
        if (selectedItemGO == null) return;

        PlayerCombat playerCombat = PlayerCombat.Instance;
        HelmetType itemHelmet = GetHelmetTypeFromItem(selectedItemName);

        if (playerCombat.GetEquippedHelmet() == itemHelmet)
        {
            // Unequip
            playerCombat.UnequipHelmet();
            Debug.Log("Helmet unequipped");
        }
        else
        {
            // Equip
            playerCombat.EquipHelmet(itemHelmet);
            Debug.Log("Equipped helmet: " + itemHelmet);
        }

        ClosePanel();
    }

    private void OnDiscard()
    {
        if (selectedItemGO == null) return;

        RemoveItemFromInventory(selectedItemName, selectedItemCategory, selectedItemGO);
        ClosePanel();
    }

    private void ApplyItemEffectsConsumable(string itemName)
    {
        PlayerState player = PlayerState.Instance;
        if (player == null) return;

        //expand later with ScriptableObject database for items
        if (itemName.Contains("Berries"))
        {
            player.currentHunger += 5f;
            if (player.currentHunger > player.maxHunger) player.currentHunger = player.maxHunger;
            Debug.Log("Restore 5 Hunger");
        }
        else if (itemName.Contains("Water"))
        {
            player.currentHydrationPercent += 25f;
            if (player.currentHydrationPercent > player.maxHydrationPercent) player.currentHydrationPercent = player.maxHydrationPercent;
            Debug.Log("Restore 25 Hydration Percent");
        }
        else if (itemName.Contains("Fish"))
        {
            player.currentHunger += 15f;
            if (player.currentHunger > player.maxHunger) player.currentHunger = player.maxHunger;
            Debug.Log("Restore 15 Hunger");
        }
        else if (itemName.Contains("Meat"))
        {
            player.currentHunger += 20f;
            if (player.currentHunger > player.maxHunger) player.currentHunger = player.maxHunger;
            Debug.Log("Restore 20 Hunger");
        }
        //else if (itemName.Contains("Herbs"))
        //{
        //    player.currentHealth += 30f;
        //    if (player.currentHealth > player.maxHealth) player.currentHealth = player.maxHealth;
        //}
        // Add more items as needed
    }

    private HelmetType GetHelmetTypeFromItem(string itemName)
    {
        if (itemName.Contains("Ice Spiked Stone Helmet")) 
            return HelmetType.IceSpikedStone;
        if (itemName.Contains("Ice Spiked Wood Helmet")) 
            return HelmetType.IceSpikedWood;
        if (itemName.Contains("Stone Helmet")) 
            return HelmetType.Stone;
        if (itemName.Contains("Ice Helmet")) 
            return HelmetType.Ice;
        if (itemName.Contains("Wood Helmet")) 
            return HelmetType.Wood;

        return HelmetType.None;
    }

    private void RemoveItemFromInventory(string itemName, string category, GameObject itemGO)
    {
        // Remove from string list
        List<string> itemList = InventorySystem.Instance.GetItemList(category);
        if (itemList.Contains(itemName))
        {
            itemList.Remove(itemName);
        }

        // Destroy the actual item GameObject
        Destroy(itemGO);
    }
}
