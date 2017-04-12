using UnityEngine;
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

        // UI references.
        public Text LocalPlayerLabel;
        public Text GameStateLabel;
        public GameObject EndTurnButton;

        // Observes some notifications from MatchController, GameModel and PlayerController.
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

        // Stops observing when disabled.
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

        // Checks state at start.
        void Awake()
        {
            CheckState();
        }
        
        // MatchController. Executed when match is ready.
        void OnMatchReady(object sender, object args)
        {
            // We wait here for clients to understand that match is ready.
            // TODO we shoud probably find a way to see when everyone is ready instead of a delay.
            if (MatchController.LocalPlayer.isServer)
                Invoke("StartGame", 1f);
        }

        // Called only by the host starts the game.
        void StartGame()
        {
            // TODO this is called initiative, but is only called for one player and no initiative is used, should be changed.
            MatchController.LocalPlayer.CmdInitiative();
        }

        // PlayerController. Called when initiative is decided.
        void OnInitiative(object sender, object args)
        {
            // TODO no initiative used, just starts the game, should be changed.
            GameModel.Reset(MatchController.Players);
        }

        // PlayerController. Called when move is requested.
        void OnRequestMakeMove(object sender, object args)
        {
            // Calls move for controlling player.
            GameModel.Move((Coordinates)args);
        }

        // PlayerController. Called when turn end is requested.
        void OnRequestEndTurn(object sender, object args)
        {
            // Calls turn change.
            GameModel.ChangeTurn();
        }

        // GameModel. Clears the map at the start of game.
        void OnDidBeginGame(object sender, object args)
        {
            // Draws the whole map.
            // TODO it should only show visible part of map for player.
            GameView.Show(GameModel.Map);

            // Checks state.
            CheckState();
        }

        // GameModel. Called when controlling player is changed.
        void OnDidChangeControl(object sender, object args)
        {
            // Checks state.
            CheckState();
        }
        
        // GameModel. Called when game is ended.
        void OnDidEndGame(object sender, object args)
        { 
            // Checks state.
            CheckState();
        }

        // GameModel. Called when move is made.
        void OnDidMakeMove(object sender, object args)
        {
            // Showes new map after the move.
            // TODO it should only show visible part of map for player.
            GameView.Show(GameModel.Map);
        }

        // Checks and changes state of GameController.
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