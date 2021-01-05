using System.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class EndBattleState : BaseBattleState, IState
	{
		public EndBattleState(BattleStateMachine machine) : base(machine) { }

		public async Task Enter(object[] args)
		{
			Notification.Send("BattleEnded");
			Debug.LogWarning("Battle over!");
		}

		public async Task Exit() { }

		public void Tick() { }
	}
}
