using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RightClickItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public string category;     //set in prefab already

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            //open consumption panel
            ItemConsumptionPanel.Instance.OpenPanel(gameObject.name.Replace("(Clone)", ""), category, gameObject);
        }
    }

    // Show item info when hovering over the item
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ItemInfoDisplay.Instance != null)
        {
            ItemInfoDisplay.Instance.ShowItemInfo(gameObject.name, category);
        }
    }
}
