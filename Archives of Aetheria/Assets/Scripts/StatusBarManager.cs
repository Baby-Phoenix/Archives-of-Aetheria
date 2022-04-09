using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarManager : MonoBehaviour
{
    public float health, maxHealth;
    private Slider slider;
    [SerializeField] private Image fill;
    [SerializeField] private Gradient gradient;
    [SerializeField] private bool isAbove = false;


    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    public void SetMaxHealth(float max)
    {
        health = max;
        maxHealth = max;
        fill.color = gradient.Evaluate(1f);
    }

    public void UpdateHealth()
    {
        float value = Mathf.Clamp(health / maxHealth, 0, 1f);
        slider.value = value;
        fill.color = gradient.Evaluate(value);
    }
    private void LateUpdate()
    {
        if (isAbove)
        {
            transform.LookAt(transform.position + GameObject.Find("Main Camera").transform.forward);
        }
    }
}


