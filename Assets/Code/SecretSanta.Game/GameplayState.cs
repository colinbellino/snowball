using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;
using static Helpers;

public class GameplayState : IState
{
	private float _startTime;
	private List<Entity> _team;
	private List<Entity> _enemies;
	private List<Entity> _projectiles;
	private List<Entity> _toDestroy;
	private double _switchTimestamp;
	private bool _victoryAchieved;
	private const double _switchCooldown = 0.2f;
	private Bounds _killBox;
	private Grid _levelGrid;

	private readonly Vector3 _recruitSpawnPosition = new Vector3(0, -6);
	private const float _scrollSpeed = 1f;

	public async Task Enter(object[] parameters)
	{
		_startTime = Time.time;
		_team = new List<Entity>();
		_enemies = new List<Entity>();
		_projectiles = new List<Entity>();
		_toDestroy = new List<Entity>();
		_switchTimestamp = Time.time + _switchCooldown;
		_victoryAchieved = false;
		_killBox = new Bounds(
			new Vector3(0f, -(GameManager.Instance.Config.AreaSize.y / 2) - 1, 0f),
			new Vector3(GameManager.Instance.Config.AreaSize.x, 1f, 0f)
		);

		var level = GameManager.Instance.Config.Levels[GameManager.Instance.State.CurrentLevel];
		// TODO: Get this from the level data
		_levelGrid = GameObject.Find("Level").GetComponent<Grid>();
		foreach (var spawn in level.Spawns)
		{
			GameManager.Instance.State.SpawnsQueue.Enqueue(spawn);
		}

		for (var recruitIndex = 0; recruitIndex < GameManager.Instance.State.Team.Count; recruitIndex++)
		{
			var recruit = GameManager.Instance.State.Team[recruitIndex];
			SpawnTeamMember(recruit, _recruitSpawnPosition + new Vector3(1f * recruitIndex +1, 0));
		}

		UpdateTeam();

		Projectile.OnImpact += OnProjectileImpact;

		GameManager.Instance.GameUI.Gameplay.UpdateTeam(GameManager.Instance.State.Team);
		GameManager.Instance.GameUI.Gameplay.ShowTeam();
	}

	public void Tick()
	{
		if (_victoryAchieved == false)
		{
			_levelGrid.transform.position += Vector3.down * (Time.deltaTime * _scrollSpeed);

			if (_team.Count > 0)
			{
				if (IsPressed(GameManager.Instance.State.Controls.Gameplay.Fire))
				{
					// _team[0].Weapon.Fire();
					// Note: all team mates shooting might be too overpowered
					foreach (var entity in _team)
					{
						entity.Weapon.Fire();
					}
				}

				if (Time.time > _switchTimestamp && WasReleasedThisFrame(GameManager.Instance.State.Controls.Gameplay.Switch))
				{
					SwitchTeamMember();
					_switchTimestamp = Time.time + _switchCooldown;
				}

				for (var recruitIndex = 0; recruitIndex < _team.Count; recruitIndex++)
				{
					if (recruitIndex == 0)
					{
						var moveInput = GameManager.Instance.State.Controls.Gameplay.Move.ReadValue<Vector2>();
						_team[recruitIndex].Movement.MoveInDirection(moveInput, true);
					}
					else
					{
						var target = _team[recruitIndex - 1].Transform.position;
						_team[recruitIndex].Movement.Follow(target, _team[0].Movement.Speed, 1.5f);
					}
				}
			}

			if (GameManager.Instance.State.SpawnsQueue.Count > 0)
			{
				var nextSpawn = GameManager.Instance.State.SpawnsQueue.Peek();
				if (Time.time >= _startTime + nextSpawn.Time)
				{
					SpawnEnemy(nextSpawn.Data, nextSpawn.Position);
					GameManager.Instance.State.SpawnsQueue.Dequeue();
				}
			}

			for (var i = _enemies.Count - 1; i >= 0; i--)
			{
				var entity = _enemies[i];

				entity.Brain?.Tick();

				if (_killBox.Contains(entity.Transform.position))
				{
					_enemies.RemoveAt(i);
					_toDestroy.Add(entity);
				}
			}

			if (GameManager.Instance.State.SpawnsQueue.Count == 0 && _enemies.Count == 0)
			{
				_victoryAchieved = true;
				Victory();
			}
		}

		foreach (var entity in _toDestroy)
		{
			GameObject.Destroy(entity.gameObject);
		}
		_toDestroy.Clear();
	}

	public async Task Exit()
	{
		GameManager.Instance.GameUI.Gameplay.HideAll();

		_toDestroy.AddRange(_team);
		_team.Clear();

		foreach (var entity in _toDestroy)
		{
			GameObject.Destroy(entity.gameObject);
		}
		_toDestroy.Clear();

		Projectile.OnImpact -= OnProjectileImpact;
	}

