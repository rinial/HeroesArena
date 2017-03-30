namespace HeroesArena
{
	// Represents one player in game logic.
	public class Player
	{
	    public readonly PlayerID ID;
        public PlayerName Name { get; private set; }
        // TODO make it list of we need multiple units for one player.
        public BasicUnit ControlledUnit { get; private set; }

	    public void AssignUnit(BasicUnit unit)
	    {
	        ControlledUnit = unit;
	    }

        // Construstor.
	    public Player(PlayerID id, PlayerName name, BasicUnit unit = null)
	    {
	        ID = id;
	        Name = name;
	        ControlledUnit = unit;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // If obj can not be cast to Player.
            Player player = obj as Player;
            if (player == null)
            {
                return false;
            }

            return ID == player.ID;
        }

        // For performance.
        public bool Equals(Player player)
        {
            if (player == null)
            {
                return false;
            }

            return ID == player.ID;
        }

        // For Equals.
        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        // TODO
    }
}
