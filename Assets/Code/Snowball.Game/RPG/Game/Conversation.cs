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

		public Conversation(ConversationUI ui)
		{
			_ui = ui;
		}

		public async UniTask Start(ConversationMessage[] messages, EncounterAuthoring encounter)
		{
			var queue = new Queue<ConversationMessage>(messages);

			_ui.Show();

			{
				var message = queue.Dequeue();
				await _ui.SetMessage(message, encounter.Foes.Contains(message.Unit) ? encounter.TeamColor : message.Unit.ColorCloth);
			}

			while (true)
			{
				await UniTask.NextFrame();

				if (Keyboard.current.spaceKey.wasPressedThisFrame)
				{
					if (queue.Count == 0)
					{
						break;
					}

					{
						var message = queue.Dequeue();
						await _ui.SetMessage(message, encounter.Foes.Contains(message.Unit) ? encounter.TeamColor : message.Unit.ColorCloth);
					}
				}
			}

			_ui.Hide();
		}
	}
}
