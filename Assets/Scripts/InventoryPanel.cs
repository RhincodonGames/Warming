using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    public GameObject FoodTab;
    public GameObject EquipmentTab;
    public GameObject AbilitiesTab;

    public Button foodButton;
    public Button equipmentButton;
    public Button abilitiesButton;

    public PauseMenuManager isPaused;

    // Update is called once per frame
    void Update()
    {
        if (isPaused && Input.GetKeyDown(KeyCode.Alpha1))
        {
            switchFoodTab();
            EventSystem.current.SetSelectedGameObject(foodButton.gameObject);
        }
        else if (isPaused && Input.GetKeyDown(KeyCode.Alpha2))
        {
            switchEquipmentTab();
            EventSystem.current.SetSelectedGameObject(equipmentButton.gameObject);
        }
        else if (isPaused && Input.GetKeyDown(KeyCode.Alpha3))
        {
            switchAbilitiesTab();
            EventSystem.current.SetSelectedGameObject(abilitiesButton.gameObject);
        }
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
