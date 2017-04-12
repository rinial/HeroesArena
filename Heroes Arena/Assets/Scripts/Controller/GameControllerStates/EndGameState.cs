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

            // Updates UI elements.
            if (GameModel.Winner == NetworkInstanceId.Invalid)
            {
                GameStateLabel.text = "State: Tie Game!";
            }
            else if (GameModel.Winner == LocalPlayer.netId)
            {
                GameStateLabel.text = "State: You Win!";
            }
            else
            {
                GameStateLabel.text = "State: You Lose!";
            }
            RefreshPlayerLabels();

            // TODO restart.
        }
    }
}