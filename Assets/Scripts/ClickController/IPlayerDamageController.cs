public interface IPlayerDamageController
{
    void AddDamage(DamageType damageType);
    void ReduceDamageByDamageType(DamageType damageType, float reduceValue);
}