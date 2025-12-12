using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StaminaBar : MonoBehaviour
{
    private Slider slider;
    public TextMeshProUGUI staminaCounter;

    public GameObject playerState;

    private float currentStamina;
    private float maxStamina;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }


    void Update()
    {
        currentStamina = playerState.GetComponent<PlayerState>().currentStamina;
        maxStamina = playerState.GetComponent<PlayerState>().maxStamina;

        if (currentStamina > maxStamina) currentStamina = maxStamina;

        float fillValue = currentStamina / maxStamina;
        slider.value = fillValue;

        staminaCounter.text = Mathf.RoundToInt(currentStamina) + "%";
    }
}
