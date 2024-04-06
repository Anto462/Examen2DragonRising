using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public HealthController HealthController;
    public Image fill;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        int fillvalue = HealthController.currentHealth;
        slider.value = fillvalue;
    }
}
