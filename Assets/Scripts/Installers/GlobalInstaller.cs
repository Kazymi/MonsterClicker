using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    [SerializeField] private InputController inputControllerPrefab;
    [SerializeField] private PlayerDamageConfiguration playerDamageConfiguration;
    public override void InstallBindings()
    {
        var playerMoneyParameters = new PlayerMoneyParametersController();
        var playerParameters = new PlayerDamageParameters(playerDamageConfiguration);
        
        Container.Bind<IInputController>().To<InputController>().FromComponentInNewPrefab(inputControllerPrefab)
            .WithGameObjectName(nameof(InputController)).AsSingle().NonLazy();


        Container.Bind<IPlayerMoneyParameterController>().FromInstance(playerMoneyParameters).AsSingle();
        Container.Bind<IPlayerMoneyParameterDispenser>().FromInstance(playerMoneyParameters).AsSingle();

        Container.Bind<IPlayerDamageController>().FromInstance(playerParameters).AsSingle();
        Container.Bind<IPlayerDamageParametersDispenser>().FromInstance(playerParameters).AsSingle();
        
    }
}