using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private Transform cylinder;
    [SerializeField] private List<VFXConfiguration> vfxConfigurations;
    [SerializeField] private Animator buttonAnimator;
    public override void InstallBindings()
    {
        var vfxSpawner = new VFXSpawner(vfxConfigurations,transform);
        Container.Bind<ButtonAnimatorController>().FromInstance(new ButtonAnimatorController(buttonAnimator)).AsSingle();
        Container.Bind<IVFXSpawner>().FromInstance(vfxSpawner).AsSingle();
        Container.Bind<Transform>().WithId(GameSceneTransformType.CoinSpawnPoint).FromInstance(cylinder).AsSingle();
    }
}