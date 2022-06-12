using System;
using System.Collections.Generic;
using EventBusSystem;

public class PlayerMoneyParametersController : IPlayerMoneyParameterController, IPlayerMoneyParameterDispenser,MoneySaveEvent
{
    private MoneySaveDate _moneySaveDate;
    public event Action<PlayerMoneyType> playerMoneyParameterUpdated;

    public PlayerMoneyParametersController()
    {
        EventBus.Subscribe(this);
    }

    ~PlayerMoneyParametersController()
    {
        EventBus.Unsubscribe(this);
    }
    public int GetPlayerMoneyParameterByMoneyType(PlayerMoneyType playerMoneyType)
    {
        if (_moneySaveDate == null) return 0;
        return  _moneySaveDate.CurrentShopValue[playerMoneyType];
    }

    public void AddPlayerMoneyParameterByMoneyType(PlayerMoneyType playerMoneyType, int addValue)
    {
        _moneySaveDate.CurrentShopValue[playerMoneyType] += addValue;
        playerMoneyParameterUpdated?.Invoke(playerMoneyType);
    }

    public void ReducePlayerMoneyParameterByMoneyType(PlayerMoneyType playerMoneyType, int reduceValue)
    {
        _moneySaveDate.CurrentShopValue[playerMoneyType] -= reduceValue;
        playerMoneyParameterUpdated?.Invoke(playerMoneyType);
    }

    public bool IsPlayerHasNeededAmountMoneyByMoneyType(PlayerMoneyType playerMoneyType, int amount)
    {
        return _moneySaveDate.CurrentShopValue[playerMoneyType] >= amount;
    }

    public void LoadFinish(MoneySaveDate moneySaveDate)
    {
        _moneySaveDate = moneySaveDate;
    }
}