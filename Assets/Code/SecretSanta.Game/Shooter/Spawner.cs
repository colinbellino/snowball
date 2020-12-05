using UnityEngine;

public class Spawner : MonoBehaviour
{
	[SerializeField] private Enemy _data;

	public Enemy Data => _data;

	private void OnValidate()
	{
		name = _data.Id;
	}
}
