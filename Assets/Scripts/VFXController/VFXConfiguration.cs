using UnityEngine;

[CreateAssetMenu(menuName = "Configuration/VFXConfigurations/VfxConfiguration", fileName = "VFXConfiguration", order = 0)]
public class VFXConfiguration : ScriptableObject
{
    [SerializeField] private GameObject vfxPrefab;

    public GameObject VFXPrefab => vfxPrefab;
}