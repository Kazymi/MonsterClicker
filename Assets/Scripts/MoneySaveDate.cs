using System;
using System.Collections.Generic;
using EventBusSystem;
using UnityEngine;

[Serializable]
public class MoneySaveDate : ISavable
{
    public List<int> saveData;
    public Dictionary<PlayerMoneyType, int> CurrentShopValue;
    public DateTime DateTime;

    public void Initialize()
    {
        CurrentShopValue = new Dictionary<PlayerMoneyType, int>();
        CurrentShopValue[PlayerMoneyType.Coin] = saveData[0];
        CurrentShopValue[PlayerMoneyType.Gem] = saveData[1];
    }

    public void DeInitialize()
    {
        saveData[0] = CurrentShopValue[PlayerMoneyType.Coin];
        saveData[1] = CurrentShopValue[PlayerMoneyType.Gem];
        DateTime = DateTime.Now;
    }

    public void LoadFinished()
    {
        EventBus.RaiseEvent<MoneySaveEvent>(h => h.LoadFinish(this));
    }

    public string SaveFileName => SaveDateNameType.MoneySaveDate.ToString();

    public void FirstInitialize()
    {
        saveData = new List<int>();
        saveData.Add(0);
        saveData.Add(0);
    }
}