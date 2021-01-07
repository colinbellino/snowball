using System.Collections.Generic;

namespace Code.SecretSanta.Game.RPG
{
	// This is what will be persisted when saving the game
	public class GameState
	{
		public int CurrentEncounterId;
		public List<Unit> Party;
	}
}
