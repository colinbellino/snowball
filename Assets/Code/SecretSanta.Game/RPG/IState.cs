using Cysharp.Threading.Tasks;

namespace Code.SecretSanta.Game.RPG
{
	public interface IState
	{
		UniTask Enter();
		UniTask Exit();
		void Tick();
	}
}
