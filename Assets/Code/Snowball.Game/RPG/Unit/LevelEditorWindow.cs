#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Snowball.Game
{
	public class LevelEditorWindow : OdinEditorWindow
	{
		[PropertyOrder(1)]
		[SerializeField]
		[ListDrawerSettings(IsReadOnly = true, DraggableItems = false, Expanded = true, ShowIndexLabels = false, ShowPaging = false, ShowItemCount = false, HideRemoveButton = true)]
		private List<AreaEditor> areas;

		private AreaEditor _current;

		private bool IsMainScene => SceneManager.GetActiveScene().name == "Main";

		[MenuItem("Snowball/Level Editor")]
		private static void OpenWindow()
		{
			var window = GetWindow<LevelEditorWindow>();
			window.Refresh();
			window.Show();
			UnityEditorEventUtility.OnProjectChanged += window.Refresh;
		}

		[OnInspectorInit]
		private void OnInspectorInit()
		{
			Refresh();
		}

		[PropertyOrder(0)]
		[ButtonGroup("Actions")] [Button("Back to main scene", ButtonSizes.Medium)]
		[DisableIf(nameof(IsMainScene))]
		private void OpenMainScene()
		{
			EditorSceneManager.CloseScene(SceneManager.GetActiveScene(), false);
			EditorSceneManager.OpenScene("Assets/Game/Scenes/Main.unity");

			if (_current != null)
			{
				_current.Opened = false;
			}
		}

		private void Refresh()
		{
			var areas = Resources.LoadAll<Area>("Areas");
			this.areas = areas.Select(area => new AreaEditor{ Area = area, Window = this }).ToList();
		}

		public void Open(AreaEditor areaEditor)
		{
			if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
			{
				var config = Resources.Load<GameConfig>("Game Config");

				if (_current != null)
				{
					_current.Opened = false;
				}

				EditorSceneManager.OpenScene("Assets/Game/Scenes/Level Editor.unity");
				var areaCreation = FindObjectOfType<AreaCreation>();

				areaCreation.Init(config);
				areaCreation.Load(areaEditor.Area);
				areaEditor.Opened = true;

				_current = areaEditor;
			}
		}

		public void Save()
		{
			var areaCreation = FindObjectOfType<AreaCreation>();
			areaCreation.Save(_current.Area);
		}
	}

	[HideReferenceObjectPicker]
	public class AreaEditor
	{
		public bool Opened { get; set; }
		public LevelEditorWindow Window { get; set; }

		[HorizontalGroup("Base")]
		[VerticalGroup("Base/A")]
		[ReadOnly]
		[HideLabel] public Area Area;

		[HorizontalGroup("Base/B", 80)]
		[VerticalGroup("Base/B/A")]
		[HideIf("Opened")]
		[Button]
		private void Open()
		{
			Window.Open(this);
		}

		[VerticalGroup("Base/B/A")]
		[ShowIf("Opened")]
		[Button, GUIColor(0f, 1f, 0f)]
		private void Save()
		{
			Window.Save();
		}
	}
}
#endif
