using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ButtonClickable : MonoBehaviour, IClickableObject
{
    [SerializeField] private VFXConfiguration coinVFXConfiguration;

    private IPlayerMoneyParameterController _playerMoneyParameterController;
    private IPlayerDamageParametersDispenser _playerDamageDispenser;
    private ButtonAnimatorController _buttonAnimatorController;
    
    private IVFXSpawner _vfxSpawner;

    [Inject(Id = GameSceneTransformType.CoinSpawnPoint)]
    private Transform _coinSpawnPoint;

    private int _animatorClickHash;

    [Inject]
    private void Construct(IPlayerMoneyParameterController playerMoneyParameterController,
        IPlayerDamageParametersDispenser damageParametersDispenser, IVFXSpawner vfxSpawner,ButtonAnimatorController buttonAnimatorController)
    {
        _buttonAnimatorController = buttonAnimatorController;
        _vfxSpawner = vfxSpawner;
        _playerDamageDispenser = damageParametersDispenser;
        _playerMoneyParameterController = playerMoneyParameterController;
    }

    public void OnClick()
    {
        _playerMoneyParameterController.AddPlayerMoneyParameterByMoneyType(PlayerMoneyType.Coin,
            (int) _playerDamageDispenser.GetDamageByDamageType(DamageType.Click));
        _vfxSpawner.GetEffectByVFXConfiguration(coinVFXConfiguration).position = _coinSpawnPoint.position;
        _buttonAnimatorController.PlayButtonAnimation();
    }
}

public class ButtonAnimatorController
{
    private const string ClickAnimationName = "OnClick";
    private readonly int _animatorClickHash;
    private readonly Animator _animator;

    public ButtonAnimatorController(Animator animator)
    {
        _animator = animator;
        _animatorClickHash = Animator.StringToHash(ClickAnimationName);
    }

    public void PlayButtonAnimation()
    {
        _animator.SetTrigger(_animatorClickHash);
    }
}