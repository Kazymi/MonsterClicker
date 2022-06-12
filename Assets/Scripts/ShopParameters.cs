using System;
using System.Collections.Generic;
using EventBusSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ShopParameters : ISavable
{
    public Dictionary<ShopType, int> ShopLevels = new Dictionary<ShopType, int>();
    public List<int> SavedShopLevels;

    public string SaveFileName => SaveDateNameType.ShopSaveData.ToString();

    public void FirstInitialize()
    {
        SavedShopLevels = new List<int>();
        foreach (ShopType shopType in Enum.GetValues(typeof(ShopType)))
        {
            SavedShopLevels.Add(1);
        }
    }

    public void Initialize()
    {
        var id = 0;
        ShopLevels = new Dictionary<ShopType, int>();
        foreach (ShopType shopType in Enum.GetValues(typeof(ShopType)))
        {
            ShopLevels.Add(shopType, SavedShopLevels[id]);
            id++;
        }
    }

    public void DeInitialize()
    {
        var id = 0;
        foreach (ShopType shopType in Enum.GetValues(typeof(ShopType)))
        {
            SavedShopLevels[id] = ShopLevels[shopType];
            id++;
        }
    }

    public void LoadFinished()
    {
        EventBus.RaiseEvent<ShopEvent>(h => h.LoadSuccess(this));
    }

    public int GetShopLevel(ShopType shopType)
    {
        return ShopLevels[shopType];
    }

    public void AddShopLVl(ShopType shopType)
    {
        ShopLevels[shopType]++;
    }
}