using System.Threading.Tasks;
using UnityEngine;

public class GameplayState : IState
{
	public async Task Enter(object[] parameters) { }

	public void Tick()
	{
		if (GameManager.Instance.State.Team.Count > 0)
		{
			for (var recruitIndex = 0; recruitIndex < GameManager.Instance.State.Team.Count; recruitIndex++)
			{
				if (recruitIndex == 0)
				{
					GameManager.Instance.State.Team[0].Movement.MoveInDirection(GameManager.Instance.State.Inputs.Move, true);
				}
				else
				{
					var target = GameManager.Instance.State.Team[recruitIndex - 1].Transform.position;
					GameManager.Instance.State.Team[recruitIndex].Movement.Follow(target, 1.5f);
				}
			}

			if (GameManager.Instance.State.Inputs.Fire)
			{
				GameManager.Instance.State.Team[0].Weapon.Fire();
			}
		}

		{
			if (GameManager.Instance.State.SpawnsQueue.Count > 0)
			{
				var nextSpawn = GameManager.Instance.State.SpawnsQueue.Peek();
				if (Time.time >= nextSpawn.Time)
				{
					var enemy = GameObject.Instantiate(nextSpawn.Prefab);
					enemy.Transform.position = nextSpawn.Position;
					GameManager.Instance.State.SpawnsQueue.Dequeue();
				}
			}
		}
	}

	public async Task Exit() { }
}
