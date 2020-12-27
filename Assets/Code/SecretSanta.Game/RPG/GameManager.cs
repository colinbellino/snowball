using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class GameManager : MonoBehaviour
	{
		private BattleStateMachine _machine;

		private void Start()
		{
			_machine = new BattleStateMachine();
		}

		private void Update()
		{
			_machine.Tick();
		}
	}
}
