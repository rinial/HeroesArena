namespace HeroesArena
{
    // Represents one of GameController states, when local player is not in control.
    public class PassiveGameState : BaseGameState
    {
        // Executed when entering this state. 
        public override void Enter()
        {
            base.Enter();

            // Updates UI elements.
            GameStateLabel.text = "State: Opponent's Turn.";
            RefreshPlayerLabels();
        }
    }
}