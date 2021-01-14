using System;
using UnityEngine;
using UnityEngine.UI;

namespace Snowball.Game
{
	public class TitleUI : MonoBehaviour
	{
		[SerializeField] private GameObject _root;
		[SerializeField] private Button _startButton;

		public event Action StartButtonClicked;

		private void Awake()
		{
			Hide();
		}

		private void OnEnable()
		{
			_startButton.onClick.AddListener(OnStartButtonClicked);
		}

		private void OnDisable()
		{
			_startButton.onClick.RemoveListener(OnStartButtonClicked);
		}

		private void OnStartButtonClicked()
		{
			StartButtonClicked?.Invoke();
		}

		public void Show(bool hasSaveFile)
		{
			_startButton.GetComponentInChildren<Text>().text = hasSaveFile ? "Continue" : "Start";
			_root.SetActive(true);
		}

		public void Hide() => _root.SetActive(false);
	}
}
