using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Snowball.Game
{
	public class SelectUnitState : BaseBattleState
	{
		public SelectUnitState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public override async UniTask Enter()
		{
			base.Enter();

			_turnManager.NextTurn();

			while (_turn == null)
			{
				#if UNITY_EDITOR
				if (_config.DebugTurn)
				{
					foreach (var unit in _turnManager.SortedUnits)
					{
						unit.Facade.GetComponent<UnitDebugger>().Init(unit);
					}

					Debug.Log("press SPACE to continue");
					await UniTask.WaitUntil(() => UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame);
				}
				#endif

				_turnManager.NextTurn();
			}

			_machine.Fire(BattleStateMachine.Triggers.UnitSelected);
		}
	}
}
