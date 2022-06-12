using System;
using UnityEngine;

[Serializable]
public class ShopButtonConfiguration
{
    [SerializeField] private ShopType shopType;
    [SerializeField] private ButtonController buttonController;
    [SerializeField] private TextController price;

    public ShopType ShopType => shopType;

    public ButtonController ButtonController => buttonController;

    public TextController Price => price;
}