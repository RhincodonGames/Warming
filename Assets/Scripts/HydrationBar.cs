using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HydrationBar : MonoBehaviour
{
    private Slider slider;
    public TextMeshProUGUI hydrationCounter;

    public GameObject playerState;

    private float currentHydrationPercent;
    private float maxHydrationPercent;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }


    void Update()
    {
        currentHydrationPercent = playerState.GetComponent<PlayerState>().currentHydrationPercent;
        maxHydrationPercent = playerState.GetComponent<PlayerState>().maxHydrationPercent;

        if (currentHydrationPercent > maxHydrationPercent) currentHydrationPercent = maxHydrationPercent;

        float fillValue = currentHydrationPercent / maxHydrationPercent;
        slider.value = fillValue;

        hydrationCounter.text = Mathf.RoundToInt(currentHydrationPercent) + "%";
    }
}
