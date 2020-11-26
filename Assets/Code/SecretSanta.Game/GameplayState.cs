using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameplayState : IState
{
	private float _startTime;
	private List<Entity> _team;
	private List<Entity> _enemies;
	private List<Entity> _projectiles;
	private readonly Vector3 _recruitSpawnPosition = new Vector3(0, -6);

	public async Task Enter(object[] parameters)
	{
		_startTime = Time.time;
		_team = new List<Entity>();
		_enemies = new List<Entity>();
		_projectiles = new List<Entity>();

		var level = GameManager.Instance.Config.Levels[0];
		foreach (var spawn in level.Spawns)
		{
			GameManager.Instance.State.SpawnsQueue.Enqueue(spawn);
		}

		foreach (var recruit in GameManager.Instance.State.Team)
		{
			SpawnTeamMember(recruit, _recruitSpawnPosition);
		}

		Projectile.OnDestroyed += OnProjectileDestroyed;

		GameManager.Instance.GameUI.Gameplay.UpdateTeam(GameManager.Instance.State.Team);
		GameManager.Instance.GameUI.Gameplay.Show();
	}

	public void Tick()
	{
		if (_team.Count > 0)
		{
			for (var recruitIndex = 0; recruitIndex < _team.Count; recruitIndex++)
			{
				if (recruitIndex == 0)
				{
					_team[recruitIndex].Movement.MoveInDirection(GameManager.Instance.State.Inputs.Move, true);
				}
				else
				{
					var target = _team[recruitIndex - 1].Transform.position;
					_team[recruitIndex].Movement.Follow(target, 1.5f);
				}
			}

			if (GameManager.Instance.State.Inputs.Fire)
			{
				_team[0].Weapon.Fire();
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
	}

	public async Task Exit()
	{
		GameManager.Instance.GameUI.Gameplay.Hide();

		foreach (var entity in _projectiles)
		{
			GameObject.Destroy(entity.gameObject);
		}
		foreach (var entity in _team)
		{
			GameObject.Destroy(entity.gameObject);
		}
		foreach (var entity in _enemies)
		{
			GameObject.Destroy(entity.gameObject);
		}

		Projectile.OnDestroyed -= OnProjectileDestroyed;
	}

	private void SpawnTeamMember(Recruit data, Vector3 position)
	{
		var entity = GameObject.Instantiate(GameManager.Instance.Config.RecruitPrefab);
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

		if (GameManager.Instance.State.SpawnsQueue.Count == 0 && _enemies.Count == 0)
		{
			GameManager.Instance.Machine.Fire(GameStateMachine.Triggers.Win);
		}
	}

	private void OnWeaponFired(Entity entity)
	{
		_projectiles.Add(entity);
	}

	private void OnProjectileDestroyed(Entity entity)
	{
		_projectiles.Remove(entity);
	}
}
