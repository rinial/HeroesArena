using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace HeroesArena
{
    // Interacts with other scripts and represents the logic behind gameplay.
    public class GameModel
    {
        // Notifications.
        public const string DidBeginGameNotification = "GameModel.DidBeginGameNotification";
        public const string DidMakeMoveNotification = "GameModel.DidMakeMoveNotification";
        public const string DidChangeControlNotification = "GameModel.DidChangeControlNotification";
        public const string DidEndGameNotification = "GameModel.DidEndGameNotification";

        // Stores players and provides easy id-player access.
        private Dictionary<NetworkInstanceId, PlayerController> _players;
        // Order of players.
        private Queue<NetworkInstanceId> _turnOrder;
        // Stores map.
        public Map Map { get; private set; }
        // ID of the player who is currently in control.
        public NetworkInstanceId Control { get; private set; }
        // TODO change to make TDM instead of FFA.
        // ID of the player who won.
        public NetworkInstanceId Winner { get; private set; }

        #region Move
        // Moves unit of the controlling player to another cell.
        public void Move(Cell cell)
        {
            // Doesn't work if cell is not specified.
            if (cell == null)
                return;

            // Moves the unit.
            _players[Control].ControlledUnit.Move(cell);

            // Notifies GameController that move was made.
            this.PostNotification(DidMakeMoveNotification);
        }
        public void Move(Coordinates pos)
        {
            // Doesn't work if position can't be found on the map.
            if (Map.Cells.ContainsKey(pos))
                Move(Map.Cells[pos]);
        }
        #endregion

        // Changes turn.
        public void ChangeTurn()
        {
            // Moves current controling player to the end of turn order.
            _turnOrder.Enqueue(_turnOrder.Dequeue());
            Control = _turnOrder.Peek();

            // Notifies GameController that move was made.
            this.PostNotification(DidChangeControlNotification);
        }

        // Resets and starts the game.
        public void Reset(List<PlayerController> players)
        {
            Winner = NetworkInstanceId.Invalid;
            SetPlayers(players);
            SetTurnOrder();
            CreateMap();
            CreateObjects();
            CreateAndAssignUnits();

            // Notifies GameController that game is begun.
            this.PostNotification(DidBeginGameNotification);
        }

        // Clears old players and sets new.
        private void SetPlayers(List<PlayerController> players)
        {
            _players.Clear();
            foreach (PlayerController player in players)
                _players[player.netId] = player;
        }

        // Clears old turn order and sets new.
        private void SetTurnOrder()
        {
            // TODO add some dependency on initiative.

            _turnOrder.Clear();
            foreach (NetworkInstanceId id in _players.Keys)
            {
                _turnOrder.Enqueue(id);
            }
            Control = _turnOrder.Peek();
        }

        // TODO this should be totally reworked.
        // Clears old map and creates new.
        public void CreateMap()
        {
            // Just a 5x5 square.
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

        // Creates objects on the map.
        public void CreateObjects()
        {
            new BasicObject(Map.Cells[new Coordinates(2, 3)]);
        }

        // TODO this should be totally reworked.
        // Creates and assigns units for players.
        public void CreateAndAssignUnits()
        {
            // TODO names are not supposed to be here.
            string[] names = { "Arngrim", "Bjorn", "Einherjar", "Guomundr", "Hrothgar", "Ingvar", "Jonark", "Kjarr", "Niohad", "Orvar", "Palnatoke", "Ragnar", "Sigmund", "Volsung", "Weohstan", "Yrsa" };

            List<NetworkInstanceId> players = _players.Keys.ToList();
            List<Cell> cells = Map.Cells.Values.ToList();
            for (int i = 0; i < players.Count; ++i)
            {
                // TODO units are not supposed to be created this way.
                BasicUnit unit = new BasicUnit(cells[i], Direction.Down, new Parameter<int>(5), new Parameter<int>(5));

                _players[players[i]].AssignUnit(unit);
                _players[players[i]].Name = names[Random.Range(0, names.Length)];
            }
        }

        // Checks if game is ended.
        public bool GameIsEnded()
        {
            // TODO
            return false;
        }

        // Constructor.
        public GameModel()
        {
            _players = new Dictionary<NetworkInstanceId, PlayerController>();
            _turnOrder = new Queue<NetworkInstanceId>();
        }
    }
}
