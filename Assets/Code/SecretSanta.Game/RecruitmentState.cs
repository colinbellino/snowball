using System.Threading.Tasks;
using UnityEngine;

public class RecruitmentState : IState
{
	public async Task Enter(object[] parameters)
	{
		GameManager.Instance.GameUI.Recruitment.Show();
		GameManager.Instance.GameUI.Recruitment.RecruitSelected += OnRecruitSelected;
	}

	public void Tick() { }

	public async Task Exit()
	{
		GameManager.Instance.GameUI.Recruitment.RecruitSelected -= OnRecruitSelected;
	}

	private void OnRecruitSelected()
	{
		GameManager.Instance.GameUI.Recruitment.Hide();

		AddRecruit(GameManager.Instance.State.RecruitsQueue.Dequeue());
		// AddRecruit(_recruitsQueue.Dequeue());
		// AddRecruit(_recruitsQueue.Dequeue());

		GameManager.Instance.State.Team[0].Transform.position = new Vector3(0, -6);

		GameManager.Instance.Machine.Fire(GameStateMachine.Triggers.StartGameplay);
	}

	private void AddRecruit(Entity prefab)
	{
		var recruit = GameObject.Instantiate(prefab);
		GameManager.Instance.State.Team.Add(recruit);
	}
}
