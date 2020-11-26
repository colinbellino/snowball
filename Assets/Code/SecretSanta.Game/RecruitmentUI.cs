using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RecruitmentUI : MonoBehaviour
{
	[SerializeField] private RecruitPanelUI recruitPanelUIPrefab;
	[SerializeField] private Transform _recruitsPanel;
	[SerializeField] private Transform _namePanel;
	[SerializeField] private InputField _nameInput;
	[SerializeField] private Button _nameSubmitButton;

	private Dictionary<string, RecruitPanelUI> _recruitPanels;
	private EventSystem _eventSystem;

	public event Action<string> RecruitSelected;
	public event Action<string> RecruitNamed;

	private void Awake()
	{
		_recruitPanels = new Dictionary<string, RecruitPanelUI>();
		_eventSystem = FindObjectOfType<EventSystem>();
		_nameSubmitButton.onClick.AddListener(() => RecruitNamed?.Invoke(_nameInput.text));
	}

	public void ShowRecruits(Recruit[] recruits)
	{
		foreach (var recruit in recruits)
		{
			var panel = Instantiate(recruitPanelUIPrefab, _recruitsPanel);
			panel.SetRecruit(recruit);
			panel.Button.onClick.AddListener(() => RecruitSelected?.Invoke(recruit.Id));
			_recruitPanels.Add(recruit.Id, panel);
		}
		_recruitsPanel.gameObject.SetActive(true);

		_eventSystem.SetSelectedGameObject(_recruitPanels[recruits[0].Id].Button.gameObject, null);
	}

	public void HideRecruits()
	{
		foreach (Transform child in _recruitsPanel)
		{
			Destroy(child.gameObject);
		}
		_recruitPanels.Clear();

		_recruitsPanel.gameObject.SetActive(false);
	}

	public void ShowName()
	{
		_namePanel.gameObject.SetActive(true);
		_eventSystem.SetSelectedGameObject(_nameSubmitButton.gameObject, null);
	}

	public void HideName()
	{
		_namePanel.gameObject.SetActive(false);
	}

	public void FocusRecruit(string id)
	{
		_recruitPanels[id].Button.gameObject.SetActive(false);
	}

	public void HideRecruit(string id)
	{
		_recruitPanels[id].gameObject.SetActive(false);
	}
}
