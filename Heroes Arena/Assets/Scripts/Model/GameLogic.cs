using UnityEngine;
using System.Collections.Generic;

namespace HeroesArena
{
	// Interacts with other scripts and represents the logic behind gameplay.
	public class GameLogic
	{
		private Dictionary<PlayerID, Player> _players;
		private List<PlayerID> _turnOrder;
		// TODO decide if these should be stored this way or just sent to players.
		private Dictionary<PlayerID, Map> _seenMaps;
		public Map Map { get; private set; }
		public PlayerID Control { get; private set; }
		// TODO change to make TDM instead of FFA.
		public PlayerID Winner { get; private set; }

		// TODO
		// GameLogic() constructor
		// Reset()
		// Player actions
		//     list of actions
		// ChangeTurn()
		// CheckForGameOver()
		//     CheckForWin()
		//     CheckForStalemate() if we ever need it
		// etc.
	}
}
