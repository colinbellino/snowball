using System.Collections.Generic;
using UnityEngine;

namespace Snowball.Game
{
	public class Turn
	{
		public Unit Unit;
		public bool HasMoved;
		public bool HasActed;
		public Plan Plan = new Plan();
	}

	public enum TurnActions { None, Attack, Build, Melt }

	public class Plan
	{
		public TurnActions Action;
		public List<Vector3Int> ActionTargets;
		public Vector3Int? ActionDestination;
		public Vector3Int? MoveDestination;
	}
}
