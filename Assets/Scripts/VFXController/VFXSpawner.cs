using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXSpawner : IVFXSpawner
{
    private readonly Dictionary<VFXConfiguration, IPool<TemporaryMonoPooled>> _pools =
        new Dictionary<VFXConfiguration, IPool<TemporaryMonoPooled>>();

    private const int AmountElementInPool = 3;
    public VFXSpawner(List<VFXConfiguration> vfxConfigurations,Transform vfxParent)
    {
        foreach (var vfxConfiguration in vfxConfigurations)
        {
            var factory = new FactoryMonoObject<TemporaryMonoPooled>(vfxConfiguration.VFXPrefab,vfxParent);
            _pools.Add(vfxConfiguration,new Pool<TemporaryMonoPooled>(factory,AmountElementInPool));
        }
    }

    public Transform GetEffectByVFXConfiguration(VFXConfiguration vfxConfiguration)
    {
       return  _pools[vfxConfiguration].Pull().transform;
    }
}