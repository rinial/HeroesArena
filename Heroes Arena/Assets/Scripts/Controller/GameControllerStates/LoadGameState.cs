namespace HeroesArena
{
    // Represents one of GameController states, when game started.
    public class LoadGameState : BaseGameState
    {
        // Executed when entering this state. 
        public override void Enter()
        {
            base.Enter();

            // Updates UI elements.
            GameStateLabel.text = "State: Waiting For Players";
            LocalPlayerLabel.text = "Player: -";
        }

        // Executed when leaving this state.
        public override void Exit()
        {
            base.Exit();

            // Updates UI elements.
            RefreshPlayerLabels();
        }
    }
}