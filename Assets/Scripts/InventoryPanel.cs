using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    public GameObject FoodTab;
    public GameObject EquipmentTab;
    public GameObject MaterialsTab;
    public GameObject AbilitiesTab;

    public Button foodButton;
    public Button equipmentButton;
    public Button materialsButton;
    public Button abilitiesButton;

    public PauseMenuManager isPaused;

    private void Start()
    {
        switchFoodTab();
    }

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
            switchMaterialsTab();
            EventSystem.current.SetSelectedGameObject(materialsButton.gameObject);
        }
        else if (isPaused && Input.GetKeyDown(KeyCode.Alpha4))
        {
            switchAbilitiesTab();
            EventSystem.current.SetSelectedGameObject(abilitiesButton.gameObject);
        }
    }

    public void switchFoodTab()
    {
        FoodTab.SetActive(true);
        EquipmentTab.SetActive(false);
        MaterialsTab.SetActive(false);
        AbilitiesTab.SetActive(false);
    }

    public void switchEquipmentTab()
    {
        EquipmentTab.SetActive(true);
        MaterialsTab.SetActive(false);
        AbilitiesTab.SetActive(false);
        FoodTab.SetActive(false);
    }
    public void switchMaterialsTab()
    {
        MaterialsTab.SetActive(true);
        AbilitiesTab.SetActive(false);
        FoodTab.SetActive(false);
        EquipmentTab.SetActive(false);
    }

    public void switchAbilitiesTab()
    {
        AbilitiesTab.SetActive(true);
        FoodTab.SetActive(false);
        EquipmentTab.SetActive(false);
        MaterialsTab.SetActive(false);
    }
}
