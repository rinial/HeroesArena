/*
 * Copyright © 2017 Fazli Jan, Oleg Ivanov
 * The project is licensed under the MIT License.
 */

namespace HeroesArena
{
    // Represents anything that can deal damage.
    public interface IDamageDealer
    {
        // Deals damage to target.
        void DealDamage(IDamageable target, Damage damage);
    }
}
