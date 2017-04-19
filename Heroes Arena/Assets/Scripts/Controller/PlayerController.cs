using System.Diagnostics;
using UnityEngine;
using UnityEngine.Networking;

namespace HeroesArena
{
    // Represents one player in network and logic.
    public class PlayerController : NetworkBehaviour
    {
        // Notifications.
        public const string Started = "PlayerController.Start";
        public const string StartedLocal = "PlayerController.StartedLocal";
        public const string Destroyed = "PlayerController.Destroyed";
        public const string Initiative = "PlayerController.Initiative";
        public const string SetName = "PlayerController.SetName";
        public const string SetClass = "PlayerController.SetClass";
        public const string Restart = "PlayerController.Restart";
        public const string Disconnect = "PlayerController.Disconnect";
        public const string RequestEndTurn = "PlayerController.RequestEndTurn";
        public const string RequestExecuteAction = "PlayerController.RequestExecuteAction";
        public const string RequestMap = "PlayerController.RequestMap";
        public const string MapCreated = "PlayerController.MapCreated";

        // Some basic properties.
        // TODO change set for name restrictions.
        public string Name = "";
        public BasicUnit ControlledUnit { get; private set; }

        // Assigns controlled unit.
        public void AssignUnit(BasicUnit unit)
        {
            ControlledUnit = unit;
        }

        // Executed when client starts.
        public override void OnStartClient()
        {
            base.OnStartClient();

            // Notifies MatchController that client started.
            this.PostNotification(Started);
        }

        // Executed when local player connects.
        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();

            // Notifies MatchController that local player connected.
            this.PostNotification(StartedLocal);
        }

        // Executed when player is destroyed.
        private void OnDestroy()
        {
            // Notifies MatchController that player is destroyed.
            this.PostNotification(Destroyed);
        }

        // Runs only on host determining initiative.
        [Command]
        public void CmdInitiative(int firstPlayer)
        {
            // TODO this is called initiative, but is only called for one player and no initiative is determined, should be changed.
            RpcInitiative(firstPlayer);
        }
        // Notifies every client about initiative.
        [ClientRpc]
        private void RpcInitiative(int firstPlayer)
        {
            // Notifies GameController that initiative is determined.
            this.PostNotification(Initiative, firstPlayer);
        }

        [Command]
        public void CmdSetName(NetworkInstanceId id, string name)
        {
            RpcSetName(id, name);
        }

        [ClientRpc]
        private void RpcSetName(NetworkInstanceId id, string name)
        {
            object[] args = new object[2];
            args[0] = id;
            args[1] = name;
            this.PostNotification(SetName, args);
        }

        [Command]
        public void CmdSetClass(NetworkInstanceId id, UnitParameters param)
        {
            RpcSetClass(id, param);
        }

        [ClientRpc]
        private void RpcSetClass(NetworkInstanceId id, UnitParameters param)
        {
            object[] args = new object[2];
            args[0] = id;
            args[1] = param;
            this.PostNotification(SetClass, args);
        }

        // Runs only on host determining initiative.
        [Command]
        public void CmdRestart()
        {
            RpcRestart();
        }
        // Notifies every client about restart.
        [ClientRpc]
        private void RpcRestart()
        {
            // Notifies GameController that restart is started.
            this.PostNotification(Restart);
        }

        [Command]
        public void CmdDisconnect()
        {
            RpcDisconnect();
        }
        [ClientRpc]
        private void RpcDisconnect()
        {
            this.PostNotification(Disconnect);
        }

        // Runs only on host executing action.
        [Command]
        public void CmdExecuteAction(ActionParameters param)
        {
            // Calls every client.
            RpcExecuteAction(param);
        }
        // Notifies every client about action.
        [ClientRpc]
        private void RpcExecuteAction(ActionParameters param)
        {
            // Notifies GameController that action is to be executed.
            this.PostNotification(RequestExecuteAction, param);
        }

        // Runs only on host ending turn.
        [Command]
        public void CmdEndTurn()
        {
            // Calls every client.
            RpcEndTurn();
        }

        // Notifies every client about turn end.
        [ClientRpc]
        private void RpcEndTurn()
        {
            // Notifies GameController that turn end is requested.
            this.PostNotification(RequestEndTurn);
        }

        // Runs only on host creating map.
        [Command]
        public void CmdCreateMap(int unitNum, int width, int height, int minWallNum, int maxWallNum, int minObjectNum, int maxObjectNum)
        {
            MapParameters mapParam = MapGenerator.Generate(unitNum, width, height, minWallNum, maxWallNum, minObjectNum, maxObjectNum);
            // Calls every client.
            RpcCreateMap(mapParam);
        }
        // Notifies every client about map creation.
        [ClientRpc]
        private void RpcCreateMap(MapParameters mapParam)
        {
            // Notifies GameController that map is created.
            this.PostNotification(MapCreated, new Map(mapParam));
        }
    }
}
