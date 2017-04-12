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
        public const string RequestMakeMove = "PlayerController.RequestMakeMove";
        public const string RequestEndTurn = "PlayerController.RequestEndTurn";
        
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

        // Runs only on host making move.
        [Command]
        public void CmdMakeMove(Coordinates pos)
        {
            // Calls every client.
            RpcMakeMove(pos);
        }
        // Notifies every client about move.
        [ClientRpc]
        private void RpcMakeMove(Coordinates pos)
        {
            // Notifies GameController that move is requested.
            this.PostNotification(RequestMakeMove, pos);
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
    }
}
