using System;
using System.Collections;
using System.Collections.Generic;
using EventBusSystem;
using UnityEngine;
using Zenject;

public class TimeDamageDealer : MonoBehaviour, ShopEvent, MoneySaveEvent
{
    private MoneySaveDate _moneySaveDate;
    private IPlayerMoneyParameterController _playerMoneyParameterController;
    private IPlayerDamageParametersDispenser _playerDamageParametersDispenser;

    [Inject]
    private void Construct(IPlayerMoneyParameterController playerMoneyParameterController,
        IPlayerDamageParametersDispenser playerDamageParametersDispenser)
    {
        _playerDamageParametersDispenser = playerDamageParametersDispenser;
        _playerMoneyParameterController = playerMoneyParameterController;
    }

    private void OnEnable()
    {
        EventBus.Subscribe(this);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(this);
    }

    public void LoadSuccess(ShopParameters shopParameters)
    {
        AddTimeMoney();
    }

    public void LoadFinish(MoneySaveDate moneySaveDate)
    {
        _moneySaveDate = moneySaveDate;
    }

    private void AddTimeMoney()
    {
        Debug.LogWarning(_moneySaveDate.DateTime + " " + DateTime.Now);
        var diffInSeconds = System.Math.Abs((DateTime.Now - _moneySaveDate.DateTime).Seconds);
        Debug.LogWarning(diffInSeconds);
        _playerMoneyParameterController.AddPlayerMoneyParameterByMoneyType(PlayerMoneyType.Coin,
            (int) (diffInSeconds * _playerDamageParametersDispenser.GetDamageByDamageType(DamageType.Second)));
    }
}