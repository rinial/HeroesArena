using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace HeroesArena
{
    // Interacts with other scripts and represents the logic behind gameplay.
    public class GameModel
    {
        // TODO extract mapsize
        public const int MapSize = 10;
        public const int NumWalls = 10;

        // Notifications.
        public const string DidBeginGameNotification = "GameModel.DidBeginGameNotification";
        public const string DidExecuteActionNotification = "GameModel.DidExecuteActionNotification";
        public const string DidChangeControlNotification = "GameModel.DidChangeControlNotification";
        public const string DidEndGameNotification = "GameModel.DidEndGameNotification";
        public const string DidRequestMap = "GameModel.DidRequestMap";

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

        #region ActionExecution
        // Executes action for the controlling player.
        public void ExecuteAction(ActionParameters param)
        {
            // If action is not specified.
            if (param.Tag == ActionTag.None)
                return;

            // If no specified action.
            if (!_players[Control].ControlledUnit.Actions.ContainsKey(param.Tag))
                return;

            // If no targets.
            if (param.Targets == null)
            {
                _players[Control].ControlledUnit.Actions[param.Tag].Execute(Map);
                return;
            }

            // Forms cells list.
            List<Cell> cells = new List<Cell>();
            foreach (Coordinates pos in param.Targets)
            {
                // If no cell for position.
                if (!Map.Cells.ContainsKey(pos) || Map.Cells[pos] == null)
                    return;
                cells.Add(Map.Cells[pos]);
            }

            // Executes action.
            _players[Control].ControlledUnit.Actions[param.Tag].Execute(cells, Map);

            // Notifies GameController that move was made.
            this.PostNotification(DidExecuteActionNotification);
        }
        #endregion

        // Changes turn.
        public void ChangeTurn()
        {
            // Moves current controling player to the end of turn order.
            _turnOrder.Enqueue(_turnOrder.Dequeue());
            Control = _turnOrder.Peek();
            _players[Control].ControlledUnit.TurnStart();

            // Notifies GameController that turn was changed.
            this.PostNotification(DidChangeControlNotification);
        }

        // Resets and starts the game.
        public void Reset(List<PlayerController> players)
        {
            Winner = NetworkInstanceId.Invalid;
            SetPlayers(players);
            SetTurnOrder();

            // Notifies GameController that map is needed.
            var args = new int[] {MapSize, MapSize, NumWalls};
            this.PostNotification(DidRequestMap, args);
        }

        public void ContinueReset(Map map)
        {
            Map = map;
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

        // Clears old map and creates new.
        public void CreateMap()
        {
            // generate a randomized map using the MapGenerator class
            Map = MapGenerator.Generate(MapSize, NumWalls);
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
            int j = 0;
            for (int i = 0; i < players.Count; ++i)
            {
                // TODO units are not supposed to be created this way.
                while (!(cells[j].Tile.Walkable && cells[j].Unit == null))
                    ++j;

                BasicUnit unit = new BasicUnit(cells[j], Direction.Down, ClassTag.Rogue);

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
