using System.Collections.Generic;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
	[SerializeField] private TeamMemberUI _teamMemberPrefab;
	[SerializeField] private Transform _teamRoot;
	[SerializeField] private BarkUI _barkUI;

	public BarkUI Bark => _barkUI;

	public void HideAll()
	{
		HideTeam();
		_barkUI.Hide();
	}

	public void ShowTeam()
	{
		_teamRoot.gameObject.SetActive(true);
	}

	public void HideTeam()
	{
		_teamRoot.gameObject.SetActive(false);
	}

	public void UpdateTeam(List<Recruit> team)
	{
		foreach (Transform child in _teamRoot)
		{
			Destroy(child.gameObject);
		}

		foreach (var recruit in team)
		{
			var panel = Instantiate(_teamMemberPrefab, _teamRoot);
			panel.SetRecruit(recruit);
		}
	}
}
