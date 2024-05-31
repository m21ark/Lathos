using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{

    [Header("Volume")]
    [Range(0, 1)]
    public float masterVolume = 1;
    [Range(0, 1)]
    public float sfxVolume = 1;
    [Range(0, 1)]
    public float musicVolume = 1;
    [Range(0, 1)]
    public float ambienceVolume = 1;

    private Bus masterBus;
    private Bus sfxBus;
    private Bus musicBus;
    private Bus ambienceBus;

    public static AudioManager instance { get; private set;}

    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> emitters;
    private EventInstance ambienceInstance;
    private EventInstance musicInstance;

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("More than one AudioManager in the scene");
        else instance = this;

        eventInstances = new List<EventInstance>();
        emitters = new List<StudioEventEmitter>();

        // commented for now because we don't have any buses logic set up yet
        // masterBus = RuntimeManager.GetBus("bus:/");
        // sfxBus = RuntimeManager.GetBus("bus:/SFX");
        // musicBus = RuntimeManager.GetBus("bus:/Music");
        // ambienceBus = RuntimeManager.GetBus("bus:/Ambience");

        UpdateVolumes();
    }

    // can be used to play a sound that doesn't need to be stopped manually like a gunshot
    public void PlayOneShot(EventReference sound, Vector3 position, float delay = 0.0f)
    {
        StartCoroutine(PlayOneShotCoroutine(sound, position, delay));
    }

    // Coroutine to handle the delay
    private IEnumerator PlayOneShotCoroutine(EventReference sound, Vector3 position, float delay)
    {
        if (delay > 0.0f) yield return new WaitForSeconds(delay);
        RuntimeManager.PlayOneShot(sound, position);
    }

    // can be used to play a sound that needs to be stopped manually like footsteps
    public EventInstance CreateInstance(EventReference sound)
    {
        EventInstance sound_instance = RuntimeManager.CreateInstance(sound);
        eventInstances.Add(sound_instance);
        return sound_instance;
    }

    public int CreateInstanceIDOnPlayer(EventReference sound)
    {
        EventInstance sound_instance = RuntimeManager.CreateInstance(sound);
        instance.eventInstances.Add(sound_instance);
        return instance.eventInstances.Count - 1;
    }

    public void PlayInstanceIfNotPlayingOnPlayer(int sound_instance_id){
        EventInstance sound_instance = eventInstances[sound_instance_id];
        sound_instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(GameLogic.instance.player.transform.position));
        PLAYBACK_STATE state;
        sound_instance.getPlaybackState(out state);
        if (state == PLAYBACK_STATE.STOPPED)
            sound_instance.start();
    }

    public void StopInstancePlayingOnPlayer(int sound_instance_id){
        EventInstance sound_instance = eventInstances[sound_instance_id];
        PLAYBACK_STATE state;
        sound_instance.getPlaybackState(out state);
        if (state == PLAYBACK_STATE.PLAYING)
            sound_instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void PlayInstanceIfNotPlaying(EventInstance sound)
    {
        PLAYBACK_STATE state;
        sound.getPlaybackState(out state);
        if (state == PLAYBACK_STATE.STOPPED)
            sound.start();
    }

    public void StopInstance(EventInstance sound)
    {
        PLAYBACK_STATE state;
        sound.getPlaybackState(out state);
        if (state == PLAYBACK_STATE.PLAYING)
            sound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    // can be used to play sounds near a GameObject like a coin hover chime or fire crackling
    public StudioEventEmitter CreateEmitter(EventReference sound, GameObject gameObject)
    {
        StudioEventEmitter emitter = gameObject.AddComponent<StudioEventEmitter>();
        emitter.EventReference = sound;
        emitters.Add(emitter);
        return emitter; // emitter.Play() to play the sound and emitter.Stop() to stop it
    }

    private void InitializeAmbience(EventReference sound)
    {
        ambienceInstance = RuntimeManager.CreateInstance(sound);
        ambienceInstance.start();
    }

    public void SetAmbienceParameter(string parameter, float value)
    {
        ambienceInstance.setParameterByName(parameter, value);
    }

    private void InitializeMusic(EventReference sound)
    {
        musicInstance = RuntimeManager.CreateInstance(sound);
        musicInstance.start();
    }

    private void Start(){
        // Commented for now because we don't have any ambience & music set up yet
        // InitializeAmbience(FMODEvents.instance.ambience);
        // InitializeMusic(FMODEvents.instance.music);
    }

    private void UpdateVolumes(){
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        ambienceVolume = PlayerPrefs.GetFloat("AmbienceVolume", 1f);

        masterBus.setVolume(masterVolume);
        sfxBus.setVolume(sfxVolume);
        musicBus.setVolume(musicVolume);
        ambienceBus.setVolume(ambienceVolume);
    }

    public void OnDestroy()
    {
        foreach (EventInstance instance in eventInstances){
            instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            instance.release();
        }       

        foreach (StudioEventEmitter emitter in emitters)
            emitter.Stop();
    }

    public void PlayEnding(int num){
        switch(num){
            case 0:
                RuntimeManager.PlayOneShot(FMODEvents.instance.endingFighter);
                break;
            case 1:
                RuntimeManager.PlayOneShot(FMODEvents.instance.endingRanger);
                break;
            case 2:
                RuntimeManager.PlayOneShot(FMODEvents.instance.endingMage);
                break;
        }
    }

    public void PlayOpening(int num){
        switch(num){
            case 0:
                RuntimeManager.PlayOneShot(FMODEvents.instance.opening0);
                break;
            case 1:
                RuntimeManager.PlayOneShot(FMODEvents.instance.opening1);
                break;
            case 2:
                RuntimeManager.PlayOneShot(FMODEvents.instance.opening2);
                break;
            case 3:
                RuntimeManager.PlayOneShot(FMODEvents.instance.opening3);
                break;
        }
    }
}
