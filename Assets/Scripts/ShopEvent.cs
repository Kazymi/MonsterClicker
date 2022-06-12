using EventBusSystem;

public interface ShopEvent : IGlobalSubscriber
{
    void LoadSuccess(ShopParameters shopParameters);
}