using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace HeroesArena
{
    // Interacts with other scripts and represents the logic behind gameplay.
    public class GameModel
    {
        // TODO extract mapsize
        public const int MapSize = 10;
        public const int NumWalls = 10;
        public const int NumObjects = 10;

        // Notifications.
        public const string DidBeginGameNotification = "GameModel.DidBeginGameNotification";
        public const string DidExecuteActionNotification = "GameModel.DidExecuteActionNotification";
        public const string DidChangeControlNotification = "GameModel.DidChangeControlNotification";
        public const string DidEndGameNotification = "GameModel.DidEndGameNotification";
        public const string RequestMap = "GameModel.RequestMap";
        public const string DidKillPlayer = "GameModel.DidKillPlayer";

        // Stores players and provides easy id-player access.
        private Dictionary<NetworkInstanceId, PlayerController> _players;
        // returns the number of players currently alive in game
        private int playersAlive
        {
            get
            {
                return _turnOrder.Count;
            }
        }

        // Order of players.
        private List<NetworkInstanceId> _turnOrder;
        private int controlIndex;
        // Stores map.
        public Map Map { get; private set; }
        // ID of the player who is currently in control.
        public NetworkInstanceId Control
        {
            get
            {
                return _turnOrder[controlIndex];
            }
        }

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

            // synchronize the random state
            Random.InitState(param.randomSeed);

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
        public void ChangeTurn(bool increment = true)
        {
            if(increment)
                IncrementTurnIndex();
            else
                FixTurnOrder();

            _players[Control].ControlledUnit.TurnStart();

            // Notifies GameController that turn was changed.
            this.PostNotification(DidChangeControlNotification);
        }

        private void IncrementTurnIndex()
        {
            controlIndex++;
            FixTurnOrder();
        }

        private void FixTurnOrder()
        {
            if (controlIndex >= _turnOrder.Count)
                controlIndex = 0;
        }

        // Resets and starts the game.
        public void Reset(List<PlayerController> players = null, int firstPlayer = 0)
        {
            Winner = NetworkInstanceId.Invalid;
            if(players != null)
                SetPlayers(players);
            SetTurnOrder(firstPlayer);

            // Notifies GameController that map is needed.
            var args = new int[] { _players.Count, MapSize, MapSize, NumWalls, NumObjects };
            this.PostNotification(RequestMap, args);
        }

        // Continues game reset with new map.
        public void ContinueReset(Map map)
        {
            Map = map;
            // AssignUnits();

            // Notifies GameController that game is begun.
            this.PostNotification(DidBeginGameNotification);
        }

        // Assigns units for players.
        public void AssignUnits()
        {

            List<NetworkInstanceId> players = _players.Keys.ToList();
            List<BasicUnit> units = Map.GetUnits();
            for (int i = 0; i < players.Count; ++i)
            {
                _players[players[i]].AssignUnit(units[i]);
            }
        }

        // Clears old players and sets new.
        private void SetPlayers(List<PlayerController> players)
        {
            _players.Clear();
            foreach (PlayerController player in players)
                _players[player.netId] = player;
        }

        // Clears old turn order and sets new.
        private void SetTurnOrder(int firstPlayer)
        {
            // TODO add some dependency on initiative.
            _turnOrder.Clear();
            foreach (NetworkInstanceId id in _players.Keys)
            {
                _turnOrder.Add(id);
            }

            controlIndex = firstPlayer;
        }

        // Sets a player as killed removing him from the player queue.
        public void KillPlayer(object sender, object args)
        {
            // finds player by the killed unit
            NetworkInstanceId deadPlayer;
            foreach (var netId in _players.Keys)
            {
                if (_players[netId].ControlledUnit == sender)
                {
                    deadPlayer = netId;
                    bool toChangeTurn = false;
                    toChangeTurn = Control == deadPlayer;
                    _turnOrder.Remove(deadPlayer);

                    if (IsGameOver)
                    {
                        Winner = _turnOrder[0];
                        this.PostNotification(DidEndGameNotification);
                    }

                    if (toChangeTurn)
                        ChangeTurn(false);

                    break;
                }

                // else exception??? I mean what are we gonna do if the killed player hadn't existed in the first place..... creepy..
            }
        }

        // Checks if game over.
        public bool IsGameOver
        {
            get
            {
                return playersAlive == 1;
            }
        }

        // Constructor.
        public GameModel()
        {
            _players = new Dictionary<NetworkInstanceId, PlayerController>();
            _turnOrder = new List<NetworkInstanceId>();
        }
    }
}
