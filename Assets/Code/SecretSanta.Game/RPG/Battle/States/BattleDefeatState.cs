using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class BattleDefeatState : BaseBattleState, IState
	{
		public BattleDefeatState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public UniTask Enter(object[] args)
		{
			Debug.Log("Battle lost :(");

			_machine.Fire(BattleStateMachine.Triggers.Done);

			return default;
		}

		public UniTask Exit() { return default; }

		public void Tick() { }
	}
}
