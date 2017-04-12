namespace HeroesArena
{
    // Represents anything that can deal damage.
    public interface IDamageDealer
    {
        // Deals damage to target.
        void DealDamage(IDamageable target, Damage damage);
    }
}
