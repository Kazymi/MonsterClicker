using System;
using UnityEngine;

public interface IInputController
{
    Vector3 TouchPosition { get; }
    event Action onClickedDown;
    event Action onClicked;
}