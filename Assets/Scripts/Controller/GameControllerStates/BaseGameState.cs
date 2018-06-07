/*
 * Copyright © 2017 Fazli Jan, Oleg Ivanov
 * The project is licensed under the MIT License.
 */

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
        public Text LocalPlayerLabel { get { return GameView.LocalPlayerLabel; } }
        // Label with game state.
        public Text GameStateLabel { get { return GameView.GameStateLabel; } }
        // Button for turn end.
        public Button EndTurnButton { get { return GameView.EndTurnButton; } }
        // Button for move mode.
        public Button MoveButton { get { return GameView.MoveButton; } }
        // Button for attack mode.
        public Button AttackButton { get { return GameView.AttackButton; } }
        // Button for hiding grid.
        public Button HideGridButton { get { return GameView.HideGridButton; } }
        // Button for using skills.
        public Button SkillButton { get { return GameView.SkillButton; } }
        // Label with end game message.
        public Text EndGameLabel { get { return GameView.EndGameLabel; } }

        public GameObject EndGamePanel { get { return GameView.EndGamePanel; } }

        public GameObject BottomPanel { get { return GameView.BottomPanel; } }

        public GameObject ClassSelectionPanel { get { return GameView.ClassSelectionPanel; } }

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