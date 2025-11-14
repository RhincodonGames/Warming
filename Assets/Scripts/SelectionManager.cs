using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class SelectionManager : MonoBehaviour
{
    public PauseMenuManager pauseMenuManager;

    public GameObject interactionInfoUI;
    TextMeshProUGUI interactionText;

    public float interactionDistance = 5f;
    public Transform player;

    private Camera cam;
    private InteractableObject currentTarget;

    void Start()
    {
        cam = Camera.main;
        interactionText = interactionInfoUI.GetComponent<TextMeshProUGUI>();
        interactionInfoUI.SetActive(false);
    }


    void Update()
    {
        if (!pauseMenuManager.isPaused)
        {
            UpdateHover();
            HandleInteraction();
        }
    }

    private void UpdateHover()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            InteractableObject interactable = hit.transform.GetComponentInParent<InteractableObject>();
            if (interactable == null)
            {
                interactable = hit.transform.GetComponentInChildren<InteractableObject>();
            }

            if (interactable != null)
            {   
                float distance = Vector3.Distance(player.position, interactable.transform.position);
                if (distance <= interactionDistance)
                {
                    currentTarget = interactable;
                    interactionText.text = interactable.GetItemName() + " (E)";

                    //UI under cursor
                    interactionInfoUI.transform.position = Input.mousePosition + new Vector3(0, -30f, 0);
                    interactionInfoUI.SetActive(true);
                    return;
                }
            }
        }

        //Hide UI if nothing is interactable or out of range
        currentTarget = null;
        interactionInfoUI.SetActive(false);
    }

    private void HandleInteraction()
    {
        if (currentTarget != null && Input.GetKeyDown(KeyCode.E))
        {
            float distance = Vector3.Distance(player.position, currentTarget.transform.position);
            if (distance <= interactionDistance)
            {
                currentTarget.Interact();
            }
        }
    }
}
