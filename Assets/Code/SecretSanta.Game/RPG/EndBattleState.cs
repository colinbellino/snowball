using System.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class EndBattleState : IState
	{
		private readonly BattleStateMachine _machine;

		public EndBattleState(BattleStateMachine machine)
		{
			_machine = machine;
		}

		public async Task Enter(object[] args)
		{
			Notification.Send("BattleEnded");
			Debug.LogWarning("Battle over!");
		}

		public async Task Exit() { }

		public void Tick() { }
	}
}
