using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace HeroesArena
{
    // Connects game model and map view.
    public class GameController : StateMachine
    {
        public GameModel GameModel = new GameModel();
        public GameView GameView;
        public MatchController MatchController;

        public Text LocalPlayerLabel;
        public Text GameStateLabel;
        public GameObject EndTurnButton;

        private void OnEnable()
        {
            this.AddObserver(OnMatchReady, MatchController.MatchReady);
            this.AddObserver(OnDidBeginGame, GameModel.DidBeginGameNotification);
            this.AddObserver(OnDidMakeMove, GameModel.DidMakeMoveNotification);
            this.AddObserver(OnDidChangeControl, GameModel.DidChangeControlNotification);
            this.AddObserver(OnDidEndGame, GameModel.DidEndGameNotification);
            this.AddObserver(OnInitiative, PlayerController.Initiative);
            this.AddObserver(OnRequestMakeMove, PlayerController.RequestMakeMove);
            this.AddObserver(OnRequestEndTurn, PlayerController.RequestEndTurn);
        }

        private void OnDisable()
        {
            this.RemoveObserver(OnMatchReady, MatchController.MatchReady);
            this.RemoveObserver(OnDidBeginGame, GameModel.DidBeginGameNotification);
            this.RemoveObserver(OnDidMakeMove, GameModel.DidMakeMoveNotification);
            this.RemoveObserver(OnDidChangeControl, GameModel.DidChangeControlNotification);
            this.RemoveObserver(OnDidEndGame, GameModel.DidEndGameNotification);
            this.RemoveObserver(OnInitiative, PlayerController.Initiative);
            this.RemoveObserver(OnRequestMakeMove, PlayerController.RequestMakeMove);
            this.RemoveObserver(OnRequestEndTurn, PlayerController.RequestEndTurn);
        }

        void Awake()
        {
            CheckState();
        }

        // TODO lots of changes here

        void OnMatchReady(object sender, object args)
        {
            // TODO
            // we wait here for client to understand that match is ready
            if (MatchController.ClientPlayer.isLocalPlayer)
                MatchController.ClientPlayer.CmdInitiative();
        }

        void OnInitiative(object sender, object args)
        {
            // float initiative = (float)args;
            // TODO
            GameModel.Reset(MatchController.Players);
        }

        void OnRequestMakeMove(object sender, object args)
        {
            GameModel.Move((Coordinates)args);
        }

        void OnRequestEndTurn(object sender, object args)
        {
            GameModel.ChangeTurn();
        }

        // Clears the map at the start of game.
        void OnDidBeginGame(object sender, object args)
        {
            // Debug.Log("OnBegin");
            // Draws the whole map.
            GameView.Show(GameModel.Map);
            CheckState();
        }

        void OnDidChangeControl(object sender, object args)
        {
            CheckState();
        }

        void OnDidEndGame(object sender, object args)
        {
            CheckState();
        }

        // Shows the move.
        void OnDidMakeMove(object sender, object args)
        {
            // Debug.Log("OnMove");
            // Redraws the whole map except for unchanged cells.
            GameView.Show(GameModel.Map);
        }

        void CheckState()
        {
            if (!MatchController.IsReady)
                ChangeState<LoadGameState>();
            else if (GameModel.GameIsEnded())
                ChangeState<EndGameState>();
            else if (GameModel.Control == MatchController.LocalPlayer.netId)
                ChangeState<ActiveGameState>();
            else
                ChangeState<PassiveGameState>();
        }
    }
}