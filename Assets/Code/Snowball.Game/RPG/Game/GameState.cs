using System.Collections.Generic;

namespace Snowball.Game
{
	// This is what will be persisted when saving the game
	public class GameState
	{
		public int CurrentEncounter;
		public List<int> EncountersDone;
		public List<int> Party;

		public void Update(GameState save)
		{
			CurrentEncounter = save.CurrentEncounter;
			EncountersDone = save.EncountersDone;
			Party = save.Party;
		}
	}
}
