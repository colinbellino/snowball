using System;
using System.Threading.Tasks;
using UnityEngine;
using static Helpers;

public class RecruitmentState : IState
{
	private Recruit[] _recruits;
	private Recruit _newRecruit;

	public async Task Enter(object[] parameters)
	{
		var count = Math.Min(GameManager.Instance.State.RecruitsQueue.Count, 3);
		_recruits = new Recruit[count];
		for (var recruitIndex = 0; recruitIndex < count; recruitIndex++)
		{
			_recruits[recruitIndex] = GameManager.Instance.State.RecruitsQueue.Dequeue();
		}

		GameManager.Instance.GameUI.Recruitment.ShowRecruits(_recruits);
		GameManager.Instance.GameUI.Recruitment.RecruitSelected += OnRecruitSelected;
		GameManager.Instance.GameUI.Recruitment.RecruitNamed += OnRecruitNamed;
	}

	public void Tick() { }

	public async Task Exit()
	{
		GameManager.Instance.GameUI.Recruitment.RecruitSelected -= OnRecruitSelected;
		GameManager.Instance.GameUI.Recruitment.RecruitNamed -= OnRecruitNamed;
	}

	private void OnRecruitSelected(string id)
	{
		foreach (var recruit in _recruits)
		{
			if (recruit.Id == id)
			{
				_newRecruit = recruit;
				GameManager.Instance.GameUI.Recruitment.FocusRecruit(recruit.Id);
			}
			else
			{
				GameManager.Instance.State.RecruitsQueue.Enqueue(recruit);
				GameManager.Instance.GameUI.Recruitment.HideRecruit(recruit.Id);
			}
		}

		var randomName = RandomItem(GameManager.Instance.Config.PlaceholderNames);
		GameManager.Instance.GameUI.Recruitment.ShowName(randomName);
	}

	private void OnRecruitNamed(string name)
	{
		_newRecruit.Name = name;
		Debug.Log($"Recruited: {_newRecruit.Id} -> {_newRecruit.Name}");

		if (GameManager.Instance.Config.CheatsFullTeam)
		{
			_recruits[0].Name = "Recruit #0";
			GameManager.Instance.State.Team.Add(_recruits[0]);
			_recruits[1].Name = "Recruit #1";
			GameManager.Instance.State.Team.Add(_recruits[1]);
			_recruits[2].Name = "Recruit #2";
			GameManager.Instance.State.Team.Add(_recruits[2]);
		}
		else
		{
			GameManager.Instance.State.Team.Add(_newRecruit);
		}

		GameManager.Instance.GameUI.Recruitment.HideRecruits();
		GameManager.Instance.GameUI.Recruitment.HideName();

		GameManager.Instance.Machine.Fire(GameStateMachine.Triggers.StartGameplay);
	}
}
