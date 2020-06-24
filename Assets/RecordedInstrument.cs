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

    [SerializeField] private GameObject gameObjectReference;
    [SerializeField] private RecordedInstrument selfReference;

    public RecordedInstrument(AudioClip instrumentClip) { 
        instrument = instrumentClip;
        clearAllRecordings();
        addToRecordingManager();
    }
    public void clearAllRecordings() { 
        Debug.Log("Instrument cleared");
        offsetTimestamps = new List<double>();
        audioSources = new List<AudioSource>();
    }

    private void addToRecordingManager() {
        Debug.Log("******************Finding Input Receiver");
        recordingManager = GameObject.Find("Input Receiver").GetComponent<AudioRecorderManager>();
        gameObjectReference = recordingManager.addNewInstrument(this);
        selfReference = gameObjectReference.GetComponentInChildren<RecordedInstrument>();
        oneShotAudioSource = gameObjectReference.GetComponentInChildren<AudioSource>();
    }

    public void addNewRecording(AudioSource audioSource, double offset){
        GameObject child = new GameObject(instrument.name);
        child.transform.parent = gameObjectReference.transform;
        selfReference.audioSources.Add(child.AddComponent<AudioSource>());
        selfReference.offsetTimestamps.Add(offset);
    }

    public void playOneShot(){
        oneShotAudioSource.PlayOneShot(instrument);
    }

    public void scheduleAllSounds(double loopStartTime) {
        if ( offsetTimestamps == null ){
            return;
        }

        for (int i = 0; i < offsetTimestamps.Count; i++ ) {
            audioSources[i].PlayScheduled(loopStartTime + offsetTimestamps[i]);
        }
    }
}
