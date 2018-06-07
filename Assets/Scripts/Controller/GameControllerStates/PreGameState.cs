/*
 * Copyright © 2017 Fazli Jan, Oleg Ivanov
 * The project is licensed under the MIT License.
 */

using UnityEngine;
using UnityEngine.UI;

namespace HeroesArena
{
    // Represents one of GameController states, when game started.
    public class PreGameState : BaseGameState
    {
        // Executed when entering this state. 
        public override void Enter()
        {
            base.Enter();

            // Updates UI elements.
            GameStateLabel.text = "State: Choose your Class";

            ClassSelectionPanel.SetActive(true);
            GameView.SetSelectionColors(0.7f, 0.7f, 0.7f);
            Transform temp = ClassSelectionPanel.transform.Find("ConfirmButton");
            temp.gameObject.SetActive(true);
            temp.GetComponent<Button>().interactable = false;

            EndTurnButton.gameObject.SetActive(false);
            MoveButton.gameObject.SetActive(false);
            AttackButton.gameObject.SetActive(false);
            HideGridButton.gameObject.SetActive(false);
            BottomPanel.SetActive(false);
        }
        
        protected override void AddListeners()
        {
            base.AddListeners();
            this.AddObserver(OnSelectedClass, GameView.SelectedClassNotification);
        }

        // Stops observing when leaving state.
        protected override void RemoveListeners()
        {
            base.RemoveListeners();
            this.RemoveObserver(OnSelectedClass, GameView.SelectedClassNotification);
        }

        private void OnSelectedClass(object sender, object args)
        {
            GameStateLabel.text = "State: Waiting for Players";
            ClassSelectionPanel.transform.Find("ConfirmButton").gameObject.SetActive(false);
            LocalPlayer.CmdSetClass(LocalPlayer.netId, new UnitParameters((ClassTag) args));
        }

        // Executed when leaving this state.
        public override void Exit()
        {
            base.Exit();
            
            ClassSelectionPanel.SetActive(false);

            // Updates UI elements.
            EndTurnButton.gameObject.SetActive(true);
            MoveButton.gameObject.SetActive(true);
            AttackButton.gameObject.SetActive(true);
            HideGridButton.gameObject.SetActive(true);
            BottomPanel.SetActive(true);
        }
    }
}