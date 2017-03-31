using UnityEngine;

namespace HeroesArena
{
    // Connects game model and map view.
    public class GameController : MonoBehaviour
    {
        public GameModel GameModel = new GameModel();
        public GameView GameView;

        private void OnEnable()
        {
            this.AddObserver(OnMapCellClicked, GameView.CellClickedNotification);
            this.AddObserver(OnEndTurnClicked, GameView.EndTurnClickedNotification);
            this.AddObserver(OnDidBeginGame, GameModel.DidBeginGameNotification);
            this.AddObserver(OnDidMakeMove, GameModel.DidMakeMoveNotification);
        }

        private void OnDisable()
        {
            this.RemoveObserver(OnMapCellClicked, GameView.CellClickedNotification);
            this.RemoveObserver(OnEndTurnClicked, GameView.EndTurnClickedNotification);
            this.RemoveObserver(OnDidBeginGame, GameModel.DidBeginGameNotification);
            this.RemoveObserver(OnDidMakeMove, GameModel.DidMakeMoveNotification);
        }

        private void Start()
        {
            GameView = GameObject.FindGameObjectWithTag("Map").GetComponent<GameView>();
            GameModel.Reset();
        }
        
        // TODO lots of changes here

        // Moves unit on click.
        private void OnMapCellClicked(object sender, object args)
        {
            Debug.Log("OnMapClick");
            GameModel.Move((Coordinates)args);
        }

        // End turn on button click.
        private void OnEndTurnClicked(object sender, object args)
        {
            Debug.Log("OnEndTurn");
            GameModel.ChangeTurn();
        }

        // Clears the map at the start of game.
        void OnDidBeginGame(object sender, object args)
        {
            Debug.Log("OnBegin");
            // Draws the whole map.
            GameView.Show(GameModel.Map);
        }

        // Shows the move.
        void OnDidMakeMove(object sender, object args)
        {
            Debug.Log("OnMove");
            // Redraws the whole map.
            GameView.Show(GameModel.Map);
        }
    }
}