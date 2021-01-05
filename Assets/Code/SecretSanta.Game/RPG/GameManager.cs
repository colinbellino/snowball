using System.Collections.Generic;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class GameManager : MonoBehaviour
	{
		private BattleStateMachine _battle;

		private void Start()
		{
			DatabaseHelpers.LoadFromResources(Game.Instance.Database);

			var party = new List<Unit>();
			foreach (var unitId in Game.Instance.Config.StartingParty)
			{
				party.Add(new Unit(Game.Instance.Database.Units[unitId]));
			}
			Game.Instance.State.Party = party;

			Game.Instance.DebugUI.Show();
			Game.Instance.WorldmapRoot.SetActive(false);

			Game.Instance.DebugUI.OnDebugButtonClicked += OnDebugButtonClicked;
		}

		private void OnDestroy()
		{
			Game.Instance.DebugUI.OnDebugButtonClicked -= OnDebugButtonClicked;
		}

		private void Update()
		{
			_battle?.Tick();
		}

		private void OnDebugButtonClicked(int key)
		{
			Game.Instance.DebugUI.Hide();

			switch (key)
			{
				case 0:
					_battle = new BattleStateMachine();
					break;
				case 1:
					Game.Instance.WorldmapRoot.SetActive(true);
					// load world map
					break;
			}
		}
	}
}
