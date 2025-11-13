using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HungerBar : MonoBehaviour
{
    private Slider slider;
    public TextMeshProUGUI hungerCounter;

    public GameObject playerState;

    private float currentHunger;
    private float maxHunger;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }


    void Update()
    {
        currentHunger = playerState.GetComponent<PlayerState>().currentHunger;
        maxHunger = playerState.GetComponent<PlayerState>().maxHunger;

        if (currentHunger > maxHunger) currentHunger = maxHunger;

        float fillValue = currentHunger / maxHunger;
        slider.value = fillValue;

        hungerCounter.text = Mathf.RoundToInt(currentHunger) + "/" + maxHunger;
    }
}
