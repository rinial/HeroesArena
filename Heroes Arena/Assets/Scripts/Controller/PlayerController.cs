using System.Diagnostics;
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
        public const string RequestEndTurn = "PlayerController.RequestEndTurn";
        public const string RequestExecuteAction = "PlayerController.RequestExecuteAction";
        public const string RequestMap = "PlayerController.RequestMap";
        public const string MapCreated = "PlayerController.MapCreated";

        // Some basic properties.
        // TODO change set for name restrictions.
        public string Name;
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
        void OnDestroy()
        {
            // Notifies MatchController that player is destroyed.
            this.PostNotification(Destroyed);
        }

        // Runs only on host determining initiative.
        [Command]
        public void CmdInitiative()
        {
            // TODO this is called initiative, but is only called for one player and no initiative is determined, should be changed.
            RpcInitiative();
        }
        // Notifies every client about initiative.
        [ClientRpc]
        private void RpcInitiative()
        {
            // Notifies GameController that initiative is determined.
            this.PostNotification(Initiative);
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
        public void CmdCreateMap(int unitNum, int width, int height, int minWallNum, int maxWallNum, int minPotionNum, int maxPotionNum)
        {
            MapParameters mapParam = MapGenerator.Generate(unitNum, width, height, minWallNum, maxWallNum, minPotionNum, maxPotionNum);
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
