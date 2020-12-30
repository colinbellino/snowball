using NesScripts.Controls.PathFind;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public static class Vector3IntExtensions
	{
		public static Point ToPoint (this Vector3Int input)
		{
			return new Point(input.x, input.y);
		}
	}
}
