namespace HeroesArena
{
    // Represents anything that can be damaged.
    public interface IDamageable
    {
        // Takes damage from source.
        void TakeDamage(IDamageDealer source, Damage damage);
    }
}
