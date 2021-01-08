using System.Collections.Generic;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class AttackResult
	{
		public Unit Attacker;
		public List<Vector3Int> Path;
		public List<Unit> Targets;
	}
}
