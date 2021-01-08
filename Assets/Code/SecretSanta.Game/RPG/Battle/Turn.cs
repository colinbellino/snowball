using System.Collections.Generic;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class Turn
	{
		public Unit Unit;
		public bool HasMoved;
		public bool HasActed;
		public Actions Action;

		public List<Vector3Int> ActionTargets;
		public Vector3Int? ActionDestination;
		public List<Vector3Int> MovePath;

		public enum Actions { None, Attack, Build }
	}
}
