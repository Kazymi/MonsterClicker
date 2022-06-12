using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonController : MonoBehaviour
{
    public event Action onButtonClicked;
    public Button Button => _button;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(ButtonClicked);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ButtonClicked);
    }

    private void ButtonClicked()
    {
        onButtonClicked?.Invoke();
    }
}