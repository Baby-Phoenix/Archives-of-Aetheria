using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarManager : MonoBehaviour
{
    public float unit, maxUnit;
    private Slider slider;
    [SerializeField] private Image fill;
    [SerializeField] private Gradient gradient;
    [SerializeField] private bool isAbove = false;
    public Text number;


    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
        number = gameObject.GetComponentInChildren<Text>();
    }

    public void SetMaxValue(float max)
    {
        unit = max;
        maxUnit = max;
        fill.color = gradient.Evaluate(1f);
        number.text = unit.ToString() + "/" + maxUnit.ToString();
    }

    public void UpdateValue()
    {
        float value = Mathf.Clamp(unit / maxUnit, 0, 1f);
        slider.value = value;
        fill.color = gradient.Evaluate(value);
        number.text = unit.ToString() + "/" + maxUnit.ToString();
    }
    private void LateUpdate()
    {
        if (isAbove)
        {
            transform.LookAt(transform.position + GameObject.Find("Main Camera").transform.forward);
        }
    }
}


