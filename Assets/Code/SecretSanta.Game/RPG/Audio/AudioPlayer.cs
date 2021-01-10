using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.SecretSanta.Game.RPG
{
	public class AudioPlayer
	{
		private readonly GameConfig _config;
		private readonly AudioSource _source;

		private readonly Dictionary<string, float> _musicTimes = new Dictionary<string, float>();

		public AudioPlayer(GameConfig config, AudioSource source)
		{
			_config = config;
			_source = source;

			SetMusicVolume(_config.MusicVolume);
			SetSoundVolume(_config.SoundVolume);
		}

		public void Tick()
		{
			if (_source.clip)
			{
				_musicTimes[_source.clip.name] = _source.time;
			}
		}

		public UniTask PlaySoundEffect(AudioClip clip)
		{
			// Default to the center of the screen
			return PlaySoundEffect(clip, (Vector2) _config.TilemapSize / 2);
		}

		public UniTask PlaySoundEffect(AudioClip clip, Vector3 position)
		{
			return PlaySoundClipAtPoint(clip, position);
		}

		public async UniTask PlayMusic(AudioClip clip, bool fromStart = true, float fadeDuration = 0f)
		{
			if (fromStart)
			{
				_source.time = 0f;
			}
			else
			{
				_musicTimes.TryGetValue(clip.name, out var time);
				_source.time = time;
			}

			_source.clip = clip;
			_source.Play();

			if (fadeDuration > 0f)
			{
				await _source.DOFade(1f, fadeDuration);
			}
			else
			{
				_source.volume = 1;
			}
		}

		public async UniTask StopMusic(float fadeDuration = 0.5f)
		{
			await _source.DOFade(0f, fadeDuration);
		}

		private void OnPaused() => SetMusicVolume(-25);

		private void OnResumed() => SetMusicVolume(_config.MusicVolume);

		private void SetMusicVolume(float volume)
		{
			_config.AudioMixer.SetFloat("MusicVolume", ConvertToMixerVolume(volume));
		}

		/**
		Convert to the volume range the unity audio mixer is expecting.
		<br />0   => -80
		<br />0.5 => -40
		<br />1   => 0
		 */
		private float ConvertToMixerVolume(float volume)
		{
			return (volume - 1f) * 80f;
		}

		private void SetSoundVolume(float volume)
		{
			_config.AudioMixer.SetFloat("SoundVolume", ConvertToMixerVolume(volume));
		}

		// TODO: Use polling instead of creating game object each time
		private UniTask PlaySoundClipAtPoint(AudioClip clip, Vector3 position)
		{
			var gameObject = new GameObject("One shot audio");
			gameObject.transform.position = position;
			var audioSource = (AudioSource) gameObject.AddComponent(typeof(AudioSource));
			audioSource.clip = clip;
			audioSource.outputAudioMixerGroup = _config.SoundsAudioMixerGroup;
			audioSource.Play();
			UnityEngine.Object.Destroy(gameObject, clip.length * ((double) Time.timeScale < 0.00999999977648258 ? 0.01f : Time.timeScale));
			return UniTask.Delay(TimeSpan.FromSeconds(clip.length));
		}
	}
}
