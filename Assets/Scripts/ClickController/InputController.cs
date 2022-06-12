using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputController : MonoBehaviour, IInputController
{
    public Vector3 TouchPosition { get; private set; }
    public event Action onClickedDown;
    public event Action onClicked;

    private bool _isClickedDown;
    private Camera _cameraMain;

    private void Update()
    {
        UpdateInput();
        UpdateTouchInput();
        UpdateTouchPosition();
    }

    private void Awake()
    {
        _cameraMain = Camera.main;
        if (_cameraMain == null)
        {
            Debug.LogWarning("Camera main == null");
        }
    }

    private void OnEnable()
    {
        onClickedDown += CheckClickable;
    }

    private void OnDisable()
    {
        onClickedDown -= CheckClickable;
    }

    private void UpdateInput()
    {
        //todo pcTest
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            onClickedDown?.Invoke();
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            onClicked?.Invoke();
        }
    }

    private void UpdateTouchInput()
    {
        if (Input.touchCount != 0 && _isClickedDown == false)
        {
            _isClickedDown = true;
            onClickedDown?.Invoke();
        }
        else
        {
            if (Input.touchCount != 0)
            {
                onClicked?.Invoke();
            }
            else
            {
                _isClickedDown = false;
            }
        }
    }

    private void CheckClickable()
    {
        var ray = _cameraMain.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var raycastHit, 100.0f) == false)
        {
            return;
        }

        var clickable = raycastHit.transform.GetComponent<IClickableObject>();
        clickable?.OnClick();
    }

    private void UpdateTouchPosition()
    {
        if (_isClickedDown == false)
        {
            return;
        }

        var screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5);
        TouchPosition = _cameraMain.ScreenToWorldPoint(screenPos);
    }
}