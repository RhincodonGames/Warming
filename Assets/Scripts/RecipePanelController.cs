using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipePanelController : MonoBehaviour
{
    public CraftingRecipe recipe;      // Assign the ScriptableObject for this recipe
    public Button craftButton;         // Button to craft this recipe
    public TMP_Text requirementsText;  // Text showing material counts

    void OnEnable()
    {
        // Update text whenever the panel becomes visible
        UpdatePanel();
    }

    void Update()
    {
        // Optional: update every frame if you want live update
        UpdatePanel();
    }

    public void UpdatePanel()
    {
        if (recipe == null || InventorySystem.Instance == null) return;

        bool canCraft = true;
        string reqText = "";

        foreach (var req in recipe.materialsRequired)
        {
            int count = InventorySystem.Instance.itemListMaterials.FindAll(x => x == req.materialName).Count;

            if (count < req.quantity)
            {
                canCraft = false;
            }

            reqText += req.materialName + " (" + count + "/" + req.quantity + ")\n";
        }

        // Update requirements text
        requirementsText.text = reqText.TrimEnd();

        // Enable/disable button based on whether player can craft
        if (canCraft)
        {
            craftButton.gameObject.SetActive(true);
            craftButton.interactable = true;
        }
        else
        {
            craftButton.gameObject.SetActive(false);
        }
    }

    public void CraftItem()
    {
        if (recipe == null || InventorySystem.Instance == null) return;

        // Remove materials
        foreach (var req in recipe.materialsRequired)
        {
            int removed = 0;
            // Go through the inventory backwards
            for (int i = InventorySystem.Instance.slotListMaterials.Count - 1; i >= 0 && removed < req.quantity; i--)
            {
                GameObject slot = InventorySystem.Instance.slotListMaterials[i];
                if (slot.transform.childCount > 0)
                {
                    GameObject itemGO = slot.transform.GetChild(0).gameObject;
                    if (itemGO.name == req.materialName + "(Clone)" || itemGO.name == req.materialName)
                    {
                        // Remove from the string list
                        InventorySystem.Instance.itemListMaterials.Remove(req.materialName);
                        // Destroy the actual GameObject in the slot
                        Destroy(itemGO);
                        removed++;
                    }
                }
            }
        }
        // Add the crafted item to inventory
        InventorySystem.Instance.AddToInventory(recipe.itemName, recipe.category);
        Debug.Log(recipe.itemName + " crafted!");
    }
}
