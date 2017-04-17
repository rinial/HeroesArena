using UnityEngine;

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

        public override void OnUpdate()
        {
            base.OnUpdate();

            // Shows highlight where mouse points.
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Coordinates coords = GameView.WorldToCoordinates(mousePos);
            GameView.ShowMouseHighlight(coords);
        }
    }
}