using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class BootstrapState : IState
{
	public async Task Enter(object[] parameters)
	{
		// var seed = DateTime.Now.Millisecond;
		// UnityEngine.Random.InitState(seed);
		// Debug.Log("Seed: " + seed);

		GameManager.Instance.State.CurrentLevel = 0;
		GameManager.Instance.State.Team = new List<Recruit>();
		GameManager.Instance.State.RecruitsQueue = new Queue<Recruit>();
		GameManager.Instance.State.SpawnsQueue = new Queue<Spawn>();
		GameManager.Instance.State.Controls = new GameControls();
		GameManager.Instance.State.Controls.Enable();

		var randomizedRecruits = GameManager.Instance.Config.Recruits
			.OrderBy(a => Guid.NewGuid())
			.ToList();
		foreach (var recruit in randomizedRecruits)
		{
			GameManager.Instance.State.RecruitsQueue.Enqueue(GameObject.Instantiate(recruit));
		}

		GameManager.Instance.GameUI.Recruitment.HideRecruits();
		GameManager.Instance.GameUI.Recruitment.HideName();
		GameManager.Instance.GameUI.Gameplay.HideAll();

		GameManager.Instance.Machine.Fire(GameStateMachine.Triggers.StartRecruitment);
	}

	public void Tick() { }

	public async Task Exit() { }
}
