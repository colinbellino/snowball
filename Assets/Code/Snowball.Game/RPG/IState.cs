using Cysharp.Threading.Tasks;

namespace Snowball.Game
{
	public interface IState
	{
		UniTask Enter();
		UniTask Exit();
		void Tick();
	}
}
