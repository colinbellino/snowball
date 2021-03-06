﻿using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using static Snowball.Game.UnitHelpers;

namespace Snowball.Game
{
	public class MeltAbility : IAbility
	{
		public bool RequiresUnitTarget => false;
		public bool DoesDamage => false;

		public bool IsValidTarget(Unit actor, Unit target)
		{
			return true;
		}

		public List<Vector3Int> GetAreaOfEffect(Vector3Int destination)
		{
			return new List<Vector3Int> { destination };
		}

		public List<Vector3Int> GetTilesInRange(Vector3Int origin, Unit actor, TurnManager turnManager)
		{
			return new List<Vector3Int> { origin };
		}

		public async UniTask Perform(TurnManager turnManager)
		{
			var actor = turnManager.Turn.Unit;

			actor.HealthCurrent = 0;

			await AnimateMelt(actor.Facade);
			turnManager.SortedUnits.Remove(actor);
		}
	}
}
