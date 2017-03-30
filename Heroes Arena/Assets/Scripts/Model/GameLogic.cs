using System.Collections.Generic;

namespace HeroesArena
{
    // Interacts with other scripts and represents the logic behind gameplay.
    public class GameLogic
    {
        private Dictionary<PlayerID, Player> _players;
        private Queue<PlayerID> _turnOrder;
        public Map Map { get; private set; }
        public PlayerID Control { get; private set; }
        // TODO change to make TDM instead of FFA.
        public PlayerID Winner { get; private set; }

        // TODO delete later, this is just for tests,
        // moves unit of the controlling player one cell up.
        public void MoveUp()
        {
            Cell currentCell = _players[Control].ControlledUnit.Cell;
            Cell newCell = Map.Cells[new Coordinates(currentCell.Position.X, currentCell.Position.Y + 1)];
            Move(newCell);
        }

        // TODO this may need rework,
        // moves unit of the controlling player to another cell.
        public void Move(Cell cell)
        {
            _players[Control].ControlledUnit.Move(cell);
        }

        private void ChangeTurn()
        {
            _turnOrder.Enqueue(_turnOrder.Dequeue());
            Control = _turnOrder.Peek();

            // TODO
        }

        // TODO may need total rework later,
        // create map, allocate units to players.
        public GameLogic()
        {
            _players = new Dictionary<PlayerID, Player>();
            _turnOrder = new Queue<PlayerID>();
            Winner = null;

            CreatePlayers();
            SetTurnOrder();
            CreateMap();
            CreateAndAssignUnits();
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

        // TODO rework this.
        public void CreateMap()
        {
            List<Cell> cells = new List<Cell>();
            for (int i = 0; i < 10; ++i)
            {
                for (int j = 0; j < 10; ++i)
                {
                    Cell c = new Cell(new Coordinates(i, j), new BasicTile());
                    cells.Add(c);
                }
            }
            Map = new Map(cells);
        }

        // TODO rework this too.
        public void CreateAndAssignUnits()
        {
            BasicUnit u1 = new BasicUnit(Map.Cells[new Coordinates(2, 2)]);
            BasicUnit u2 = new BasicUnit(Map.Cells[new Coordinates(7, 7)]);

            _players[new PlayerID(1)].AssignUnit(u1);
            _players[new PlayerID(2)].AssignUnit(u2);
        }

        // TODO 
        // hey, i just met you
        // and this is crazy
        // but here's my number
        // rework this maybe.
        public void Reset()
        {
            SetTurnOrder();
            CreateMap();
            CreateAndAssignUnits();
        }

        // TODO
        // Player actions
        //     list of actions
        // CheckForGameOver()
        //     CheckForWin()
        //     CheckForStalemate() if we ever need it
        // etc.
    }
}
