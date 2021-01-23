using Sirenix.OdinInspector;
using UnityEngine;

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
			var fileName = $"{Id} - {Name}";
			if (name != fileName)
			{
				var assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
				AssetDatabase.RenameAsset(assetPath, fileName);
				AssetDatabase.SaveAssets();
			}
		}
#endif
	}
}
