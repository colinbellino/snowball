using UnityEngine;

namespace Snowball.Game
{
	public class Pause
	{
		private readonly PauseUI _ui;

		private bool _paused;

		public Pause(PauseUI ui)
		{
			_ui = ui;
		}

		~Pause()
		{
			Time.timeScale = 1f;
		}

		public void Toggle()
		{
			if (_paused)
			{
				Time.timeScale = 1f;
				_ui.Hide();
			}
			else
			{
				Time.timeScale = 0f;
				_ui.Show();
			}

			_paused = !_paused;
		}
	}
}
