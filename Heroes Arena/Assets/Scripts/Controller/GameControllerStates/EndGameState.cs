using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace HeroesArena
{
    public class EndGameState : BaseGameState
    {
        public override void Enter()
        {
            base.Enter();

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

            if (!LocalPlayer.isServer)
                StartCoroutine(Restart());
        }

        IEnumerator Restart()
        {
            yield return new WaitForSeconds(5);
            LocalPlayer.CmdInitiative();
        }
    }
}