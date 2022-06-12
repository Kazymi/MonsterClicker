public interface IPlayerMoneyParameterController
{
    void AddPlayerMoneyParameterByMoneyType(PlayerMoneyType playerMoneyType, int addValue);
    void ReducePlayerMoneyParameterByMoneyType(PlayerMoneyType playerMoneyType, int reduceValue);
}