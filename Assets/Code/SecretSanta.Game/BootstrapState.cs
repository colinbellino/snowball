using System.Threading.Tasks;

public class BootstrapState : IState
{
	public async Task Enter(object[] parameters)
	{
		GameManager.Instance.Machine.Fire(GameStateMachine.Triggers.StartRecruitment);
	}

	public void Tick() { }

	public async Task Exit() { }
}
