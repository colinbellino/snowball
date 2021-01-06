using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Code.SecretSanta.Game.RPG
{
	[CreateAssetMenu(menuName = "Secret Santa/RPG/Unit")]
	public class UnitAuthoring : ScriptableObject
	{
		public int Id;
		public string Name;
		public Color Color = Color.magenta;
		public int MoveRange = 3;
		public int Health = 1;
		public int Speed = 5;

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (name != Name)
			{
				var assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
				AssetDatabase.RenameAsset(assetPath, Name);
				AssetDatabase.SaveAssets();
			}
		}
#endif
	}
}
