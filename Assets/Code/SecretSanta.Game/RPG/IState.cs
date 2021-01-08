using Cysharp.Threading.Tasks;

namespace Code.SecretSanta.Game.RPG
{
	public interface IState
	{
		UniTask Enter(object[] args);
		UniTask Exit();
		void Tick();
	}
}
