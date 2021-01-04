using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class GameManager : MonoBehaviour
	{
		private BattleStateMachine _battle;

		private void OnEnable()
		{
			Game.Instance.DebugUI.Show();
			Game.Instance.WorldmapRoot.SetActive(false);

			Game.Instance.DebugUI.OnDebugButtonClicked += OnDebugButtonClicked;
		}

		private void OnDisable()
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
					_battle = new BattleStateMachine(Game.Instance.Config.Encounters[0]);
					break;
				case 1:
					Game.Instance.WorldmapRoot.SetActive(true);
					// load world map
					break;
			}
		}
	}
}
