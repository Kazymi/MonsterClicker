using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseCanvasByButtonController : MonoBehaviour
{
    [SerializeField] private EnableDisableType enableDisableType;
    [SerializeField] private ButtonController buttonController;
    [SerializeField] private Canvas canvas;

    private readonly Dictionary<EnableDisableType, Action> _clickActions = new Dictionary<EnableDisableType, Action>();

    private void Awake()
    {
        DictionaryInitialize();
    }

    private void OnEnable()
    {
        buttonController.onButtonClicked += OnButtonClick;
    }

    private void OnDisable()
    {
        buttonController.onButtonClicked -= OnButtonClick;
    }

    private void DictionaryInitialize()
    {
        _clickActions.Add(EnableDisableType.Enable, () => canvas.enabled = true);
        _clickActions.Add(EnableDisableType.Disable, () => canvas.enabled = false);
    }

    private void OnButtonClick()
    {
        _clickActions[enableDisableType].Invoke();
    }
}