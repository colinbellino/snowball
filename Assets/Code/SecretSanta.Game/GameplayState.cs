using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using static Helpers;
using Random = UnityEngine.Random;

public class GameplayState : IState
{
	private float _startTime;
	private List<Entity> _team;
	private List<Entity> _enemies;
	private List<Entity> _projectiles;
	private double _switchTimestamp;
	private bool _victoryAchieved;
	private const double _switchCooldown = 0.2f;
	private readonly Vector3 _recruitSpawnPosition = new Vector3(0, -6);

	public async Task Enter(object[] parameters)
	{
		_startTime = Time.time;
		_team = new List<Entity>();
		_enemies = new List<Entity>();
		_projectiles = new List<Entity>();
		_switchTimestamp = Time.time + _switchCooldown;
		_victoryAchieved = false;

		var level = GameManager.Instance.Config.Levels[0];
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

		Projectile.OnDestroyed += OnProjectileDestroyed;

		GameManager.Instance.GameUI.Gameplay.UpdateTeam(GameManager.Instance.State.Team);
		GameManager.Instance.GameUI.Gameplay.ShowTeam();
	}

	public void Tick()
	{
		if (_victoryAchieved)
		{
			return;
		}

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

		if (GameManager.Instance.State.SpawnsQueue.Count == 0 && _enemies.Count == 0)
		{
			_victoryAchieved = true;
			Victory();
		}
	}

	public async Task Exit()
	{
		GameManager.Instance.GameUI.Gameplay.HideAll();

		foreach (var entity in _team)
		{
			GameObject.Destroy(entity.gameObject);
		}
		_team.Clear();

		Projectile.OnDestroyed -= OnProjectileDestroyed;
	}

	private void SpawnTeamMember(Recruit data, Vector3 position)
	{
		var entity = GameObject.Instantiate(GameManager.Instance.Config.RecruitPrefab);
		entity.name = $"Recruit: {data.Name}";
		entity.Movement.Speed = data.MoveSpeed;
		entity.Transform.position = position;
		entity.Target.OnKilled += OnTeamMemberKilled;
		entity.Weapon.OnFired += OnWeaponFired;
		entity.Renderer.Color = data.Color;
		_team.Add(entity);
	}

	private void SpawnEnemy(Enemy data, Vector2 position)
	{
		var entity = GameObject.Instantiate(GameManager.Instance.Config.EnemyPrefab);
		entity.Movement.Speed = data.MoveSpeed;
		entity.Transform.position = position;
		entity.Target.OnKilled += OnEnemyKilled;
		entity.Weapon.OnFired += OnWeaponFired;
		entity.Renderer.Color = data.Color;
		entity.Brain.FireDelay = data.FireDelay;
		_enemies.Add(entity);
	}

	private void OnTeamMemberKilled(Entity entity)
	{
		GameManager.Instance.Machine.Fire(GameStateMachine.Triggers.Lose);
	}

	private void OnEnemyKilled(Entity entity)
	{
		_enemies.Remove(entity);
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

	private void OnProjectileDestroyed(Entity entity)
	{
		_projectiles.Remove(entity);
	}

	private async void Victory()
	{
		foreach (var entity in _projectiles)
		{
			GameObject.Destroy(entity.gameObject);
		}
		_projectiles.Clear();
		foreach (var entity in _enemies)
		{
			GameObject.Destroy(entity.gameObject);
		}
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

		GameManager.Instance.Machine.Fire(GameStateMachine.Triggers.Win);
	}
}
