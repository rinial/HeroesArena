/*
 * Copyright © 2017 Fazli Jan, Oleg Ivanov
 * The project is licensed under the MIT License.
 */

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace HeroesArena
{
    // Basic player manager. 
    public class MatchController : MonoBehaviour
    {
        // Notifications.
        public const string MatchReady = "MatchController.Ready";

        // TODO this should not be here probably. 
        // Number of players in game.
        public int NumberOfPlayers = 2;

        // Reference to network HUD.
        public NetworkManagerHUD NetworkManagerHud;

        // Checks if match is ready.
        public bool IsReady
        {
            get { return Players.Count == NumberOfPlayers; }
        }
        
        // Local player.
        public PlayerController LocalPlayer;

        // All players.
        public List<PlayerController> Players = new List<PlayerController>();

        // Observes player connections and destructions.
        void OnEnable()
        {
            this.AddObserver(OnPlayerStarted, PlayerController.Started);
            this.AddObserver(OnPlayerStartedLocal, PlayerController.StartedLocal);
            this.AddObserver(OnPlayerDestroyed, PlayerController.Destroyed);

            NetworkManagerHud.showGUI = true;

            // NetworkManager.singleton.connectionConfig.NetworkDropThreshold = 90;
        }

        // Stops observing when disabled.
        void OnDisable()
        {
            this.RemoveObserver(OnPlayerStarted, PlayerController.Started);
            this.RemoveObserver(OnPlayerStartedLocal, PlayerController.StartedLocal);
            this.RemoveObserver(OnPlayerDestroyed, PlayerController.Destroyed);
        }

        // Called from PlayerController when client starts.
        void OnPlayerStarted(object sender, object args)
        {
            // Adds new player to list.
            Players.Add((PlayerController)sender);

            // Checks if match is ready.
            Configure();
        }

        // Called from PlayerController when local player connects.
        void OnPlayerStartedLocal(object sender, object args)
        {
            // Remembers reference for local player.
            LocalPlayer = (PlayerController)sender;

            // Checks if match is ready.
            Configure();
        }

        // Called from PlayerController when player is destroyed.
        void OnPlayerDestroyed(object sender, object args)
        {
            // Clears local player reference if local player is destroyed.
            PlayerController pc = (PlayerController)sender;
            if (LocalPlayer == pc)
                LocalPlayer = null;

            // Removes destroyed player from list.
            if (Players.Contains(pc))
                Players.Remove(pc);
        }

        // Checks if the game is set and finishes preparations. 
        void Configure()
        {
            // Notifies GameController that match is ready.
            if (LocalPlayer != null && Players.Count == NumberOfPlayers)
            {
                NetworkManagerHud.showGUI = false;
                this.PostNotification(MatchReady);
            }
        }
    }
}