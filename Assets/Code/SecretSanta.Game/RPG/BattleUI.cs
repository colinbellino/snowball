using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.SecretSanta.Game.RPG
{
	public class BattleUI : MonoBehaviour
	{
		[SerializeField] private GameObject _actionsRoot;
		[SerializeField] private Button _attackButton;
		[SerializeField] private Button _moveButton;
		[SerializeField] private Button _waitButton;

		private EventSystem _eventSystem;

		private void Awake()
		{
			_eventSystem = EventSystem.current;
		}

		public void ShowActions() => _actionsRoot.SetActive(true);
		public void HideActions() => _actionsRoot.SetActive(false);

		public void SelectAction(BattleAction action)
		{
			switch (action)
			{
				case BattleAction.Attack:
					_eventSystem.SetSelectedGameObject(_attackButton.gameObject);
					break;
				case BattleAction.Move:
					_eventSystem.SetSelectedGameObject(_moveButton.gameObject);
					break;
				case BattleAction.Wait:
					_eventSystem.SetSelectedGameObject(_waitButton.gameObject);
					break;
			}
		}
	}
}
