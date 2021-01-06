using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class StartBattleState : BaseBattleState, IState
	{
		public StartBattleState(BattleStateMachine machine, TurnManager turnManager) : base(machine, turnManager)
		{
		}

		public async Task Enter(object[] args)
		{
			var encounter = _database.Encounters[_state.CurrentEncounterId];

			Debug.Log($"Starting battle: {encounter.Name}");
			_board.DrawArea(encounter.Area);
			_board.ShowEncounter();

			var allUnits = new List<Unit>();
			for (var index = 0; index < _state.Party.Count; index++)
			{
				var position = new Vector3Int(encounter.Area.AllySpawnPoints[index].x,
					encounter.Area.AllySpawnPoints[index].y, 0);
				var unit = _state.Party[index];
				unit.SetFacade(SpawnFacade(unit, position, true));

				allUnits.Add(unit);
			}

			for (var index = 0; index < encounter.Foes.Count; index++)
			{
				var position = new Vector3Int(encounter.Area.FoeSpawnPoints[index].x, encounter.Area.FoeSpawnPoints[index].y, 0);
				var unit = new Unit(encounter.Foes[index]);
				unit.SetFacade(SpawnFacade(unit, position, false));

				allUnits.Add(unit);
			}

			_controls.Enable();
			TurnManager.Start(allUnits, encounter.Area, _config.TilesData);

			#if UNITY_EDITOR
			_board.DrawGridWalk(TurnManager.WalkGrid);
			#endif

			_machine.Fire(BattleStateMachine.Triggers.BattleStarted);
		}

		public async Task Exit() { }

		public void Tick() { }

		private UnitFacade SpawnFacade(Unit unit, Vector3Int position, bool isPlayerControlled)
		{
			var facade = GameObject.Instantiate(_config.UnitPrefab);
			unit.GridPosition = position;
			unit.Direction = isPlayerControlled ? Vector3Int.right : Vector3Int.left;
			unit.IsPlayerControlled = isPlayerControlled;
			facade.Initialize(unit);

			return facade;
		}
	}
}
