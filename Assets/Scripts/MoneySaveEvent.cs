using EventBusSystem;

public interface MoneySaveEvent : IGlobalSubscriber
{
    void LoadFinish(MoneySaveDate moneySaveDate);
}