using System.Collections.Generic;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class Turn
	{
		public UnitComponent Unit;
		public bool HasMoved;
		public bool HasActed;
		public Vector3Int InitialPosition;

		public List<Vector3Int> AttackTargets;
		public List<Vector3Int> AttackPath;
		public List<Vector3Int> MovePath;
	}
}
