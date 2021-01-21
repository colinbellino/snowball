using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Snowball.Game
{
	public interface IAbility
	{
		bool RequiresUnitTarget { get; }

		bool IsValidTarget(Unit actor, Unit target);
		List<Vector3Int> GetAreaOfEffect(Vector3Int destination);
		List<Vector3Int> GetTilesInRange(Vector3Int origin, Unit actor, TurnManager turnManager);
		UniTask Perform(TurnManager turnManager);
	}
}
