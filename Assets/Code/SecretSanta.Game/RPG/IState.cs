using System.Threading.Tasks;

namespace Code.SecretSanta.Game.RPG
{
	public interface IState
	{
		Task Enter(object[] args);
		Task Exit();
		void Tick();
	}
}
