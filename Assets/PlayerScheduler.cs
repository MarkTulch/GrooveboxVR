using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScheduler : MonoBehaviour
{
    [SerializeField] private List<GameObject> instruments;
    [SerializeField] private double[] beats;
    private bool recordingStatus = false;
    private double loopStartTime;
    private bool running = false;
    private double beatLength;
    private double barLength;
    private double barSequenceLength;
    private RecordedInstrument recordedInstrument;

    // LinkedList<Measures[]> loop;
    
    // Update()
    // if (!isRunning()){ return false;}

    // for each StartTimeObject in StartTimeList {
    //      tell it to tell its measures to start
    //      wait until next StartTimeObject.startTime
    //      StartTimeList.next{}
    // }
    
    // StartTimeObject
    //      Array of measures
    //          Each measure plays itself once started



    // addNewMeasureToChain(measure)
    // if (adding above or below && is not the new longest measure) {
    //      totalLengthOfMeasure does not change
    // } else if { 
    // } else if {

    // }





/*

Coroutines brainstorming

MeasureObject:
    If told to play by the player:
        


*/
    public int bpm = 154;

    public void flipRecordingState() {
        recordingStatus = !recordingStatus;
    }

    public bool isRecording(){
        return recordingStatus;
    }

    void Start()
    {
        Debug.Log("***********************We're starting ok");
        running = true;
        loopStartTime = AudioSettings.dspTime;
        instruments = new List<GameObject>();

        // Duration of one beat:
        beatLength = 60d/bpm;
        barLength = beatLength * 4;
        barSequenceLength = barLength * 8;
    }
    
    private void scheduleAllInstruments() {
        Debug.Log("*********************** Instrument Count:"+  instruments.Count);
        foreach( GameObject instrument in instruments) {
            recordedInstrument = instrument.GetComponent<RecordedInstrument>();
            recordedInstrument.scheduleAllSounds(beats);
        }
    }

    void Update()
    {
        if (!running) {
            return;
        }

        // Scheduling from the top of the measure 
        if (( AudioSettings.dspTime - loopStartTime ) >= (beatLength * 8 )) {
            loopStartTime = AudioSettings.dspTime;
            calculateBeats();
            scheduleAllInstruments();
        }
    }

    private void calculateBeats() {
        int numSteps = 8;
        beats = new double[numSteps];
        for (int i = 0; i< numSteps; i++ ) {
            beats[i] = loopStartTime + (beatLength * i);
        }
    }

    public double getStartTime() {
        return loopStartTime;
    }

    public RecordedInstrument addNewInstrument(AudioClip instrument) {
        Debug.Log("**********************************Adding new instrument...." + instrument.name);
        GameObject child = new GameObject(instrument.name + " Instrument");
        child.transform.parent = gameObject.transform;
        RecordedInstrument newRecordedInstrument = child.AddComponent<RecordedInstrument>();
        newRecordedInstrument.setInstrument(instrument);
        instruments.Add(child);
        return newRecordedInstrument;
    }
}
