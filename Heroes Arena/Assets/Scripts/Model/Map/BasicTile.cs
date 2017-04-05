namespace HeroesArena
{
    // TODO maybe change it to enum later if tiles are just pictures with no mechanics,
    // represents one generiс tile in game logic.
    public class BasicTile
    {
        #region Equals
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // If obj can not be cast to BasicTile.
            BasicTile tile = obj as BasicTile;
            if (tile == null)
            {
                return false;
            }

            return true;
        }

        // For performance.
        public bool Equals(BasicTile tile)
        {
            if (tile == null)
            {
                return false;
            }

            return true;
        }

        // For Equals.
        public override int GetHashCode()
        {
            return 0;
        }
        #endregion

        // TODO
    }
}