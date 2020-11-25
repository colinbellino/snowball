using System;
using UnityEngine;
using UnityEngine.UI;

public class RecruitmentUI : MonoBehaviour
{
	[SerializeField] private Transform _root;
	private Button[] _recruitButtons;

	public event Action RecruitSelected;

	private void Awake()
	{
		_recruitButtons = GetComponentsInChildren<Button>();
		for (var buttonIndex = 0; buttonIndex < _recruitButtons.Length; buttonIndex++)
		{
			var button = _recruitButtons[buttonIndex];
			button.onClick.AddListener(() => RecruitSelected?.Invoke());
		}
	}

	public void Show()
	{
		_root.gameObject.SetActive(true);
	}

	public void Hide()
	{
		_root.gameObject.SetActive(false);
	}
}
