namespace HeroesArena
{
    // Represents damage in game logic.
    public class Damage
    {
        // TODO add damage type.
        
        // Amount of damage.
        public int Amount { get; private set; }

        // Constructor.
        public Damage(int amount = 0)
        {
            Amount = amount;
        }

        #region Equals
        // Equality override.
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // If obj can not be cast to Damage.
            Damage dmg = obj as Damage;
            if (dmg == null)
            {
                return false;
            }

            return Amount.Equals(dmg.Amount);
        }

        // For performance.
        public bool Equals(Damage dmg)
        {
            if (dmg == null)
            {
                return false;
            }

            return Amount.Equals(dmg.Amount);
        }

        // For Equals.
        public override int GetHashCode()
        {
            return Amount.GetHashCode();
        }
        #endregion
    }
}
