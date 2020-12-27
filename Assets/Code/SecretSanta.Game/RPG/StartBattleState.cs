using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class StartBattleState : IState
	{
		private readonly BattleStateMachine _machine;

		public StartBattleState(BattleStateMachine machine)
		{
			_machine = machine;
		}

		public async Task Enter(object[] args)
		{
			var encounter = Game.Instance.Config.Encounters[0];
			Helpers.LoadArea(encounter.Area, Game.Instance.Tilemap, Game.Instance.Config.Tiles);

			var allUnits = new List<UnitComponent>();
			for (var index = 0; index < encounter.Allies.Count; index++)
			{
				var point = encounter.Area.AllySpawnPoints[index];
				allUnits.Add(SpawnUnit(encounter.Allies[index], new Vector3Int(point.x, point.y, 0), true));
			}
			for (var index = 0; index < encounter.Foes.Count; index++)
			{
				var point = encounter.Area.FoeSpawnPoints[index];
				allUnits.Add(SpawnUnit(encounter.Foes[index], new Vector3Int(point.x, point.y, 0)));
			}

			Notification.Send("BattleStarted");
			Game.Instance.Battle.Start(allUnits);
			Game.Instance.Controls.Enable();

			_machine.Fire(BattleStateMachine.Triggers.BattleStarted);
		}

		public async Task Exit() { }

		public void Tick() { }

		private static UnitComponent SpawnUnit(Unit data, Vector3Int position, bool isPlayerControlled = false)
		{
			var unit = GameObject.Instantiate(Game.Instance.Config.UnitPrefab);
			unit.SetGridPosition(position);
			_ = unit.Turn(isPlayerControlled ? Vector3Int.right : Vector3Int.left);
			unit.UpdateData(data, isPlayerControlled);
			return unit;
		}
	}
}
