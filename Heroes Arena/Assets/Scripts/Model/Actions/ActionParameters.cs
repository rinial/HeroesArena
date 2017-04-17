namespace HeroesArena
{
    // Represents parameters for one action.
    public class ActionParameters
    {
        // Action's tag.
        public ActionTag Tag;
        // Targets of action.
        public Coordinates[] Targets;

        #region Constructors
        // Constructors. UNetWeaver needs basic constructor.
        public ActionParameters()
        {
            Tag = ActionTag.None;
            Targets = null;
        }
        public ActionParameters(ActionTag tag, Coordinates[] targets = null)
        {
            Tag = tag;
            Targets = targets;
        }
        public ActionParameters(ActionTag tag, Coordinates target)
        {
            Tag = tag;
            Targets = new [] { target };
        }
        #endregion
    }
}
