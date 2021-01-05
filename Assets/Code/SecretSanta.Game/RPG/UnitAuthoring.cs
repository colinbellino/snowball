using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	[CreateAssetMenu(menuName = "Secret Santa/RPG/Unit")]
	public class UnitAuthoring : ScriptableObject
	{
		public int Id;
		public string Name => name;
		public Color Color = Color.magenta;
		public int MoveRange = 3;
	}

	public class UnitRuntime
	{
		public int Id;
		public string Name;
		public Color Color;
		public int MoveRange;
		public Vector3Int GridPosition;
		public Vector3Int Direction;
		public bool IsPlayerControlled;

		public UnitComponent Facade;

		public UnitRuntime(UnitAuthoring authoring)
		{
			Id = authoring.Id;
			Name = authoring.Name;
			Color = authoring.Color;
			MoveRange = authoring.MoveRange;
		}
	}
}
