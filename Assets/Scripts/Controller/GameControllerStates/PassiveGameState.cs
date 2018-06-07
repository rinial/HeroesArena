/*
 * Copyright © 2017 Fazli Jan, Oleg Ivanov
 * The project is licensed under the MIT License.
 */

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