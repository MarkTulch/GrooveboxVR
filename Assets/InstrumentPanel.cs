using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentPanel : MonoBehaviour
{
    [SerializeField] AudioClip instrument;
    private PlayerScheduler playerScheduler;
    private RecordedInstrument recordedInstrument;
    private AudioSource audioSource;
    private double lastPlayedTimestamp;
    private double clipDuration;
    private bool hasBeenAddedToManager = false;
    private bool isRunning;
    private float volume;
    private int numOfSteps;
    private int offset;

    [SerializeField] private List<int> beatsToPlayOn;
    [SerializeField] private AudioSource oneShotAudioSource;    
    [SerializeField] private List<AudioSource> audioSources;

    // Start is called before the first frame update
    void Start()
    {
        isRunning = true;
        oneShotAudioSource = gameObject.AddComponent<AudioSource>();
        oneShotAudioSource.clip = instrument;
        volume = 1f;
        beatsToPlayOn = new List<int>();

        playerScheduler = GameObject.Find("Player").GetComponent<PlayerScheduler>();
        SetupAudioSources();
        playerScheduler.addNewInstrument(this);

        CalculateEuclidianRhythmArray(playerScheduler.beatsPerMeasure, numOfSteps, offset);

        lastPlayedTimestamp = AudioSettings.dspTime;
        clipDuration = (double) instrument.samples / instrument.frequency;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning) {
            return;
        }

        // If next beat is one to play on, schedule
        if ( beatsToPlayOn[playerScheduler.getNextBeatInMeasure()] == 1) {
            audioSources[playerScheduler.getNextBeatInMeasure()].PlayScheduled(playerScheduler.getNextBeatStart());
            // oneShotAudioSource.PlayScheduled(playerScheduler.getNextBeatStart());
        }
    }

    // Creates audio sources equal to the number of beats in each measure
    // Might not need this method with the new configuration
    private void SetupAudioSources(){
        for (int i = 0; i < playerScheduler.beatsPerMeasure; i++ ){
            GameObject child = new GameObject(instrument.name);
            child.transform.parent = gameObject.transform;
            AudioSource tempAudioSource = child.AddComponent<AudioSource>();
            tempAudioSource.clip = instrument;
            tempAudioSource.volume = volume;
            audioSources.Add(tempAudioSource);
        }
    }

    public void setNumSteps(float newNumSteps){
        numOfSteps = (int) newNumSteps;
        CalculateEuclidianRhythmArray(playerScheduler.beatsPerMeasure, numOfSteps, offset);
    }

    public void setOffset(float newOffset){
        offset = (int) newOffset;
        CalculateEuclidianRhythmArray(playerScheduler.beatsPerMeasure, numOfSteps, offset);
    }

    public void setVolume(float newVolume){
        volume = newVolume;
        foreach (AudioSource source in audioSources) {
            source.volume = volume;
        }
        oneShotAudioSource.volume = volume;
    }

    public void togglePlaying(){
        isRunning = !isRunning;
    }
    
    public List<int> getBeatsToPlayOn(){
        return beatsToPlayOn;
    }

    private void OnTriggerEnter() {
        if ( !isFinishedPlaying() || numOfSteps > 0) { 
            return;
        }

        lastPlayedTimestamp = AudioSettings.dspTime;
        oneShotAudioSource.Play();

        // if (recordingManager.isRecording()) {
        //     //Add instrument at timestamp to loop
        //     double timeElapsed = lastPlayedTimestamp - recordingManager.getStartTime();
        //     Debug.Log("********************** Adding New Sound: " + instrument.name + " at offset: " + timeElapsed);
        //     recordedInstrument.addNewRecording(audioSource, timeElapsed);
        // }
    }

    private bool isFinishedPlaying() {
        return !((AudioSettings.dspTime - lastPlayedTimestamp) < clipDuration);
    }

    // Calculates an array of which beats to play on 
    private void CalculateEuclidianRhythmArray(int timeIntervalsN, int pulsesK, int rotationR){
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
    }
    
    private List<int> rotateSeq(int rotation){
        List<int> output = new List<int>();
        int count = beatsToPlayOn.Count;
        int val = count - rotation;

        for (int i =0; i < count; i++ ) {
            output.Add(beatsToPlayOn[Mathf.Abs((i+val)%count)]);
        }
        return output;
    }
}
