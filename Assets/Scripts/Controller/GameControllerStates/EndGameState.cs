/*
 * Copyright © 2017 Fazli Jan, Oleg Ivanov
 * The project is licensed under the MIT License.
 */

using UnityEngine;
using UnityEngine.Networking;

namespace HeroesArena
{
    // Represents one of GameController states, when game ended.
    public class EndGameState : BaseGameState
    {
        // Executed when entering this state.
        public override void Enter()
        {
            base.Enter();

            EndGamePanel.SetActive(true);

            // Updates UI elements.
            if (GameModel.Winner == NetworkInstanceId.Invalid)
            {
                GameStateLabel.text = "State: Tie Game!";
                EndGameLabel.text = "Tie Game!";
            }
            else if (GameModel.Winner == LocalPlayer.netId)
            {
                GameStateLabel.text = "State: You Win!";
                EndGameLabel.text = "You win!";
            }
            else
            {
                GameStateLabel.text = "State: You Lose!";
                EndGameLabel.text = "You lose!";
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            // Shows highlight where mouse points.
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Coordinates coords = GameView.WorldToCoordinates(mousePos);
            GameView.ShowMouseHighlight(coords);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Owner.OnRequestRestart();
            }
        }

        public override void Exit()
        {
            base.Exit();

            EndGamePanel.SetActive(false);
        }
    }
}