using System.Collections.Generic;

namespace Snowball.Game
{
	// This is what will be persisted when saving the game
	public class GameState
	{
		public string Version;
		public int CurrentEncounterId;
		public List<int> EncountersDone;
		public List<int> Party;
		public List<int> Guests;

		public void Set(GameState save)
		{
			Version = save.Version;
			CurrentEncounterId = save.CurrentEncounterId;
			EncountersDone = save.EncountersDone;
			Party = save.Party;
			Guests = save.Guests;
		}
	}
}
