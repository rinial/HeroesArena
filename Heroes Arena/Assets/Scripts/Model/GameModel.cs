using System.Collections.Generic;

namespace HeroesArena
{
    // Interacts with other scripts and represents the logic behind gameplay.
    public class GameModel
    {
        public const string DidBeginGameNotification = "GameModel.DidBeginGameNotification";
        public const string DidMakeMoveNotification = "GameModel.DidMakeMoveNotification";
        public const string DidChangeControlNotification = "GameModel.DidChangeControlNotification";
        public const string DidEndGameNotification = "GameModel.DidEndGameNotification";

        private Dictionary<PlayerID, Player> _players;
        private Queue<PlayerID> _turnOrder;
        public Map Map { get; private set; }
        public PlayerID Control { get; private set; }
        // TODO change to make TDM instead of FFA.
        public PlayerID Winner { get; private set; }

        // TODO this may need rework,
        // moves unit of the controlling player to another cell.
        public void Move(Cell cell)
        {
            if (cell == null)
                return;

            _players[Control].ControlledUnit.Move(cell);
            
            this.PostNotification(DidMakeMoveNotification, cell.Position);
        }
        public void Move(Coordinates pos)
        {
            if(Map.Cells.ContainsKey(pos))
                Move(Map.Cells[pos]);
        }

        // Changes turn.
        public void ChangeTurn()
        {
            _turnOrder.Enqueue(_turnOrder.Dequeue());
            Control = _turnOrder.Peek();

            this.PostNotification(DidChangeControlNotification);

            // TODO
        }

        // TODO may need total rework later,
        // create map, allocate units to players.
        public GameModel()
        {
            _players = new Dictionary<PlayerID, Player>();
            _turnOrder = new Queue<PlayerID>();
            CreatePlayers();
        }

        // TODO rework this.
        public void Reset()
        {
            Winner = null;
            SetTurnOrder();
            CreateMap();
            CreateObjects();
            CreateAndAssignUnits();

            this.PostNotification(DidBeginGameNotification);
        }

        // TODO rework this.
        private void CreatePlayers()
        {
            _players.Clear();
            Player p1 = new Player(new PlayerID(1), new PlayerName("Ingvar"));
            Player p2 = new Player(new PlayerID(2), new PlayerName("Hrothgar"));
            _players[p1.ID] = p1;
            _players[p2.ID] = p2;
        }

        // TODO rework this.
        private void SetTurnOrder()
        {
            _turnOrder.Clear();
            foreach (PlayerID id in _players.Keys)
            {
                _turnOrder.Enqueue(id);
            }
            Control = _turnOrder.Peek();
        }

        // TODO rework this too.
        public void CreateMap()
        {
            List<Cell> cells = new List<Cell>();
            for (int i = 0; i < 5; ++i)
            {
                for (int j = 0; j < 5; ++j)
                {
                    Cell c = new Cell(new Coordinates(i, j), new BasicTile());
                    cells.Add(c);
                }
            }
            Map = new Map(cells);
        }

        // TODO rework this as well
        public void CreateObjects()
        {
            BasicObject o = new BasicObject(Map.Cells[new Coordinates(2,3)]);
        }

        // TODO 
        // hey, i just met you
        // and this is crazy
        // but here's my number
        // rework this maybe.
        public void CreateAndAssignUnits()
        {
            BasicUnit u1 = new BasicUnit(Map.Cells[new Coordinates(1, 1)]);
            BasicUnit u2 = new BasicUnit(Map.Cells[new Coordinates(3, 3)]);

            _players[new PlayerID(1)].AssignUnit(u1);
            _players[new PlayerID(2)].AssignUnit(u2);
        }
        
        // TODO
        // Player actions
        // CheckForGameOver()
        //     CheckForWin()
        //     CheckForStalemate() if we ever need it
        // etc.
    }
}
