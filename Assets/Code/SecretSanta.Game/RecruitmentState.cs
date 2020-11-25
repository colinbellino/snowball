using System.Threading.Tasks;
using UnityEngine;

public class RecruitmentState : IState
{
	private Recruit[] _recruits;

	public async Task Enter(object[] parameters)
	{
		_recruits = new Recruit[]
		{
			GameManager.Instance.State.RecruitsQueue.Dequeue(),
			GameManager.Instance.State.RecruitsQueue.Dequeue(),
			GameManager.Instance.State.RecruitsQueue.Dequeue(),
		};
		GameManager.Instance.GameUI.Recruitment.Show(_recruits);
		GameManager.Instance.GameUI.Recruitment.RecruitSelected += OnRecruitSelected;
	}

	public void Tick() { }

	public async Task Exit()
	{
		GameManager.Instance.GameUI.Recruitment.RecruitSelected -= OnRecruitSelected;
	}

	private void OnRecruitSelected(string id)
	{
		GameManager.Instance.GameUI.Recruitment.Hide();

		foreach (var recruit in _recruits)
		{
			if (recruit.Id == id)
			{
				Debug.Log($"Recruited: {recruit.Id}");
				GameManager.Instance.State.Team.Add(recruit);
			}
			else
			{
				GameManager.Instance.State.RecruitsQueue.Enqueue(recruit);
			}
		}

		GameManager.Instance.Machine.Fire(GameStateMachine.Triggers.StartGameplay);
	}
}
