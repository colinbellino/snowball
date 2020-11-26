using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameplayState : IState
{
	private float _startTime;
	private List<Entity> _team;
	private List<Entity> _enemies;
	private readonly Vector3 _recruitSpawnPosition = new Vector3(0, -6);

	public async Task Enter(object[] parameters)
	{
		_startTime = Time.time;
		_team = new List<Entity>();
		_enemies = new List<Entity>();

		var level = GameManager.Instance.Config.Levels[0];
		foreach (var spawn in level.Spawns)
		{
			GameManager.Instance.State.SpawnsQueue.Enqueue(spawn);
		}

		foreach (var recruit in GameManager.Instance.State.Team)
		{
			var entity = GameObject.Instantiate(GameManager.Instance.Config.RecruitPrefab);
			entity.Movement.Speed = recruit.MoveSpeed;
			entity.Transform.position = _recruitSpawnPosition;
			entity.Renderer.Color = recruit.Color;
			entity.Target.OnDestroyed += OnTeamMemberDestroy;
			_team.Add(entity);
		}

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
				GameManager.Instance.State.SpawnsQueue.Dequeue();

				var entity = GameObject.Instantiate(GameManager.Instance.Config.EnemyPrefab);
				entity.Movement.Speed = nextSpawn.Data.MoveSpeed;
				entity.Brain.FireDelay = nextSpawn.Data.FireDelay;
				entity.Transform.position = nextSpawn.Position;
				entity.Target.OnDestroyed += OnEnemyDestroyed;
				_enemies.Add(entity);
			}
		}
	}

	public async Task Exit()
	{
		GameManager.Instance.GameUI.Gameplay.Hide();

		foreach (var entity in _team)
		{
			GameObject.Destroy(entity.gameObject);
		}
		foreach (var entity in _enemies)
		{
			GameObject.Destroy(entity.gameObject);
		}
	}

	private void OnTeamMemberDestroy(Entity entity)
	{
		GameManager.Instance.Machine.Fire(GameStateMachine.Triggers.Lose);
	}

	private void OnEnemyDestroyed(Entity entity)
	{
		_enemies.Remove(entity);

		if (GameManager.Instance.State.SpawnsQueue.Count == 0 && _enemies.Count == 0)
		{
			GameManager.Instance.Machine.Fire(GameStateMachine.Triggers.Win);
		}
	}
}
