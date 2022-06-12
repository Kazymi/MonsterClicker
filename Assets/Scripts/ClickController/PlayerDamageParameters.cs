using System;
using System.Collections;
using System.Collections.Generic;
using EventBusSystem;
using UnityEngine;

public class PlayerDamageParameters : IPlayerDamageController, IPlayerDamageParametersDispenser, ShopEvent
{
    private readonly PlayerDamageConfiguration _playerDamageConfiguration;

    private readonly Dictionary<DamageType, float>
        _damageParameters = new Dictionary<DamageType, float>(); //todo add saveLoadSystem

    public PlayerDamageParameters(PlayerDamageConfiguration playerDamageConfiguration)
    {
        _playerDamageConfiguration = playerDamageConfiguration;
        EventBus.Subscribe(this);
        foreach (DamageType damageType in Enum.GetValues(typeof(DamageType)))
        {
            _damageParameters.Add(damageType, 0);
        }
    }

    ~PlayerDamageParameters()
    {
        EventBus.Unsubscribe(this);
    }

    public float GetDamageByDamageType(DamageType damageType)
    {
        return _damageParameters[damageType];
    }

    public void ReduceDamageByDamageType(DamageType damageType, float reduceValue)
    {
        _damageParameters[damageType] -= reduceValue;
    }

    public void LoadSuccess(ShopParameters shopParameters)
    {
        foreach (var shopLevel in shopParameters.ShopLevels)
        {
            switch (shopLevel.Key)
            {
                case ShopType.Click:
                    if (shopLevel.Value == 1)
                    {
                        _damageParameters[DamageType.Click] = 1;
                        continue;
                    }
                    _damageParameters[DamageType.Click] =
                        1 * (shopLevel.Value * _playerDamageConfiguration.AddClickDamageAfterLvlUp);
                    break;
                case ShopType.AutoClick:
                    if (shopLevel.Value == 1) continue;
                    _damageParameters[DamageType.Second] =
                        shopLevel.Value * _playerDamageConfiguration.AddAutoClickDamageAfterLvlUp;
                    break;
                case ShopType.Splash:
                    if (shopLevel.Value == 1) continue;
                    break;
            }
        }
    }

    public void AddDamage(DamageType damageType)
    {
        switch (damageType)
        {
            case DamageType.Click:
                _damageParameters[DamageType.Click] += _playerDamageConfiguration.AddClickDamageAfterLvlUp; 
                break;
            case DamageType.Splash:
                _damageParameters[DamageType.Splash] += _playerDamageConfiguration.AddAutoClickDamageAfterLvlUp; 
                break;
            case DamageType.Second:
                _damageParameters[DamageType.Second] += _playerDamageConfiguration.AddAutoClickDamageAfterLvlUp; 
                break;
        }
    }
}