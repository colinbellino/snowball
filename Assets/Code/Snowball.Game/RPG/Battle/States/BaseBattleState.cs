using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Snowball.Game
{
	public abstract class BaseBattleState : IState
	{
		private readonly Game _game;
		protected readonly BattleStateMachine _machine;
		protected readonly TurnManager _turnManager;

		protected Vector3Int _cursorPosition;
		protected GameConfig _config => _game.Config;
		protected GameControls _controls => _game.Controls;
		protected BattleUI _ui => _game.BattleUI;
		protected GameState _state => _game.State;
		protected Database _database => _game.Database;
		protected StuffSpawner _spawner => _game.Spawner;
		protected Board _board => _game.Board;
		protected AudioPlayer _audio => _game.AudioPlayer;
		protected ComputerPlayerUnit _cpu => _game.CPU;
		protected Turn _turn => _turnManager.Turn;

		protected BaseBattleState(BattleStateMachine machine, TurnManager turnManager)
		{
			_machine = machine;
			_turnManager = turnManager;
			_game = Game.Instance;
		}

		public virtual UniTask Enter()
		{
			if (_turn != null && _turn.Unit.Driver == Unit.Drivers.Human)
			{
				_controls.Gameplay.Confirm.performed += OnConfirmPerformed;
				_controls.Gameplay.Cancel.performed += OnCancelPerformed;
			}

			return default;
		}

		public virtual UniTask Exit()
		{
			if (_turn != null && _turn.Unit.Driver == Unit.Drivers.Human)
			{
				_controls.Gameplay.Confirm.performed -= OnConfirmPerformed;
				_controls.Gameplay.Cancel.performed -= OnCancelPerformed;
			}

			return default;
		}

		public virtual void Tick()
		{
			if (_turn != null && _turn.Unit.Driver == Unit.Drivers.Human)
			{
				var mousePosition = _controls.Gameplay.MousePosition.ReadValue<Vector2>();
				var mouseWorldPosition = _game.Camera.ScreenToWorldPoint(mousePosition);
				var cursorPosition = GridHelpers.GetCursorPosition(mouseWorldPosition, _config.TilemapSize);

				if (cursorPosition != _cursorPosition)
				{
					_cursorPosition = cursorPosition;
					OnCursorMove();
				}
			}
		}

		private void OnConfirmPerformed(InputAction.CallbackContext context)
		{
			OnConfirm();
		}

		private void OnCancelPerformed(InputAction.CallbackContext context)
		{
			OnCancel();
		}

		protected virtual void OnConfirm() { }

		protected virtual void OnCancel() { }

		protected virtual void OnCursorMove() { }
	}
}
