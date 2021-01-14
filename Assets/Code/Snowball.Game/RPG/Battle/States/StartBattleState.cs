using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Snowball.Game
{
	public class StartBattleState : BaseBattleState
	{
		public StartBattleState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public override async UniTask Enter()
		{
			base.Enter();

			var encounter = _database.Encounters[_state.CurrentEncounter];

			Debug.Log($"Starting battle: {encounter.Name}");
			_board.DrawArea(encounter.Area);
			_board.ShowEncounter();

			var allUnits = new List<Unit>();
			for (var index = 0; index < encounter.Area.AllySpawnPoints.Count; index++)
			{
				var position = new Vector3Int(encounter.Area.AllySpawnPoints[index].x, encounter.Area.AllySpawnPoints[index].y, 0);
				var unit = new Unit(_database.Units[_state.Party[index]]);
				var facade = UnitHelpers.SpawnUnitFacade(
					_config.UnitPrefab,
					unit,
					position,
					Unit.Drivers.Human,
					Unit.Alliances.Ally,
					Unit.Directions.Right
				);
				unit.SetFacade(facade);

				allUnits.Add(unit);
			}

			for (var index = 0; index < encounter.Foes.Count; index++)
			{
				var position = new Vector3Int(encounter.Area.FoeSpawnPoints[index].x, encounter.Area.FoeSpawnPoints[index].y, 0);
				var unit = new Unit(encounter.Foes[index]);
				var facade = UnitHelpers.SpawnUnitFacade(
					_config.UnitPrefab,
					unit,
					position,
					Unit.Drivers.Computer,
					Unit.Alliances.Foe,
					Unit.Directions.Left
				);
				unit.SetFacade(facade);

				allUnits.Add(unit);
			}

			_controls.Enable();
			_turnManager.Start(allUnits, encounter.Area, _config.TilesData);

#if UNITY_EDITOR
			_board.DrawGridWalk(_turnManager.WalkGrid);
			_board.DrawBlockWalk(_turnManager.BlockGrid);
#endif

			await Game.Instance.Transition.EndTransition(Color.white);

			_machine.Fire(BattleStateMachine.Triggers.BattleStarted);
		}
	}
}
