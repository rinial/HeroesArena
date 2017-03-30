namespace HeroesArena
{
    // Represents one player's name.
    public class PlayerName
    {
        public string Name { get; private set; }

        // Constructor. TODO Some name restrictions should be here.
        public PlayerName(string name)
        {
            Name = name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // If obj can not be cast to PlayerName.
            PlayerName name = obj as PlayerName;
            if (name == null)
            {
                return false;
            }

            return Name == name.Name;
        }

        // For performance.
        public bool Equals(PlayerName name)
        {
            if (name == null)
            {
                return false;
            }

            return Name == name.Name;
        }

        // For Equals.
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        // TODO
    }
}