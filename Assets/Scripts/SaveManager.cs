using System;
using System.Collections;
using System.Collections.Generic;
using EventBusSystem;
using UnityEditor;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private float saveTime;

    private ShopParameters _shopParameters;
    private MoneySaveDate _moneySaveDate;
    private bool _isLoaded;

    private void Start()
    {
        Load();
        StartCoroutine(Save());
    }

    private IEnumerator Save()
    {
        while (true)
        {
            yield return new WaitForSeconds(saveTime);
            if (_isLoaded == false) continue;
            Save(_moneySaveDate);
            Save(_shopParameters);
        }
    }

    private void Load()
    {
        foreach (SaveDateNameType saveName in Enum.GetValues(typeof(SaveDateNameType)))
        {
            var savePath = Application.dataPath + $"/{saveName.ToString()}.txt";
            switch (saveName)
            {
                case SaveDateNameType.MoneySaveDate:
                    var saveDataMoney = TryToLoad<MoneySaveDate>(savePath);
                    if (saveDataMoney == null)
                    {
                        _moneySaveDate = new MoneySaveDate();
                        FirstInitialize(_moneySaveDate);
                    }
                    else
                    {
                        _moneySaveDate = (MoneySaveDate) saveDataMoney;
                    }

                    break;
                case SaveDateNameType.ShopSaveData:
                    var saveDataShop = TryToLoad<ShopParameters>(savePath);
                    if (saveDataShop == null)
                    {
                        _shopParameters = new ShopParameters();
                        FirstInitialize(_shopParameters);
                    }
                    else
                    {
                        _shopParameters = (ShopParameters) saveDataShop;
                    }

                    break;
            }
        }

        _isLoaded = true;
    }

    private void FirstInitialize(ISavable savable)
    {
        savable.FirstInitialize();
        savable.Initialize();
        savable.LoadFinished();
    }

    private void Save<T>(T saveData) where T : ISavable
    {
        Debug.LogWarning(Application.dataPath);
        var savePath = Application.dataPath + $"/{saveData.SaveFileName}.txt";
        Debug.Log(saveData.SaveFileName + " try to save");
        saveData.DeInitialize();
        var saveString = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(savePath, saveString);
        PlayerPrefs.Save();
        Debug.Log(saveData.SaveFileName + " was saved");
    }

    private object TryToLoad<T>(string fileName) where T : ISavable
    {
        var loadString = PlayerPrefs.GetString(fileName);
        var saveData = JsonUtility.FromJson<T>(loadString);
        if (saveData == null)
        {
            return null;
        }

        saveData.Initialize();
        saveData.LoadFinished();
        Debug.Log(fileName + " was loaded");
        return saveData;
    }
}

public interface ISavable
{
    public string SaveFileName { get; }
    public void FirstInitialize();
    public void Initialize();
    public void DeInitialize();
    public void LoadFinished();
}

public enum SaveDateNameType
{
    MoneySaveDate,
    ShopSaveData
}