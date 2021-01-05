using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class StuffSpawner
	{
		public void SpawnEffect(ParticleSystem effectPrefab, Vector3 position)
		{
			GameObject.Instantiate(effectPrefab, position, Quaternion.identity);
		}

		public UniTask SpawnText(FloatingText textPrefab, string text, Vector3 position)
		{
			var floatingText = GameObject.Instantiate(textPrefab, position, Quaternion.identity);
			floatingText.Show(text);

			return UniTask.Delay(TimeSpan.FromSeconds(floatingText.Duration));
		}
	}
}
