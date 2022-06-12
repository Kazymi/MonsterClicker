using System;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDeActivator : MonoBehaviour
{
    [SerializeField] private List<GameObject> deactivateObjects;
    [SerializeField] private ButtonController buttonController;
    [SerializeField] private EnableDisableType enableDisableType;
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
        _clickActions.Add(EnableDisableType.Enable, () =>
        {
            foreach (var deactivateObject in deactivateObjects)
            {
                deactivateObject.SetActive(true);
            }
        });
        _clickActions.Add(EnableDisableType.Disable, () =>
        {
            foreach (var deactivateObject in deactivateObjects)
            {
                deactivateObject.SetActive(false);
            }
        });
    }

    private void OnButtonClick()
    {
        _clickActions[enableDisableType].Invoke();
    }
}