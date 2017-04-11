using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace HeroesArena
{
    // Interacts with other scripts and represents the logic behind gameplay.
    public class GameModel
    {
        public const string DidBeginGameNotification = "GameModel.DidBeginGameNotification";
        public const string DidMakeMoveNotification = "GameModel.DidMakeMoveNotification";
        public const string DidChangeControlNotification = "GameModel.DidChangeControlNotification";
        public const string DidEndGameNotification = "GameModel.DidEndGameNotification";

        private Dictionary<NetworkInstanceId, PlayerController> _players;
        private Queue<NetworkInstanceId> _turnOrder;
        public Map Map { get; private set; }
        public NetworkInstanceId Control { get; private set; }
        // TODO change to make TDM instead of FFA.
        public NetworkInstanceId Winner { get; private set; }

        // TODO this may need rework,
        // moves unit of the controlling player to another cell.
        public void Move(Cell cell)
        {
            if (cell == null)
                return;

            _players[Control].ControlledUnit.Move(cell);
            
            this.PostNotification(DidMakeMoveNotification);
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
            _players = new Dictionary<NetworkInstanceId, PlayerController>();
            _turnOrder = new Queue<NetworkInstanceId>();
        }

        // TODO rework this.
        public void Reset(List<PlayerController> players)
        {
            Winner = NetworkInstanceId.Invalid;
            SetPlayers(players);
            SetTurnOrder();
            CreateMap();
            CreateObjects();
            CreateAndAssignUnits();

            this.PostNotification(DidBeginGameNotification);
        }

        private void SetPlayers(List<PlayerController> players)
        {
            _players.Clear();
            foreach (PlayerController player in players)
                _players[player.netId] = player;
        }

        // TODO rework this.
        private void SetTurnOrder()
        {
            _turnOrder.Clear();
            foreach (NetworkInstanceId id in _players.Keys)
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
            new BasicObject(Map.Cells[new Coordinates(2,3)]);
        }

        // TODO 
        // hey, i just met you
        // and this is crazy
        // but here's my number
        // rework this maybe.
        public void CreateAndAssignUnits()
        {
            // TODO names are not supposed to be here.
            string[] names = { "Arngrim", "Bjorn", "Einherjar", "Guomundr", "Hrothgar", "Ingvar", "Jonark", "Kjarr", "Niohad", "Orvar", "Palnatoke", "Ragnar", "Sigmund", "Volsung", "Weohstan", "Yrsa"};

            List<NetworkInstanceId> players = _players.Keys.ToList();
            List<Cell> cells = Map.Cells.Values.ToList();
            for (int i = 0; i < players.Count; ++i)
            {
                // TODO units are not supposed to be created this way.
                BasicUnit unit = new BasicUnit(cells[i]);

                _players[players[i]].AssignUnit(unit);
                _players[players[i]].Name = names[Random.Range(0, names.Length)];
            }
        }

        public bool GameIsEnded()
        {
            // TODO
            return false;
        }

        // TODO
        // Player actions
        // CheckForGameOver()
        //     CheckForWin()
        //     CheckForStalemate() if we ever need it
        // etc.
    }
}
