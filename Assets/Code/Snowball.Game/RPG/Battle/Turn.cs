using System.Collections.Generic;
using UnityEngine;

namespace Snowball.Game
{
	public class Turn
	{
		public Unit Unit;
		public bool HasMoved;
		public bool HasActed;
		public Plan Plan;
	}

	public enum TurnActions { None, Wait, Attack, Build, Melt }

	public class Plan
	{
		public TurnActions Action;
		public Vector3Int ActionDestination;
		public Vector3Int MoveDestination;
		public bool NeedsToMove;
	}

	public struct ActionOption
	{
		public Vector3Int ActionTarget;
		public List<Vector3Int> AreaTargets;
		public Vector3Int MoveTarget;
	}
}
