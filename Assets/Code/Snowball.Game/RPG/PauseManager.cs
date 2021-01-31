using UnityEngine;
using UnityEngine.InputSystem;

namespace Snowball.Game
{
	public class PauseManager
	{
		private readonly GameConfig _config;
		private readonly PauseUI _ui;
		private readonly GameControls _controls;

		public PauseManager(GameConfig config, GameControls controls, PauseUI ui)
		{
			_config = config;
			_ui = ui;
			_controls = controls;
		}

		public void Resume()
		{
			Time.timeScale = 1f;
			_ui.Hide();
			_config.AudioMixer.TransitionToSnapshots(new[] { _config.DefaultAudioSnapshot }, new []{ 1f }, 0f);

			_controls.PauseMenu.Disable();
			_controls.PauseMenu.Cancel.performed -= OnCancelPerformed;
			_controls.Global.Enable();
		}

		public void Pause()
		{
			Time.timeScale = 0f;
			_ui.Show();
			_config.AudioMixer.TransitionToSnapshots(new[] { _config.PauseAudioSnapshot }, new []{ 1f }, 0f);

			_controls.Global.Disable();
			_controls.PauseMenu.Enable();
			_controls.PauseMenu.Cancel.performed += OnCancelPerformed;
		}

		private void OnCancelPerformed(InputAction.CallbackContext obj)
		{
			Resume();
		}
	}
}
