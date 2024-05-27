using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Test : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Toggle _toggle;
    [SerializeField] private Slider _slider;
    [SerializeField] private Button _button;

    private void Start()
    {
        _toggle.onValueChanged.AddListener(OnToggleValueChanged);
        _button.onClick.AddListener(ChangeText);
        _button.onClick.AddListener(ChangeColor);
    }

    private void ChangeColor()
    {
        text.color = Random.ColorHSV();
    }

    private void OnToggleValueChanged(bool arg0)
    {
        text.text = arg0.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _slider.value = Random.Range(_slider.minValue, _slider.maxValue);
        }
    }

    private void ChangeText()
    {
        text.text = Random.Range(0, 1000).ToString();
    }
}
