using UnityEngine;

public interface IVFXSpawner
{
    Transform GetEffectByVFXConfiguration(VFXConfiguration vfxConfiguration);
}