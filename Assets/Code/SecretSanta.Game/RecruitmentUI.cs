using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RecruitmentUI : MonoBehaviour
{
	[SerializeField] private RecruitPanelUI recruitPanelUIPrefab;
	[SerializeField] private Transform _root;

	private List<RecruitPanelUI> _recruitPanels;
	private EventSystem _eventSystem;

	public event Action<string> RecruitSelected;

	private void Awake()
	{
		_recruitPanels = new List<RecruitPanelUI>();
		_eventSystem = FindObjectOfType<EventSystem>();
		foreach (Transform child in _root)
		{
			Destroy(child.gameObject);
		}
	}

	public void Show(Recruit[] recruits)
	{
		foreach (var recruit in recruits)
		{
			var panel = Instantiate(recruitPanelUIPrefab, _root);
			panel.SetRecruit(recruit);
			panel.Button.onClick.AddListener(() => RecruitSelected?.Invoke(recruit.Id));
			_recruitPanels.Add(panel);
		}
		_root.gameObject.SetActive(true);

		_eventSystem.SetSelectedGameObject(_recruitPanels[1].Button.gameObject, null);
	}

	public void Hide()
	{
		_root.gameObject.SetActive(false);
	}
}
