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
    public Button discardButton;
    public Button cancelButton;
    public TMP_Text itemNameText;

    private string selectedItemName;
    private string selectedItemCategory;
    private GameObject selectedItemGO;

    public bool isOpen = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        panel.SetActive(false);

        eatButton.onClick.AddListener(OnEat);
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
        }
        else
        {
            eatButton.gameObject.SetActive(false);
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
        ApplyItemEffects(selectedItemName);

        // Remove item from inventory
        RemoveItemFromInventory(selectedItemName, selectedItemCategory, selectedItemGO);

        ClosePanel();
    }

    private void OnDiscard()
    {
        if (selectedItemGO == null) return;

        RemoveItemFromInventory(selectedItemName, selectedItemCategory, selectedItemGO);
        ClosePanel();
    }

    private void ApplyItemEffects(string itemName)
    {
        PlayerState player = PlayerState.Instance;
        if (player == null) return;

        //expand later with ScriptableObject database for items
        if (itemName.Contains("Berries"))
        {
            player.currentHunger += 20f;
            if (player.currentHunger > player.maxHunger) player.currentHunger = player.maxHunger;
            Debug.Log("Restore 20 Hunger");
        }
        else if (itemName.Contains("Water"))
        {
            player.currentHydrationPercent += 25f;
            if (player.currentHydrationPercent > player.maxHydrationPercent) player.currentHydrationPercent = player.maxHydrationPercent;
            Debug.Log("Restore 25 Hydration Percent");
        }
        //else if (itemName.Contains("Herbs"))
        //{
        //    player.currentHealth += 30f;
        //    if (player.currentHealth > player.maxHealth) player.currentHealth = player.maxHealth;
        //}
        // Add more items as needed
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
