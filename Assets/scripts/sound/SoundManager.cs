using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : Singleton<SoundManager> {

    public Sound[] sounds;

    protected SoundManager() { }

    void Awake() {
       foreach(Sound sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    public void Play(string name) {
        Sound sound = Array.Find(sounds, s => s.name == name);
        sound.source.Play();
    }
}
