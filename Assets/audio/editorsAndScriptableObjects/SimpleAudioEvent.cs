﻿using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName="Audio Events/Simple")]
public class SimpleAudioEvent : AudioEvent
{
	public AudioClip[] clips;

	public RangedFloat volume;

	[MinMaxRange(0, 2)]
	public RangedFloat pitch;

	public override void PlayOneShot(AudioSource source)
	{
		if (clips.Length == 0) return;

		source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
		source.PlayOneShot(clips[Random.Range(0, clips.Length)], Random.Range(volume.minValue, volume.maxValue));
	}
	
	public override void Play(AudioSource source){
		if (clips.Length == 0) return;

		source.clip = clips[Random.Range(0, clips.Length)];
		source.volume = Random.Range(volume.minValue, volume.maxValue);
		source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
		source.Play();
	}
}