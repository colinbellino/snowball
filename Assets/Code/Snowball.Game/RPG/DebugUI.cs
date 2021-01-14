using UnityEngine;

namespace Snowball.Game
{
	public class DebugUI : MonoBehaviour
	{
		public void Save()
		{
			SaveHelpers.SaveToFile(Game.Instance.State);
		}

		public void Load()
		{
			Game.Instance.State.Update(SaveHelpers.LoadFromFile());
		}
	}
}
