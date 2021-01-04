using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.SecretSanta.Game.RPG
{
	public class StartBattleState : BaseBattleState, IState
	{
		private readonly EncounterAuthoring _encounter;

		public StartBattleState(BattleStateMachine machine, EncounterAuthoring encounter) : base(machine)
		{
			_encounter = encounter;
		}

		public async Task Enter(object[] args)
		{
			Helpers.RenderArea(_encounter.Area, _areaTilemap, _config.TilesData);

			var allUnits = new List<UnitComponent>();
			for (var index = 0; index < _encounter.Allies.Count; index++)
			{
				var point = _encounter.Area.AllySpawnPoints[index];
				allUnits.Add(SpawnUnit(_encounter.Allies[index], new Vector3Int(point.x, point.y, 0), true));
			}
			for (var index = 0; index < _encounter.Foes.Count; index++)
			{
				var point = _encounter.Area.FoeSpawnPoints[index];
				allUnits.Add(SpawnUnit(_encounter.Foes[index], new Vector3Int(point.x, point.y, 0)));
			}

			Notification.Send("BattleStarted");
			_battle.Start(allUnits, _encounter.Area, _config.TilesData);
			_controls.Enable();

			#if UNITY_EDITOR
			var gridWalkTilemap = GameObject.Find("GridWalk").GetComponent<Tilemap>();
			Helpers.RenderGridWalk(_battle.WalkGrid, gridWalkTilemap, _config.EmptyTile);
			#endif

			_machine.Fire(BattleStateMachine.Triggers.BattleStarted);
		}

		public async Task Exit() { }

		public void Tick() { }

		private UnitComponent SpawnUnit(Unit data, Vector3Int position, bool isPlayerControlled = false)
		{
			var unit = GameObject.Instantiate(_config.UnitPrefab);
			unit.SetGridPosition(position);
			unit.Turn(isPlayerControlled ? Vector3Int.right : Vector3Int.left);
			unit.UpdateData(data, isPlayerControlled);
			return unit;
		}
	}
}
