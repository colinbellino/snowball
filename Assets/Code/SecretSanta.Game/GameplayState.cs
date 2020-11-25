using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameplayState : IState
{
	private List<Entity> _team;
	private readonly Vector3 _recruitSpawnPosition = new Vector3(0, -6);

	public async Task Enter(object[] parameters)
	{
		_team = new List<Entity>();

		foreach (var recruit in GameManager.Instance.State.Team)
		{
			var entity = GameObject.Instantiate(GameManager.Instance.RecruitPrefab);
			entity.Movement.Speed = recruit.MoveSpeed;
			entity.Transform.position = _recruitSpawnPosition;
			entity.Renderer.Color = recruit.Color;
			_team.Add(entity);
		}
	}

	public void Tick()
	{
		if (_team.Count > 0)
		{
			for (var recruitIndex = 0; recruitIndex < _team.Count; recruitIndex++)
			{
				if (recruitIndex == 0)
				{
					_team[0].Movement.MoveInDirection(GameManager.Instance.State.Inputs.Move, true);
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
			if (Time.time >= nextSpawn.Time)
			{
				var entity = GameObject.Instantiate(GameManager.Instance.EnemyPrefab);
				entity.Movement.Speed = nextSpawn.Data.MoveSpeed;
				entity.Transform.position = nextSpawn.Position;
				GameManager.Instance.State.SpawnsQueue.Dequeue();
			}
		}
	}

	public async Task Exit() { }
}
