using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    public GameObject FoodTab;
    public GameObject EquipmentTab;
    public GameObject AbilitiesTab;

    public PauseMenuManager isPaused;

    // Update is called once per frame
    void Update()
    {
        if (isPaused && Input.GetKeyDown(KeyCode.Alpha1))
            switchFoodTab();
        else if (isPaused && Input.GetKeyDown(KeyCode.Alpha2))
            switchEquipmentTab();
        else if (isPaused && Input.GetKeyDown(KeyCode.Alpha3))
            switchAbilitiesTab();
    }

    public void switchFoodTab()
    {
        FoodTab.SetActive(true);
        EquipmentTab.SetActive(false);
        AbilitiesTab.SetActive(false);
    }

    public void switchEquipmentTab()
    {
        EquipmentTab.SetActive(true);
        AbilitiesTab.SetActive(false);
        FoodTab.SetActive(false);
    }

    public void switchAbilitiesTab()
    {
        AbilitiesTab.SetActive(true);
        FoodTab.SetActive(false);
        EquipmentTab.SetActive(false);
    }
}
