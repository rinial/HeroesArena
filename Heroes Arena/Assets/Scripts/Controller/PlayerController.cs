using UnityEngine;
using UnityEngine.Networking;

namespace HeroesArena
{
    // Represents one player in network.
    public class PlayerController : NetworkBehaviour
    {
        public const string Started = "PlayerController.Start";
        public const string StartedLocal = "PlayerController.StartedLocal";
        public const string Destroyed = "PlayerController.Destroyed";
        public const string Initiative = "PlayerController.Initiative";
        public const string RequestMakeMove = "PlayerController.RequestMakeMove";
        public const string RequestEndTurn = "PlayerController.RequestEndTurn";
        
        // TODO change set for name restrictions
        public string Name { get; set; }
        // TODO make it list of we need multiple units for one player.
        public BasicUnit ControlledUnit { get; private set; }
        public void AssignUnit(BasicUnit unit)
        {
            ControlledUnit = unit;
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            this.PostNotification(Started);
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            this.PostNotification(StartedLocal);
        }

        void OnDestroy()
        {
            this.PostNotification(Destroyed);
        }

        // TODO
        [Command]
        public void CmdInitiative()
        {
            RpcInitiative(Random.value);
        }
        [ClientRpc]
        private void RpcInitiative(float init)
        {

            this.PostNotification(Initiative, init);
        }

        [Command]
        public void CmdMakeMove(Coordinates pos)
        {
            RpcMakeMove(pos);
        }
        [ClientRpc]
        private void RpcMakeMove(Coordinates pos)
        {
            this.PostNotification(RequestMakeMove, pos);
        }

        [Command]
        public void CmdEndTurn()
        {
            RpcEndTurn();
        }
        [ClientRpc]
        private void RpcEndTurn()
        {
            this.PostNotification(RequestEndTurn);
        }

        // TODO
    }
}
