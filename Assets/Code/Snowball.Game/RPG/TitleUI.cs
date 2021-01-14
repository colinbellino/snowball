using System;
using UnityEngine;
using UnityEngine.UI;

namespace Snowball.Game
{
	public class TitleUI : MonoBehaviour
	{
		[SerializeField] private GameObject _root;
		[SerializeField] private Button _startButton;
		[SerializeField] private Button _newGameButton;

		public event Action StartButtonClicked;
		public event Action NewGameButtonClicked;

		private void Awake()
		{
			Hide();
		}

		private void OnEnable()
		{
			_startButton.onClick.AddListener(OnStartButtonClicked);
			_newGameButton.onClick.AddListener(OnNewGameButtonClicked);
		}

		private void OnDisable()
		{
			_startButton.onClick.RemoveListener(OnStartButtonClicked);
			_newGameButton.onClick.RemoveListener(OnNewGameButtonClicked);
		}

		private void OnStartButtonClicked() => StartButtonClicked?.Invoke();

		private void OnNewGameButtonClicked() => NewGameButtonClicked?.Invoke();

		public void Show(bool hasSaveFile)
		{
			_startButton.GetComponentInChildren<Text>().text = hasSaveFile ? "Continue" : "Start";
			_newGameButton.gameObject.SetActive(hasSaveFile);
			_root.SetActive(true);
		}

		public void Hide() => _root.SetActive(false);
	}
}
