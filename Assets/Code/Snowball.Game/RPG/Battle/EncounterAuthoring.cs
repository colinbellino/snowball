using System.Collections.Generic;
using Sirenix.OdinInspector;
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
		[Required] public Area Area;
		[Required] public List<UnitAuthoring> Foes;
		[Required] public Color TeamColor = Color.red;
		public ConversationMessage[] StartConversation;
		public ConversationMessage[] EndConversation;

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
