using UnityEngine;

namespace Snowball.Game
{
	public class Pause
	{
		private readonly GameConfig _config;
		private readonly PauseUI _ui;
		private readonly AudioPlayer _audio;

		private bool _paused;

		public Pause(GameConfig config, AudioPlayer audio, PauseUI ui)
		{
			_config = config;
			_ui = ui;
			_audio = audio;
		}

		public void Toggle()
		{
			_paused = !_paused;

			if (_paused)
			{
				Time.timeScale = 0f;
				_ui.Show();
				// _audio.SetMusicVolume(_config.MusicVolume * 0.7f);
				_config.AudioMixer.TransitionToSnapshots(new[] { _config.PauseAudioSnapshot }, new []{ 1f }, 0f);
			}
			else
			{
				Time.timeScale = 1f;
				_ui.Hide();
				// _audio.SetMusicVolume(_config.MusicVolume);
				_config.AudioMixer.TransitionToSnapshots(new[] { _config.DefaultAudioSnapshot }, new []{ 1f }, 0f);
			}
		}
	}
}
