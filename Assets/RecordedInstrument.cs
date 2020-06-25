using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordedInstrument : MonoBehaviour
{
    [SerializeField] private List<AudioSource> audioSources;
    [SerializeField] private List<double> offsetTimestamps;

    [SerializeField] private List<int> beatsToPlayOn;
    [SerializeField] private List<int> remainder;
    [SerializeField] private List<int> count;
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
        beatsToPlayOn = new List<int>();
        clearAllRecordings();
        for (int i = 0; i < 8; i++ ){
            addNewRecording(oneShotAudioSource, 0d);
        }
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

    private List<int> euclidianRhythmArray(int timeIntervalsN, int pulsesK, int rotationR){
        rotationR = rotationR++ % timeIntervalsN;
        beatsToPlayOn = new List<int>();

        int bucket = 0;
        for (int i=0; i < timeIntervalsN; i++){
            bucket += pulsesK;
            if ( bucket >= timeIntervalsN ) {
                bucket -= timeIntervalsN;
                beatsToPlayOn.Add(1);
            } else {
                beatsToPlayOn.Add(0);
            }
        }
        if (rotationR > 0 ) {
            beatsToPlayOn = rotateSeq(rotationR);
        }
        return beatsToPlayOn;
    }

    public List<int> rotateSeq(int rotation){
        List<int> output = new List<int>();
        int count = beatsToPlayOn.Count;
        int val = count - rotation;

        for (int i =0; i < count; i++ ) {
            output.Add(beatsToPlayOn[Mathf.Abs((i+val)%count)]);
        }
        return output;
    }

    public void scheduleAllSounds(double[] beats) {
        beatsToPlayOn = euclidianRhythmArray(beats.Length, 5, 0);
        for (int i = 0; i < beats.Length; i++ ) {
            if ( beatsToPlayOn[i] == 1 ) {
                audioSources[i].PlayScheduled(beats[i]);
            }
        }
        // for (int i = 0; i < offsetTimestamps.Count; i++ ) {
        //     Debug.Log("Scheduling " + i + " audio source at " + (loopStartTime + offsetTimestamps[i]));
        //     audioSources[i].PlayScheduled(loopStartTime + offsetTimestamps[i]);
        // }
    }
}
