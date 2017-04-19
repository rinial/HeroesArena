using UnityEngine;
using UnityEngine.UI;

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
            EndTurnButton.interactable = true;
            MoveButton.interactable = true;
            AttackButton.interactable = true;
            SkillButton.interactable = true;
            // Sets chosen click action to move. Shows action highlight.
            GameView.OnMoveClick();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            // Shows highlight where mouse points.
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Coordinates coords = GameView.WorldToCoordinates(mousePos);
            GameView.ShowMouseHighlight(coords);
            GameView.ShowSelectedAreaHighlights(coords);
        }

        // Executed when leaving this state.
        public override void Exit()
        {
            base.Exit();

            GameView.OnMoveClick();

            GameView.HideActionHighlights();
            GameView.ClearSelectedAreaHighlights();

            // Updates UI elements.
            EndTurnButton.interactable = false;
            MoveButton.interactable = false;
            AttackButton.interactable = false;
            SkillButton.interactable = false;
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

        // Called from GameView when cell on map is clicked.
        private void OnMapCellClicked(object sender, object args)
        {
            LocalPlayer.CmdExecuteAction((ActionParameters)args);
        }

        // Called from GameView when EndTurn button is clicked.
        private void OnEndTurnClicked(object sender, object args)
        {
            // Calls command to end turn.
            LocalPlayer.CmdEndTurn();
        }
    }
}