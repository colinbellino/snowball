using System.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class BattleDefeatState : BaseBattleState, IState
	{
		public BattleDefeatState(BattleStateMachine machine) : base(machine) { }

		public async Task Enter(object[] args)
		{
			_ui.HideAll();

			Debug.Log("Battle lost :(");

#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			UnityEngine.Application.Quit();
#endif
		}

		public async Task Exit() { }

		public void Tick() { }
	}
}
