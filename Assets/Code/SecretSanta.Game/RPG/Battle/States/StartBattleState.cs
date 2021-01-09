using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class StartBattleState : BaseBattleState
	{
		public StartBattleState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager)
		{
		}

		public override UniTask Enter()
		{
			base.Enter();

			var encounter = _database.Encounters[_state.CurrentEncounterId];

			Debug.Log($"Starting battle: {encounter.Name}");
			_board.DrawArea(encounter.Area);
			_board.ShowEncounter();

			var allUnits = new List<Unit>();
			for (var index = 0; index < encounter.Area.AllySpawnPoints.Count; index++)
			{
				var position = new Vector3Int(encounter.Area.AllySpawnPoints[index].x,
					encounter.Area.AllySpawnPoints[index].y, 0);
				var unit = _state.Party[index];
				unit.SetFacade(UnitHelpers.SpawnUnitFacade(_config.UnitPrefab, unit, position, true,
					Unit.Directions.Right));

				allUnits.Add(unit);
			}

			for (var index = 0; index < encounter.Foes.Count; index++)
			{
				var position = new Vector3Int(encounter.Area.FoeSpawnPoints[index].x,
					encounter.Area.FoeSpawnPoints[index].y, 0);
				var unit = new Unit(encounter.Foes[index]);
				unit.SetFacade(UnitHelpers.SpawnUnitFacade(_config.UnitPrefab, unit, position, false,
					Unit.Directions.Left));

				allUnits.Add(unit);
			}

			_controls.Enable();
			_turnManager.Start(allUnits, encounter.Area, _config.TilesData);

#if UNITY_EDITOR
			_board.DrawGridWalk(_turnManager.WalkGrid);
			_board.DrawBlockWalk(_turnManager.BlockGrid);
#endif

			_machine.Fire(BattleStateMachine.Triggers.BattleStarted);

			return default;
		}
	}
}
