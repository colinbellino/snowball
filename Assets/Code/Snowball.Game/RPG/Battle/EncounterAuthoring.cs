using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Snowball.Game
{
	[CreateAssetMenu(menuName = "Snowball/RPG/Encounter")]
	public class EncounterAuthoring : ScriptableObject
	{
		public int Id;
		public string Name;
		public Area Area;
		public List<UnitAuthoring> Foes;
		public ConversationMessage[] StartConversation;

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
