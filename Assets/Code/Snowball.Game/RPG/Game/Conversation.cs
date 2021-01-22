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
		[TextArea] public string Text;
		public UnitAuthoring Unit;
	}

	public class Conversation
	{
		private readonly ConversationUI _ui;

		public Conversation(ConversationUI ui)
		{
			_ui = ui;
		}

		public async UniTask Start(ConversationMessage[] messages)
		{
			var queue = new Queue<ConversationMessage>(messages);

			_ui.Show();

			await _ui.SetMessage(queue.Dequeue());

			while (true)
			{
				await UniTask.NextFrame();

				if (Keyboard.current.spaceKey.wasPressedThisFrame)
				{
					if (queue.Count == 0)
					{
						break;
					}

					await _ui.SetMessage(queue.Dequeue());
				}
			}

			_ui.Hide();
		}
	}
}
