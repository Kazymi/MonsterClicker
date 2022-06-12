using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(ParticleSystem))]
public class CoinVFXController : TemporaryMonoPooled
{
    private float _disableTime;
    private ParticleSystem _particleSystem;

    private IPlayerDamageParametersDispenser _playerDamageParametersDispenser;
    
    private const int MaxAmountCoin = 10;

    [Inject]
    private void Construct(IPlayerDamageParametersDispenser playerDamageParametersDispenser)
    {
        _playerDamageParametersDispenser = playerDamageParametersDispenser;
    }
    
    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        Timer =  _particleSystem.duration;
    }

    public override void Initialize()
    {
        base.Initialize();
        var currentAmountCoin = _playerDamageParametersDispenser.GetDamageByDamageType(DamageType.Click);
        if (currentAmountCoin > MaxAmountCoin)
        {
            currentAmountCoin = MaxAmountCoin;
        }

        var particleSystemEmission = _particleSystem.emission;
        particleSystemEmission.burstCount = (int) currentAmountCoin;
        _particleSystem.Play();
    }
}
