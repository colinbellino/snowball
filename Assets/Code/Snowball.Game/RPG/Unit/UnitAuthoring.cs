using UnityEngine;
using UnityEngine.Serialization;
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
		public Sprite Sprite;
		[FormerlySerializedAs("Color")] public Color ColorCloth = Color.magenta;
		[FormerlySerializedAs("Color2")] public Color ColorHair = Color.magenta;
		[FormerlySerializedAs("Color3")] public Color ColorSkin = Color.magenta;
		public Unit.Types Type;
		public int MoveRange = 3;
		public int Health = 1;
		public int Speed = 5;
		public int HitAccuracy = 100;
		public int HitRange = 10;
		public int HitDamage = 1;

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
