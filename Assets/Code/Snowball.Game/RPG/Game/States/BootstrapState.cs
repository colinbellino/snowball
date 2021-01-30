using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Snowball.Game
{
	public class BootstrapState : IState
	{
		private readonly GameStateMachine _machine;

		public BootstrapState(GameStateMachine machine)
		{
			_machine = machine;
		}

		public UniTask Enter()
		{
			DatabaseHelpers.LoadFromResources(Game.Instance.Database);
			DatabaseHelpers.PrepareAbilities(Game.Instance.Database, Game.Instance.Config, Game.Instance.Spawner);

			Game.Instance.AudioPlayer.SetMusicVolume(Game.Instance.Config.MusicVolume);
			Game.Instance.AudioPlayer.SetSoundVolume(Game.Instance.Config.SoundVolume);

			Time.timeScale = 1f;

			_machine.Fire(GameStateMachine.Triggers.Done);

			return default;
		}

		public UniTask Exit() { return default; }

		public void Tick() { }
	}
}
