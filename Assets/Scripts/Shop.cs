using System;
using System.Collections.Generic;
using EventBusSystem;
using UnityEngine;
using Zenject;

public class Shop : MonoBehaviour, ShopEvent
{
    [SerializeField] private ShopConfiguration shopConfiguration;
    [SerializeField] private List<ShopButtonConfiguration> shopButtonConfigurations;

    private ShopParameters _shopParameters;
    private IPlayerMoneyParameterDispenser _playerMoneyParameterDispenser;
    private IPlayerMoneyParameterController _playerMoneyParameterController;
    private IPlayerDamageController _playerDamageController;

    private readonly Dictionary<ShopType, IDamageUpgrader> _damageUpgraders = new Dictionary<ShopType, IDamageUpgrader>();
    public Dictionary<ShopType, float> CurrentPrices { get; } = new Dictionary<ShopType, float>();

    [Inject]
    private void Construct(IPlayerMoneyParameterDispenser playerMoneyParameterDispenser,
        IPlayerMoneyParameterController playerMoneyParameterController, IPlayerDamageController playerDamageController)
    {
        _playerDamageController = playerDamageController;
        _playerMoneyParameterController = playerMoneyParameterController;
        _playerMoneyParameterDispenser = playerMoneyParameterDispenser;
    }

    private void OnEnable()
    {
        _playerMoneyParameterDispenser.playerMoneyParameterUpdated += MoneyUpdated;
        EventBus.Subscribe(this);
    }

    private void OnDisable()
    {
        _playerMoneyParameterDispenser.playerMoneyParameterUpdated -= MoneyUpdated;
        EventBus.Unsubscribe(this);
    }

    private void OnDestroy()
    {
        UnsubscribeButtons();
    }

    private void InitializeUpgrader(IPlayerMoneyParameterController playerMoneyParameterController,
        IPlayerDamageController playerDamageController)
    {
        _damageUpgraders.Add(ShopType.Click,
            new ClickDamageUpgrader());
        _damageUpgraders.Add(ShopType.AutoClick, new AutoClickDamageUpgrader());
        foreach (var damageUpgrader in _damageUpgraders)
        {
            damageUpgrader.Value.Initialize(this, playerMoneyParameterController, _playerMoneyParameterDispenser,
                _shopParameters, playerDamageController);
        }
    }

    public void LoadSuccess(ShopParameters shopParameters)
    {
        _shopParameters = shopParameters;
        RecalculatePrices();
        InitializeUpgrader(_playerMoneyParameterController, _playerDamageController);
        SubscribeButtons();
    }

    public void RecalculatePrices()
    {
        foreach (var shopButtonConfiguration in shopButtonConfigurations)
        {
            switch (shopButtonConfiguration.ShopType)
            {
                case ShopType.Click:
                    var currentPriceClick = shopConfiguration.ClickPrice * (shopConfiguration.AddLvlPercentClick *
                                                                            _shopParameters
                                                                                .GetShopLevel(ShopType.Click));
                    shopButtonConfiguration.Price.UpdateText(currentPriceClick.ToString());
                    if (CurrentPrices.ContainsKey(ShopType.Click) == false)
                    {
                        CurrentPrices.Add(ShopType.Click, currentPriceClick);
                    }
                    else
                    {
                        CurrentPrices[ShopType.Click] = currentPriceClick;
                    }

                    break;
                case ShopType.AutoClick:
                    var currentPriceAutoClick = shopConfiguration.AutoClickPrice *
                                                (shopConfiguration.AddLvlPercentAutoClick *
                                                 _shopParameters.GetShopLevel(ShopType.AutoClick));
                    shopButtonConfiguration.Price.UpdateText(currentPriceAutoClick.ToString());
                    if (CurrentPrices.ContainsKey(ShopType.AutoClick) == false)
                    {
                        CurrentPrices.Add(ShopType.AutoClick, currentPriceAutoClick);
                    }
                    else
                    {
                        CurrentPrices[ShopType.AutoClick] = currentPriceAutoClick;
                    }

                    break;
                case ShopType.Splash:
                    var currentPriceSplash = shopConfiguration.SplashPrice * (shopConfiguration.AddLvlPercentSplash *
                                                                              _shopParameters.GetShopLevel(
                                                                                  ShopType.Splash));
                    shopButtonConfiguration.Price.UpdateText(currentPriceSplash.ToString());
                    if (CurrentPrices.ContainsKey(ShopType.Splash) == false)
                    {
                        CurrentPrices.Add(ShopType.Splash, currentPriceSplash);
                    }
                    else
                    {
                        CurrentPrices[ShopType.Splash] = currentPriceSplash;
                    }

                    break;
            }
        }
    }

