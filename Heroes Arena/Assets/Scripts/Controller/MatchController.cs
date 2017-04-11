using UnityEngine;
using System.Collections.Generic;

namespace HeroesArena
{
    public class MatchController : MonoBehaviour
    {
        public const string MatchReady = "MatchController.Ready";

        // TODO should it be here? No.
        public const int NumberOfPlayers = 3;

        // Checks if match is ready.
        public bool IsReady
        {
            get { return Players.Count == NumberOfPlayers; }
        }
        
        public PlayerController LocalPlayer;

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

            if (Players.Contains(pc))
                Players.Remove(pc);
        }

        // Checks if the game is set and finishes preparations. 
        void Configure()
        {
            if (LocalPlayer != null && Players.Count == NumberOfPlayers)
                this.PostNotification(MatchReady);
        }
    }
}