using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MoneyTextController : MonoBehaviour
{
    [SerializeField] private PlayerMoneyType playerMoneyType;
    [SerializeField] private TextController textController;

    private IPlayerMoneyParameterDispenser _playerMoneyParameterDispenser;

    [Inject]
    private void Construct(IPlayerMoneyParameterDispenser playerMoneyParameterDispenser)
    {
        _playerMoneyParameterDispenser = playerMoneyParameterDispenser;
    }

    private void Start()
    {
        UpdateText(playerMoneyType);
    }

    private void OnEnable()
    {
        _playerMoneyParameterDispenser.playerMoneyParameterUpdated += UpdateText;
        UpdateText(playerMoneyType);
    }

    private void OnDisable()
    {
        _playerMoneyParameterDispenser.playerMoneyParameterUpdated -= UpdateText;
    }

    private void UpdateText(PlayerMoneyType playerMoneyType)
    {
        if (playerMoneyType == this.playerMoneyType)
        {
            textController.UpdateText(_playerMoneyParameterDispenser.GetPlayerMoneyParameterByMoneyType(playerMoneyType).ToString());
        }
    }
}
