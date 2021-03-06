using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour {
	public Sound[] Sounds;

	private void Awake() {
		foreach (Sound sound in Sounds) {
			sound.Source = gameObject.AddComponent<AudioSource>();
			sound.Source.clip = sound.Clip;

			sound.Source.volume = sound.Volume;
			sound.Source.pitch = sound.Pitch;
		}
	}

	public void StopAllSounds() {
		foreach (Sound sound in Sounds) {
			Stop(sound.Name);
		}
	}

	public void Play(string name) {
		Sound soundToPlay = GetSound(name);
		if (soundToPlay.Source.isPlaying || soundToPlay == null)
			return;
		soundToPlay.Source.Play();
	}
	public void Stop(string name) {
		Sound soundToPlay = GetSound(name);
		if (soundToPlay == null)
			return;
		soundToPlay.Source.Stop();
	}
	private Sound GetSound(string name) {
		return Array.Find(Sounds, sound => sound.Name == name);
	}
}

[Serializable]
public class Sound {
	public AudioClip Clip;

	public string Name;
	[Range(0f, 1f)]
	public float Volume;
	[Range(.1f, 3f)]
	public float Pitch;

	[HideInInspector]
	public AudioSource Source;
}
