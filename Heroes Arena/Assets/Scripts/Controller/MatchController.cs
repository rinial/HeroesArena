using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HeroesArena
{
    public class MatchController : MonoBehaviour
    {
        public const string MatchReady = "MatchController.Ready";

        // Checks if match is ready.
        public bool IsReady
        {
            get { return LocalPlayer != null && RemotePlayer != null; }
        }

        // TODO change smth here for more players
        public PlayerController LocalPlayer;
        public PlayerController RemotePlayer;
        public PlayerController HostPlayer;
        public PlayerController ClientPlayer;
        public List<PlayerController> Players = new List<PlayerController>();

        void OnEnable()
        {
            this.AddObserver(OnPlayerStarted, PlayerController.Started);
            this.AddObserver(OnPlayerStartedLocal, PlayerController.StartedLocal);
            this.AddObserver(OnPlayerDestroyed, PlayerController.Destroyed);
        }

        void OnDisable()
        {
            this.RemoveObserver(OnPlayerStarted, PlayerController.Started);
            this.RemoveObserver(OnPlayerStartedLocal, PlayerController.StartedLocal);
            this.RemoveObserver(OnPlayerDestroyed, PlayerController.Destroyed);
        }

        void OnPlayerStarted(object sender, object args)
        {
            Players.Add((PlayerController)sender);
            Configure();
        }

        void OnPlayerStartedLocal(object sender, object args)
        {
            LocalPlayer = (PlayerController)sender;
            Configure();
        }

        void OnPlayerDestroyed(object sender, object args)
        {
            PlayerController pc = (PlayerController)sender;
            if (LocalPlayer == pc)
                LocalPlayer = null;
            if (RemotePlayer == pc)
                RemotePlayer = null;
            if (HostPlayer == pc)
                HostPlayer = null;
            if (ClientPlayer == pc)
                ClientPlayer = null;
            if (Players.Contains(pc))
                Players.Remove(pc);
        }

        void Configure()
        {
            if (LocalPlayer == null || Players.Count < 2)
                return;

            for (int i = 0; i < Players.Count; ++i)
            {
                if (Players[i] != LocalPlayer)
                {
                    RemotePlayer = Players[i];
                    break;
                }
            }

            HostPlayer = (LocalPlayer.isServer) ? LocalPlayer : RemotePlayer;
            ClientPlayer = (LocalPlayer.isServer) ? RemotePlayer : LocalPlayer;

            this.PostNotification(MatchReady);
        }
    }
}