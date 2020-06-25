using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRecorderManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> instruments;
    private bool recordingStatus = false;
    private double loopStartTime;
    private bool running = false;
    private double beatLength;
    private double barLength;
    private double barSequenceLength;
    private RecordedInstrument recordedInstrument;

    public int bpm = 140;

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
        barSequenceLength = barLength * 4;
    }
    
    private void scheduleAllInstruments() {
        Debug.Log("*********************** Instrument Count:"+  instruments.Count);
        foreach( GameObject instrument in instruments) {
            recordedInstrument = instrument.GetComponent<RecordedInstrument>();
            recordedInstrument.scheduleAllSounds(loopStartTime);
        }
    }

    void Update()
    {
        if (!running) {
            return;
        }

        // Scheduling from the top of the measure 
        if (( AudioSettings.dspTime - loopStartTime ) >= barSequenceLength) {
            loopStartTime = AudioSettings.dspTime;
            scheduleAllInstruments();
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
