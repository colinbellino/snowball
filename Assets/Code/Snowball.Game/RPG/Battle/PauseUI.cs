using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Snowball.Game
{
	public class PauseUI : MonoBehaviour
	{
		[SerializeField] [Required] private GameObject _menuRoot;
		[SerializeField] [Required] private Button _continueButton;
		[SerializeField] [Required] private Button _quitButton;

		public event Action ContinueClicked;
		public event Action QuitClicked;

		private void Awake()
		{
			Hide();
		}

		public void Show()
		{
			_menuRoot.SetActive(true);

			_continueButton.onClick.AddListener(OnContinueClicked);
			_quitButton.onClick.AddListener(OnQuitClicked);
		}

		public void Hide()
		{
			_menuRoot.SetActive(false);

			_continueButton.onClick.RemoveListener(OnContinueClicked);
			_quitButton.onClick.RemoveListener(OnQuitClicked);
		}

		private void OnContinueClicked()
		{
			ContinueClicked?.Invoke();
		}

		private void OnQuitClicked()
		{
			QuitClicked?.Invoke();
		}
	}
}
