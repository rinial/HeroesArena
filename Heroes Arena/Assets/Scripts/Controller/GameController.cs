using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace HeroesArena
{
    // Connects MatchController, PlayerController, GameModel and GameView.
    public class GameController : StateMachine
    {
        // Important references.
        public GameModel GameModel = new GameModel();
        public GameView GameView;
        public MatchController MatchController;

        // For testing matters.
        public bool DebugMode = true;

        // Observes some notifications from MatchController, GameModel and PlayerController.
        private void OnEnable()
        {
            this.AddObserver(OnMatchReady, MatchController.MatchReady);
            this.AddObserver(OnDidBeginGame, GameModel.DidBeginGameNotification);
            this.AddObserver(OnDidExecuteAction, GameModel.DidExecuteActionNotification);
            this.AddObserver(OnDidChangeControl, GameModel.DidChangeControlNotification);
            this.AddObserver(OnDidEndGame, GameModel.DidEndGameNotification);
            this.AddObserver(OnInitiative, PlayerController.Initiative);
            this.AddObserver(OnSetName, PlayerController.SetName);
            this.AddObserver(OnRestart, PlayerController.Restart);
            this.AddObserver(OnRequestExecuteAction, PlayerController.RequestExecuteAction);
            this.AddObserver(OnRequestEndTurn, PlayerController.RequestEndTurn);
            this.AddObserver(OnRequestMap, GameModel.RequestMap);
            this.AddObserver(OnMapCreated, PlayerController.MapCreated);
            this.AddObserver(OnPlayerDied, BasicUnit.PlayerDied);
        }

        // Stops observing when disabled.
        private void OnDisable()
        {
            this.RemoveObserver(OnMatchReady, MatchController.MatchReady);
            this.RemoveObserver(OnDidBeginGame, GameModel.DidBeginGameNotification);
            this.RemoveObserver(OnDidExecuteAction, GameModel.DidExecuteActionNotification);
            this.RemoveObserver(OnDidChangeControl, GameModel.DidChangeControlNotification);
            this.RemoveObserver(OnDidEndGame, GameModel.DidEndGameNotification);
            this.RemoveObserver(OnInitiative, PlayerController.Initiative);
            this.RemoveObserver(OnSetName, PlayerController.SetName);
            this.RemoveObserver(OnRequestExecuteAction, PlayerController.RequestExecuteAction);
            this.RemoveObserver(OnRequestEndTurn, PlayerController.RequestEndTurn);
            this.RemoveObserver(OnRequestMap, GameModel.RequestMap);
            this.RemoveObserver(OnMapCreated, PlayerController.MapCreated);
            this.RemoveObserver(OnPlayerDied, BasicUnit.PlayerDied);
        }

        // Checks state at start.
        private void Awake()
        {
            CheckState();
        }

        // Called from MatchController when match is ready.
        private void OnMatchReady(object sender, object args)
        {
            // We wait here for clients to understand that match is ready.
            // TODO we shoud probably find a way to see when everyone is ready instead of a delay.
            if (MatchController.LocalPlayer.isServer)
                Invoke("StartGame", 1f);
        }

        // Called only by the host starts the game.
        private void StartGame()
        {
            // TODO this is called initiative, but is only called for one player and no initiative is used, should be changed.
            MatchController.LocalPlayer.CmdInitiative();
        }

        // Called from PlayerController when initiative is decided.
        private void OnInitiative(object sender, object args)
        {
            // TODO no initiative used, just starts the game, should be changed.
            PlayerController player = MatchController.LocalPlayer;
            player.CmdSetName(player.netId, FindObjectOfType<MainMenu>().PlayerName);
        }

        private void OnSetName(object sender, object args)
        {
            object[] param = args as object[];
            int num = MatchController.NumberOfPlayers;
            foreach (PlayerController player in MatchController.Players)
            {
                if (player.netId == (NetworkInstanceId) param[0])
                    player.Name = (string) param[1];
                if (player.Name != "")
                    --num;
            }
            if(num == 0)
                GameModel.Reset(MatchController.Players);
        }

        // Called from GameModel when new map is needed.
        private void OnRequestMap(object sender, object args)
        {
            // arguments: width, height, number of rand. walls
            var intArgs = args as int[];
            if (MatchController.LocalPlayer.isServer)
                MatchController.LocalPlayer.CmdCreateMap(intArgs[0], intArgs[1], intArgs[2], intArgs[3], intArgs[3], intArgs[4], intArgs[4]);
        }
        // Calles from PlayerController when map is created.
        private void OnMapCreated(object sender, object args)
        {
            // continues game reset with new map
            GameModel.ContinueReset(args as Map);
        }

        public void OnRequestRestart()
        {
            MatchController.LocalPlayer.CmdRestart();
        }
        public void OnRestart(object sender, object args)
        {
            GameModel.Reset(MatchController.Players);
        }

        // Called from PlayerController when action execution is requested.
        private void OnRequestExecuteAction(object sender, object args)
        {
            // Calls action execution for controlling player.
            GameModel.ExecuteAction((ActionParameters)args);
        }

        // Called from GameModel when action is executed.
        private void OnDidExecuteAction(object sender, object args)
        {
            GameView.Show(new Map(GameModel.Map.GetVisibleCells(MatchController.LocalPlayer.ControlledUnit.Cell)));
        }

        // Called from PlayerController when turn end is requested.
        private void OnRequestEndTurn(object sender, object args)
        {
            // Calls turn change.
            GameModel.ChangeTurn();
        }

        // Called from GameModel when game starts.
        private void OnDidBeginGame(object sender, object args)
        {
            GameView.Clear();
            GameView.Show(new Map(GameModel.Map.GetVisibleCells(MatchController.LocalPlayer.ControlledUnit.Cell)));
            GameView.SetSkillButton();

            // Checks state.
            CheckState();
        }

        // Called from GameModel when controlling player is changed.
        private void OnDidChangeControl(object sender, object args)
        {
            GameView.Show(new Map(GameModel.Map.GetVisibleCells(MatchController.LocalPlayer.ControlledUnit.Cell)));

            // Checks state.
            CheckState();
        }

        // Called from BasicUnit when the unit is killed.
        private void OnPlayerDied(object sender, object args)
        {
            GameModel.KillPlayer(sender, args);
        }

        // Called from GameModel when game is ended.
        private void OnDidEndGame(object sender, object args)
        {
            GameView.Show(GameModel.Map);
            GameView.Show(new Map(GameModel.Map.GetVisibleCells(MatchController.LocalPlayer.ControlledUnit.Cell)));
            // Checks state.
            CheckState();
        }

        // Checks and changes state of GameController.
        private void CheckState()
        {
            if (!MatchController.IsReady)
                ChangeState<LoadGameState>();
            else if (GameModel.IsGameOver && !DebugMode)
                ChangeState<EndGameState>();
            else if (GameModel.Control == MatchController.LocalPlayer.netId)
                ChangeState<ActiveGameState>();
            else
                ChangeState<PassiveGameState>();
        }
    }
}