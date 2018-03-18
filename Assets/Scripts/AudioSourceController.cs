using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioSourceController : MonoBehaviour {

	public AudioClip backgroundMusicClip;

	private AudioSource backgroundMusic;
	private AudioSource soundEffects;


	[HideInInspector]
	public static AudioSourceController instance;


	private Queue<AudioClip> queuedSoundEffects;




	// Use this for initialization
	void Start () {

		instance = this;

		queuedSoundEffects = new Queue<AudioClip>();

		backgroundMusic = GameObject.Find("BGMusic").GetComponent<AudioSource>();
		if(backgroundMusic == null){
			throw new Exception("Audio source controller could not find sound effects object");
		}
		soundEffects = GameObject.Find("SoundEffects").GetComponent<AudioSource>();
		if(soundEffects == null){
			throw new Exception("Audio source controller could not find sound effects object");
		}


		backgroundMusic.clip = backgroundMusicClip;
		backgroundMusic.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(soundEffects.isPlaying) return;

		if(queuedSoundEffects.Count == 0) return;

		soundEffects.clip = queuedSoundEffects.Dequeue();

		soundEffects.Play();

	
	}


	public void PushClip(String clipName){
		AudioClip clip = Resources.Load(String.Format("Sounds/{0}", clipName)) as AudioClip;
		if(clip == null) return;
		queuedSoundEffects.Enqueue(clip);
	}



}
