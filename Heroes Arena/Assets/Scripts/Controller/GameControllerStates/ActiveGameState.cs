namespace HeroesArena
{
    // Represents one of GameController states, when local player is in control.
    public class ActiveGameState : BaseGameState
    {
        // Executed when entering this state. 
        public override void Enter()
        {
            base.Enter();

            // Updates UI elements.
            GameStateLabel.text = "State: Your Turn!";
            EndTurnButton.SetActive(true);
            RefreshPlayerLabels();
        }

        // Executed when leaving this state.
        public override void Exit()
        {
            base.Exit();

            // Updates UI elements.
            EndTurnButton.SetActive(false);
        }

        // During active phase observes CellClicked and EndTurn notifications.
        protected override void AddListeners()
        {
            base.AddListeners();
            this.AddObserver(OnMapCellClicked, GameView.CellClickedNotification);
            this.AddObserver(OnEndTurnClicked, GameView.EndTurnClickedNotification);
        }

        // Stops observing when leaving state.
        protected override void RemoveListeners()
        {
            base.RemoveListeners();
            this.RemoveObserver(OnMapCellClicked, GameView.CellClickedNotification);
            this.RemoveObserver(OnEndTurnClicked, GameView.EndTurnClickedNotification);
        }

        // GameView. Executed when cell on map is clicked.
        private void OnMapCellClicked(object sender, object args)
        {
            // Calls command to make move.
            LocalPlayer.CmdMakeMove((Coordinates)args);
        }

        // GameView. Executed when EndTurn button is clicked.
        private void OnEndTurnClicked(object sender, object args)
        {
            // Calls command to end turn.
            LocalPlayer.CmdEndTurn();
        }
    }
}