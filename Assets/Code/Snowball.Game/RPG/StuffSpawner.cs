using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Snowball.Game
{
	public class StuffSpawner
	{
		public ParticleSystem SpawnEffect(ParticleSystem effectPrefab)
		{
			return GameObject.Instantiate(effectPrefab);
		}

		public ParticleSystem SpawnEffect(ParticleSystem effectPrefab, Vector3 position)
		{
			return GameObject.Instantiate(effectPrefab, position, Quaternion.identity);
		}

		public async UniTask SpawnText(FloatingText textPrefab, string text, Vector3 position)
		{
			var floatingText = GameObject.Instantiate(textPrefab, position, Quaternion.identity);

			await floatingText.Show(text);

			GameObject.Destroy(floatingText.gameObject);
		}
	}
}
