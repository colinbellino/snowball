using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Snowball.Game
{
	public class SelectUnitState : BaseBattleState
	{
		public SelectUnitState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public override async UniTask Enter()
		{
			await base.Enter();

			_turnManager.NextTurn();

			while (_turn == null)
			{
				if (GameConfig.GetDebug(_config.DebugTurn))
				{
					foreach (var unit in _turnManager.SortedUnits)
					{
						unit.Facade.GetComponent<UnitDebugger>().Init(unit);
					}

					Debug.Log("press SPACE to continue");
					await UniTask.WaitUntil(() => UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame);
				}

				_turnManager.NextTurn();
			}

			_ui.ShowActorInfos(_turn.Unit, _database.Encounters[_state.CurrentEncounterId].TeamColor);

			_machine.Fire(BattleStateMachine.Triggers.UnitSelected);
		}
	}
}