    private void SubscribeButtons()
    {
        foreach (var shopButtonConfiguration in shopButtonConfigurations)
        {
            if (_damageUpgraders.ContainsKey(shopButtonConfiguration.ShopType) == false)
            {
                continue;
            }

            shopButtonConfiguration.ButtonController.onButtonClicked +=
                _damageUpgraders[shopButtonConfiguration.ShopType].Upgrade;
        }
    }

    private void UnsubscribeButtons()
    {
        foreach (var shopButtonConfiguration in shopButtonConfigurations)
        {
            if (_damageUpgraders.ContainsKey(shopButtonConfiguration.ShopType) == false)
            {
                continue;
            }

            shopButtonConfiguration.ButtonController.onButtonClicked -=
                _damageUpgraders[shopButtonConfiguration.ShopType].Upgrade;
        }
    }

    private void MoneyUpdated(PlayerMoneyType playerMoneyType)
    {
        if (_shopParameters == null) return;
        foreach (var shopButtonConfiguration in shopButtonConfigurations)
        {
            shopButtonConfiguration.ButtonController.Button.interactable =
                _playerMoneyParameterDispenser.IsPlayerHasNeededAmountMoneyByMoneyType(PlayerMoneyType.Coin,
                    (int) CurrentPrices[shopButtonConfiguration.ShopType]);
        }
    }
}

public interface IDamageUpgrader
{
    void Initialize(Shop shop, IPlayerMoneyParameterController playerMoneyParametersController,
        IPlayerMoneyParameterDispenser playerMoneyParameterDispenser,
        ShopParameters shopParameters, IPlayerDamageController playerDamageController);

    void Upgrade();
}

public class ClickDamageUpgrader : IDamageUpgrader
{
    private IPlayerMoneyParameterDispenser _playerMoneyParameterDispenser;
    private IPlayerMoneyParameterController _playerMoneyParameterController;
    private ShopParameters _shopParameters;
    private IPlayerDamageController _playerDamageParameters;
    private Shop _shop;

    public void Initialize(Shop shop, IPlayerMoneyParameterController playerMoneyParametersController,
        IPlayerMoneyParameterDispenser playerMoneyParameterDispenser,
        ShopParameters shopParameters, IPlayerDamageController playerDamageController)
    {
        _shop = shop;
        _playerDamageParameters = playerDamageController;
        _playerMoneyParameterDispenser = playerMoneyParameterDispenser;
        _playerMoneyParameterController = playerMoneyParametersController;
        _shopParameters = shopParameters;
    }

    public void Upgrade()
    {
        var price = (int) _shop.CurrentPrices[ShopType.Click];
        if (_playerMoneyParameterDispenser.IsPlayerHasNeededAmountMoneyByMoneyType(PlayerMoneyType.Coin,
            price) == false)
        {
           return;
        }
        _shopParameters.AddShopLVl(ShopType.Click);
        _playerMoneyParameterController.ReducePlayerMoneyParameterByMoneyType(PlayerMoneyType.Coin, price);
        _playerDamageParameters.AddDamage(DamageType.Click);
        _shop.RecalculatePrices();
    }
}

public class AutoClickDamageUpgrader : IDamageUpgrader
{
    private IPlayerMoneyParameterDispenser _playerMoneyParameterDispenser;
    private IPlayerMoneyParameterController _playerMoneyParameterController;
    private ShopParameters _shopParameters;
    private IPlayerDamageController _playerDamageParameters;
    private Shop _shop;

    public void Initialize(Shop shop, IPlayerMoneyParameterController playerMoneyParametersController,
        IPlayerMoneyParameterDispenser playerMoneyParameterDispenser,
        ShopParameters shopParameters, IPlayerDamageController playerDamageController)
    {
        _shop = shop;
        _playerDamageParameters = playerDamageController;
        _playerMoneyParameterDispenser = playerMoneyParameterDispenser;
        _playerMoneyParameterController = playerMoneyParametersController;
        _shopParameters = shopParameters;
    }

    public void Upgrade()
    {
        var price = (int) _shop.CurrentPrices[ShopType.AutoClick];
        if (_playerMoneyParameterDispenser.IsPlayerHasNeededAmountMoneyByMoneyType(PlayerMoneyType.Coin,
            price) == false) return;
        _shopParameters.AddShopLVl(ShopType.AutoClick);
        _playerMoneyParameterController.ReducePlayerMoneyParameterByMoneyType(PlayerMoneyType.Coin, price);
        _playerDamageParameters.AddDamage(DamageType.Second);
        _shop.RecalculatePrices();
    }
}