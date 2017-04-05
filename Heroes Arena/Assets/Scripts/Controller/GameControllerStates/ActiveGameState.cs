namespace HeroesArena
{
    public class ActiveGameState : BaseGameState
    {
        public override void Enter()
        {
            base.Enter();
            GameStateLabel.text = "State: Your Turn!";
            EndTurnButton.SetActive(true);
            RefreshPlayerLabels();
        }

        public override void Exit()
        {
            base.Exit();
            EndTurnButton.SetActive(false);
        }

        protected override void AddListeners()
        {
            base.AddListeners();
            this.AddObserver(OnMapCellClicked, GameView.CellClickedNotification);
            this.AddObserver(OnEndTurnClicked, GameView.EndTurnClickedNotification);
        }

        protected override void RemoveListeners()
        {
            base.RemoveListeners();
            this.RemoveObserver(OnMapCellClicked, GameView.CellClickedNotification);
        }

        private void OnMapCellClicked(object sender, object args)
        {
            // Debug.Log("OnMapClick");
            LocalPlayer.CmdMakeMove((Coordinates)args);
        }

        private void OnEndTurnClicked(object sender, object args)
        {
            // Debug.Log("OnEndTurn");
            LocalPlayer.CmdEndTurn();
        }
    }
}