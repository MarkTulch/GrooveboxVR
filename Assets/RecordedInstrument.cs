using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordedInstrument : MonoBehaviour
{
    [SerializeField] private List<AudioSource> audioSources;
    [SerializeField] private List<double> offsetTimestamps;
    public AudioClip instrument {get; set;}
    private AudioSource oneShotAudioSource;

    private bool hasBeenAddedToManager = false;

    private AudioRecorderManager recordingManager;

    public void setInstrument(AudioClip instrumentClip) { 
        instrument = instrumentClip;
    }

    public /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        oneShotAudioSource = gameObject.AddComponent<AudioSource>();
        clearAllRecordings();
    }
    public void clearAllRecordings() { 
        Debug.Log("******************Instrument cleared");
        offsetTimestamps = new List<double>();
        audioSources = new List<AudioSource>();
    }

    public void addNewRecording(AudioSource audioSource, double offset){
        GameObject child = new GameObject(instrument.name);
        child.transform.parent = gameObject.transform;
        AudioSource tempAudioSource = child.AddComponent<AudioSource>();
        tempAudioSource.clip = instrument;
        audioSources.Add(tempAudioSource);
        offsetTimestamps.Add(offset);
    }

    public void playOneShot(){
        oneShotAudioSource.PlayOneShot(instrument);
    }

    public void scheduleAllSounds(double loopStartTime) {
        if ( offsetTimestamps == null ){
            return;
        }

        for (int i = 0; i < offsetTimestamps.Count; i++ ) {
            Debug.Log("Scheduling " + i + " audio source at " + (loopStartTime + offsetTimestamps[i]));
            audioSources[i].PlayScheduled(loopStartTime + offsetTimestamps[i]);
        }
    }
}
