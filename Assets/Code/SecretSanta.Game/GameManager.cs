using UnityEngine;
using static Helpers;

public class GameManager : MonoBehaviour
{
	[SerializeField] private Player _playerPrefab;

	private GameState _gameState;

	private readonly Vector2 _areaSize = new Vector2(32, 18);

	void Awake()
	{
		_gameState = new GameState();
		_gameState.PlayerInput = new PlayerInput();
	}

	void Start()
	{
		var player = GameObject.Instantiate(_playerPrefab);
		player.Transform.position = new Vector3(0, -6);
		_gameState.Player = player;
	}

	void Update()
	{
		_gameState.PlayerInput.Move = new Vector2(
			Input.GetAxis("Horizontal"),
			Input.GetAxis("Vertical")
		);

		{
			var position = _gameState.Player.Transform.position +
			               (Vector3) _gameState.PlayerInput.Move *
			               (Time.deltaTime * _gameState.Player.Speed);
			var size = _gameState.Player.Transform.localScale;
			position.x = Mathf.Clamp(position.x, -_areaSize.x / 2f + size.x / 2f, _areaSize.x / 2f - size.x / 2f);
			position.y = Mathf.Clamp(position.y, -_areaSize.y / 2f + size.y / 2f, _areaSize.y / 2f - size.y / 2f);
			_gameState.Player.Transform.position = position;
		}
	}
}

public class GameState
{
	public PlayerInput PlayerInput;
	public Player Player;
}

public class PlayerInput
{
	public Vector2 Move;
}
