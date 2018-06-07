/*
 * Copyright © 2017 Fazli Jan, Oleg Ivanov
 * The project is licensed under the MIT License.
 */

namespace HeroesArena
{
    // Represents anything that can be damaged.
    public interface IDamageable
    {
        // Takes damage from source.
        void TakeDamage(IDamageDealer source, Damage damage);
    }
}
