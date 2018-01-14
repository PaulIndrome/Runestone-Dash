using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour {


	private AudioSource musicAudioSource;

	public AudioEvent musicTracksCollection;
	bool fadingOut, fadingIn;
	float currentTrackLength, currentTrackTime;
	float currentTrackStartTime;
	public float fadeOutTime, fadeInTime;

	[Range(0,1)]
	public float maxVolume;

	void Start () {
		fadingOut = true;
		musicAudioSource = GetComponent<AudioSource>();
		StartCoroutine(FadeInTrack());
	}

	void Update(){
		if(!(fadingOut || fadingIn) && currentTrackTime >= currentTrackLength - fadeOutTime - (Time.deltaTime * currentTrackLength)){
			//Debug.Log("CurrentTrackTime: " + currentTrackTime);
			//Debug.Log("Starting to fade out track at " + currentTrackTime);
			StartCoroutine(FadeOutTrack());
		}
		currentTrackTime += Time.deltaTime;
	}

	IEnumerator FadeOutTrack(){
		float timer = 0;
		fadingOut = true;
		while(timer <= fadeOutTime){
			musicAudioSource.volume = Mathf.SmoothStep(maxVolume, 0.1f, timer/fadeOutTime);
			timer += Time.deltaTime;
			yield return null;
		}
		//Debug.Log("Done fading out at " + Time.time);
		StartCoroutine(FadeInTrack());
		yield return null;
	}

	IEnumerator FadeInTrack(){
		float timer = 0;
		fadingIn = true;
		musicTracksCollection.Play(musicAudioSource);
		currentTrackStartTime = Time.time;
		currentTrackTime = 0;
		currentTrackLength = musicAudioSource.clip.length;
		//Debug.Log("Currently fading in: " + musicAudioSource.clip.name + " at " + Time.time + " with length " + currentTrackLength);
		//Debug.Log("CurrentTrackLength minus fadeOutTime: " + (currentTrackLength - fadeOutTime));
		//Debug.Log("CurrentTrackTime: " + currentTrackTime);
		//Debug.Log("Will fade out track at " + (currentTrackLength - fadeOutTime - (Time.deltaTime * currentTrackLength)));
		fadingOut = false;
		while(timer <= fadeInTime){
			//Debug.Log("currentTrackTime during coroutine: " + currentTrackTime);
			musicAudioSource.volume = Mathf.SmoothStep(0.1f, maxVolume, timer/fadeInTime);
			timer += Time.deltaTime;
			yield return null;
		}
		//Debug.Log("CurrentTrackTime: " + currentTrackTime);
		fadingIn = false;
		yield return null;
	}
}
