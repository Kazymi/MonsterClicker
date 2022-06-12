using System.Collections;
using UnityEngine;
using Zenject;

public class DamageSecondDealer : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    private IPlayerDamageParametersDispenser _playerDamageParametersDispenser;
    private IPlayerMoneyParameterController _playerMoneyParameterController;

    private float _currentTime;
    private ButtonAnimatorController _buttonAnimatorController;

    [Inject]
    public void Construct(IPlayerDamageParametersDispenser playerDamageParametersDispenser,
        IPlayerMoneyParameterController playerMoneyParameterController,
        ButtonAnimatorController buttonAnimatorController)
    {
        _buttonAnimatorController = buttonAnimatorController;
        _playerDamageParametersDispenser = playerDamageParametersDispenser;
        _playerMoneyParameterController = playerMoneyParameterController;
    }

    private void FixedUpdate()
    {
        if (_currentTime < 0)
        {
            _currentTime = 1;
            GiveDamage();
        }
        else
        {
            _currentTime -= Time.fixedDeltaTime;
        }
    }

    private void GiveDamage()
    {
        if (_playerDamageParametersDispenser.GetDamageByDamageType(DamageType.Second) == 0) return;
        _playerMoneyParameterController.AddPlayerMoneyParameterByMoneyType(PlayerMoneyType.Coin, (int
            ) _playerDamageParametersDispenser.GetDamageByDamageType(DamageType.Second));
        _buttonAnimatorController.PlayButtonAnimation();
        particleSystem.Play();
    }
}