using UnityEngine;
using UnityEngine.UI;

namespace HeroesArena
{
    public abstract class BaseGameState : State
    {
        public GameController Owner;

        public GameModel GameModel { get { return Owner.GameModel; } }
        public GameView GameView { get { return Owner.GameView; } }

        public Text LocalPlayerLabel { get { return Owner.LocalPlayerLabel; } }
        public Text GameStateLabel { get { return Owner.GameStateLabel; } }
        public GameObject EndTurnButton { get { return Owner.EndTurnButton; } }

        public PlayerController LocalPlayer { get { return Owner.MatchController.LocalPlayer; } }
        // TODO do we need it?
        public PlayerController RemotePlayer { get { return Owner.MatchController.RemotePlayer; } }

        protected virtual void Awake()
        {
            Owner = GetComponent<GameController>();
        }

        protected void RefreshPlayerLabels()
        {
            LocalPlayerLabel.text = string.Format("Player: {0}", LocalPlayer.Name);
        }
    }
}