	private void SpawnTeamMember(Recruit data, Vector3 position)
	{
		var entity = GameObject.Instantiate(GameManager.Instance.Config.RecruitPrefab);
		entity.name = $"Recruit: {data.Name}";
		entity.Movement.Speed = data.MoveSpeed;
		entity.Transform.position = position;
		entity.Target.OnHit += OnTeamMemberHit;
		entity.Weapon.OnFired += OnWeaponFired;
		entity.Renderer.Color = data.Color;
		_team.Add(entity);
	}

	private void SpawnEnemy(Enemy data, Vector2 position)
	{
		var entity = GameObject.Instantiate(GameManager.Instance.Config.EnemyPrefab);
		entity.Movement.Speed = data.MoveSpeed;
		entity.Transform.position = position;
		entity.Target.OnHit += OnEnemyHit;
		entity.Weapon.OnFired += OnWeaponFired;
		entity.Renderer.Color = data.Color;
		entity.Brain.FireDelay = data.FireDelay;
		_enemies.Add(entity);
	}

	private void SwitchTeamMember()
	{
		var positions = _team.Select(entity => entity.Transform.position).ToArray();

		for (var recruitIndex = 0; recruitIndex < _team.Count; recruitIndex++)
		{
			var previous = _team[(recruitIndex + _team.Count - 1) % _team.Count];
			// var next = _team[(recruitIndex + 1) % _team.Count];
			_team[recruitIndex].Movement.TeleportTo(positions[(recruitIndex + _team.Count - 1) % _team.Count]);
		}
		{
			var oldLeader = _team[0];

			_team.Remove(oldLeader);
			_team.Add(oldLeader);
		}
		{
			var currentLeader = GameManager.Instance.State.Team[0];
			GameManager.Instance.State.Team.Remove(currentLeader);
			GameManager.Instance.State.Team.Add(currentLeader);
		}
		UpdateTeam();
	}

	private void OnTeamMemberHit(Entity entity)
	{
		if (_victoryAchieved)
		{
			return;
		}

		var leader = GameManager.Instance.State.Team[0];
		var bark = GameManager.Instance.BarkManager.GetRandomBark(BarkType.Died, leader);
		if (bark != null)
		{
			GameManager.Instance.GameUI.Gameplay.Bark.Show(leader, bark, 2000f);
		}

		SwitchTeamMember();
		_toDestroy.Add(_team[_team.Count - 1]);
		_team.Remove(_team[_team.Count - 1]);
		GameManager.Instance.State.Team.RemoveAt(GameManager.Instance.State.Team.Count - 1);

		if (GameManager.Instance.State.Team.Count == 0)
		{
			Defeat();
		}
	}

	private void OnEnemyHit(Entity entity)
	{
		_enemies.Remove(entity);
		_toDestroy.Add(entity);
	}

	private void OnWeaponFired(Entity entity)
	{
		_projectiles.Add(entity);
	}

	private void UpdateTeam()
	{
		for (var recruitIndex = 0; recruitIndex < _team.Count; recruitIndex++)
		{
			var entity = _team[recruitIndex];

			entity.Renderer.SetSize(recruitIndex == 0 ? Renderer.Size.Normal : Renderer.Size.Small);
			entity.Target.Activate(recruitIndex == 0);
		}

		GameManager.Instance.GameUI.Gameplay.UpdateTeam(GameManager.Instance.State.Team);
	}

	private void OnProjectileImpact(Entity entity)
	{
		_projectiles.Remove(entity);
		_toDestroy.Add(entity);
	}

	private async void Victory()
	{
		_toDestroy.AddRange(_projectiles);
		_projectiles.Clear();

		_toDestroy.AddRange(_enemies);
		_enemies.Clear();

		var randomRecruit = GameManager.Instance.State.Team[Random.Range(0, GameManager.Instance.State.Team.Count)];
		var bark = GameManager.Instance.BarkManager.GetRandomBark(BarkType.LevelFinished, randomRecruit);
		if (bark != null)
		{
			GameManager.Instance.GameUI.Gameplay.Bark.Show(randomRecruit, bark, 2000f);
		}

		await UniTask.Delay(1000);

		foreach (var entity in _team)
		{
			var destination = entity.Transform.position + new Vector3(0, GameManager.Instance.Config.AreaSize.y);
			entity.Movement.MoveTo(destination, _team[0].Movement.Speed);
		}

		await UniTask.Delay(2000);

		GameManager.Instance.State.CurrentLevel += 1;

		if (GameManager.Instance.State.CurrentLevel > GameManager.Instance.Config.Levels.Length - 1)
		{
			GameManager.Instance.Machine.Fire(GameStateMachine.Triggers.Win);
		}

		GameManager.Instance.Machine.Fire(GameStateMachine.Triggers.FinishLevel);
	}

	private void Defeat()
	{
		_toDestroy.AddRange(_projectiles);
		_projectiles.Clear();

		_toDestroy.AddRange(_enemies);
		_enemies.Clear();

		GameManager.Instance.Machine.Fire(GameStateMachine.Triggers.Lose);
	}
}
