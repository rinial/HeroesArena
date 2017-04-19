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
            LocalPlayerLabel.text = "Player: " + FindObjectOfType<MainMenu>().PlayerName;

            EndTurnButton.gameObject.SetActive(false);
            MoveButton.gameObject.SetActive(false);
            AttackButton.gameObject.SetActive(false);
            HideGridButton.gameObject.SetActive(false);
            BottomPanel.SetActive(false);
        }

        // Executed when leaving this state.
        public override void Exit()
        {
            base.Exit();
        }
    }
}