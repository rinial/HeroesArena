using UnityEngine;
using UnityEngine.UI;

namespace HeroesArena
{
    // Represents basic GameController state.
    public abstract class BaseGameState : State
    {
        // GameController that uses this state.
        public GameController Owner;
        // Convenient access to GameModel.
        public GameModel GameModel { get { return Owner.GameModel; } }
        // Convenient access to GameView.
        public GameView GameView { get { return Owner.GameView; } }
        // Convenient access to local player.
        public PlayerController LocalPlayer { get { return Owner.MatchController.LocalPlayer; } }

        // UI references.
        // Label with player name.
        public Text LocalPlayerLabel { get { return Owner.LocalPlayerLabel; } }
        // Label with game state.
        public Text GameStateLabel { get { return Owner.GameStateLabel; } }
        // Button for turn end.
        public GameObject EndTurnButton { get { return Owner.EndTurnButton; } }

        // Sets owner.
        protected virtual void Awake()
        {
            Owner = GetComponent<GameController>();
        }

        // Used to update player name label.
        protected void RefreshPlayerLabels()
        {
            LocalPlayerLabel.text = string.Format("Player: {0}", LocalPlayer.Name);
        }
    }
}