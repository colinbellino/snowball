using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Snowball.Game
{
	[CreateAssetMenu(menuName = "Snowball/RPG/Unit")]
	public class UnitAuthoring : ScriptableObject
	{
		public int Id;
		public string Name = "Annyong";
		public Unit.Types Type;

		[Title("Visuals")]
		public Sprite Sprite;
		public Color ColorCloth = Color.magenta;
		public Color ColorHair = Color.magenta;
		public Color ColorSkin = Color.magenta;

		[Title("Stats")]
		public int MoveRange = 3;
		public int Health = 1;
		public int Speed = 5;
		public int HitAccuracy = 100;
		public int HitRange = 10;
		public int HitDamage = 1;
		public int BuildRange = 1;

		[Title("AI")]
		public ActionPatterns ActionPatterns;
		public Abilities[] Abilities;

#if UNITY_EDITOR
		private void OnValidate()
		{
			var fileName = $"{Name}";
			if (name != fileName)
			{
				var assetPath = AssetDatabase.GetAssetPath(GetInstanceID());
				AssetDatabase.RenameAsset(assetPath, fileName);
				AssetDatabase.SaveAssets();
			}
		}

		[Button]
		private void GenerateId()
		{
			Id = Random.Range(0, int.MaxValue);
			OnValidate();
		}
#endif
	}
}
