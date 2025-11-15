using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RightClickItem : MonoBehaviour, IPointerClickHandler
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
}
