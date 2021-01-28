using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Snowball.Game
{
	[Serializable]
	public class ConversationMessage
	{
		[TextArea(5, 5)] public string Text;
		public UnitAuthoring Unit;
		public TextAnchor Alignment;
	}

	public class Conversation
	{
		private readonly ConversationUI _ui;
		private readonly GameConfig _config;
		private readonly AudioPlayer _audio;
		private readonly GameControls _controls;
		private bool _confirmWasPerformed;

		public Conversation(ConversationUI ui, GameConfig config, AudioPlayer audioPlayer, GameControls controls)
		{
			_ui = ui;
			_config = config;
			_audio = audioPlayer;
			_controls = controls;
		}

		public async UniTask Start(ConversationMessage[] messages, EncounterAuthoring encounter)
		{
			var queue = new Queue<ConversationMessage>(messages);

			_controls.Gameplay.Confirm.performed += OnConfirmPerformed;

			_ui.Show();

			{
				var message = queue.Dequeue();
				await _ui.SetMessage(message, encounter.Foes.Contains(message.Unit) ? encounter.TeamColor : message.Unit.ColorCloth);
			}

			while (true)
			{
				_confirmWasPerformed = false;

				await UniTask.NextFrame();

				if (_confirmWasPerformed)
				{
					if (queue.Count == 0)
					{
						break;
					}

					_audio.PlaySoundEffect(_config.MenuConfirmClip);

					{
						var message = queue.Dequeue();
						await _ui.SetMessage(message, encounter.Foes.Contains(message.Unit) ? encounter.TeamColor : message.Unit.ColorCloth);
					}
				}
			}

			_controls.Gameplay.Confirm.performed -= OnConfirmPerformed;

			_ui.Hide();
		}

		private void OnConfirmPerformed(InputAction.CallbackContext context)
		{
			_confirmWasPerformed = true;
		}
	}
}
