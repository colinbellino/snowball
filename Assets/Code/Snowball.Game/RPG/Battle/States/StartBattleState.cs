using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using static Snowball.Game.UnitHelpers;

namespace Snowball.Game
{
	public class StartBattleState : BaseBattleState
	{
		public StartBattleState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager) { }

		public override async UniTask Enter()
		{
			await base.Enter();

			var encounter = _database.Encounters[_state.CurrentEncounterId];

			Debug.Log($"Starting battle: {encounter.Name}");
			_board.DrawArea(encounter.Area);
			_board.ShowEncounter();

			var allUnits = new List<Unit>();
			for (var index = 0; index < encounter.Area.AllySpawnPoints.Count; index++)
			{
				if (index > _state.Party.Count - 1)
				{
					break;
				}

				var position = new Vector3Int(encounter.Area.AllySpawnPoints[index].x, encounter.Area.AllySpawnPoints[index].y, 0);
				var unit = Create(_database.Units[_state.Party[index]]);
				SpawnUnitFacade(
					_config.UnitPrefab,
					unit,
					position,
					_state.Guests.Contains(unit.Id) ? Unit.Drivers.Computer : Unit.Drivers.Human,
					Unit.Alliances.Ally,
					Unit.Directions.Right
				);

				allUnits.Add(unit);
			}

			for (var index = 0; index < encounter.Foes.Count; index++)
			{
				var position = new Vector3Int(encounter.Area.FoeSpawnPoints[index].x, encounter.Area.FoeSpawnPoints[index].y, 0);
				var unit = Create(encounter.Foes[index]);
				SpawnUnitFacade(
					_config.UnitPrefab,
					unit,
					position,
					Unit.Drivers.Computer,
					Unit.Alliances.Foe,
					Unit.Directions.Left
				);
				ApplyClothColor(unit.Facade, encounter.TeamColor);

				allUnits.Add(unit);
			}

			_controls.Gameplay.Enable();

			await _transition.EndTransition(Color.white);

			if (encounter.StartConversation.Length > 0)
			{
				await _conversation.Start(encounter.StartConversation, encounter);
			}

			_turnManager.Start(allUnits, encounter.Area, _config.GridOffset, _config.TilesData);

			#if UNITY_EDITOR
			_board.DrawGridWalk(_turnManager.WalkGrid);
			_board.DrawBlockWalk(_turnManager.BlockGrid);
			#endif

			_ = _audio.PlayMusic(_config.BattleMusic, false, 0.5f);

			_machine.Fire(BattleStateMachine.Triggers.BattleStarted);
		}
	}
}
