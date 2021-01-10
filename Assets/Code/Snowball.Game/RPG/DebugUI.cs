using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Snowball.Game
{
	public class DebugUI : MonoBehaviour
	{
		[Serializable]
		public class DebugButtons : UnitySerializedDictionary<int, Button> { }

		[SerializeField] private GameObject _root;
		[SerializeField] private DebugButtons _debugButtons;

		public event Action<int> OnDebugButtonClicked;

		private void Awake()
		{
			Hide();
		}

		private void OnEnable()
		{
			foreach (var item in _debugButtons)
			{
				item.Value.onClick.AddListener(OnDebugClicked(item.Key));
			}
		}

		private void OnDisable()
		{
			foreach (var item in _debugButtons)
			{
				item.Value.onClick.RemoveListener(OnDebugClicked(item.Key));
			}
		}

		private UnityAction OnDebugClicked(int key)
		{
			return () => OnDebugButtonClicked?.Invoke(key);
		}

		public void Show() => _root.SetActive(true);

		public void Hide() => _root.SetActive(false);
	}
}
