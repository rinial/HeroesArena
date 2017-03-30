namespace HeroesArena
{
    // Represents one player's ID.
    public class PlayerID
    {
        public readonly int ID;

        // Constructor.
        public PlayerID(int id)
        {
            ID = id;
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // If obj can not be cast to PlayerID.
            PlayerID id = obj as PlayerID;
            if (id == null)
            {
                return false;
            }

            return ID == id.ID;
        }

        // For performance.
        public bool Equals(PlayerID id)
        {
            if (id == null)
            {
                return false;
            }

            return ID == id.ID;
        }

        // For Equals.
        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        // TODO
    }
}