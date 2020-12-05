using System.Threading.Tasks;
using UnityEngine;

public class VictoryState : IState
{
	public async Task Enter(object[] parameters)
	{
		Debug.Log("Game over");
	}

	public void Tick() { }

	public async Task Exit() { }
}

