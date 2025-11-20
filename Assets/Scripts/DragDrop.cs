using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public static GameObject itemBeingDragged;
    public static bool droppedOnSlot;

    public bool isBeingDragged = false;

    Vector3 startPosition;
    Transform startParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isBeingDragged = true;
        droppedOnSlot = false;
        itemBeingDragged = gameObject;

        Debug.Log("OnBeginDrag");

        startPosition = transform.position;
        startParent = transform.parent;

        //Makes transparent - unesscary!!!!!
        //took me 3 hours to find this bug 
        //the whole time I thought my items being dragged were on the wrong hierarchy layer
        //canvasGroup.alpha = 0.9f;
        canvasGroup.blocksRaycasts = false;


        transform.SetParent(GetComponentInParent<Canvas>().transform);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        //so the item will move with the mouse (at same speed)
        //and so it will be consistent if canvas is a diff. scale than 1
        //rectTransform.anchoredPosition += eventData.delta;
        rectTransform.position = eventData.position;
        
        isBeingDragged = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeingDragged = null;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (!droppedOnSlot)
        {
            transform.position = startPosition;
            transform.SetParent(startParent, true);
        }

        Debug.Log("OnEndDrag");

        isBeingDragged = false;
    }
}

