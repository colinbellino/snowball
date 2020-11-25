using System.Threading.Tasks;

public class BootstrapState : IState
{
	public async Task Enter(object[] parameters)
	{
		GameManager.Instance.GameUI.Recruitment.HideRecruits();
		GameManager.Instance.GameUI.Recruitment.HideName();
		GameManager.Instance.GameUI.Gameplay.Hide();

		GameManager.Instance.Machine.Fire(GameStateMachine.Triggers.StartRecruitment);
	}

	public void Tick() { }

	public async Task Exit() { }
}
