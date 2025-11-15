using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCraftingRecipe", menuName = "Crafting/Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public string itemName;     //name of crafted item
    public string category;     //Inventory category
    public GameObject prefab;   //Prefab to spawn in inventory
    public List<MaterialRequirement> materialsRequired;     //list of materials
}
