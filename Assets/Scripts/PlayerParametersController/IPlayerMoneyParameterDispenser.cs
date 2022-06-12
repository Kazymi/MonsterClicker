using System;

public interface IPlayerMoneyParameterDispenser
{
    bool IsPlayerHasNeededAmountMoneyByMoneyType(PlayerMoneyType playerMoneyType, int amount);
    int GetPlayerMoneyParameterByMoneyType(PlayerMoneyType playerMoneyType);
    event Action<PlayerMoneyType> playerMoneyParameterUpdated;
}