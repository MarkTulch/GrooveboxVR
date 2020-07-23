using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScheduler : MonoBehaviour
{
    private static PlayerScheduler _PlayerSchedulerInstance;
    [SerializeField] private List<InstrumentPanel> instruments;
    private bool recordingStatus = false;
    private int currentBeat;
    public double currentBeatStartTime;
    public double nextBeatStartTime;
    private bool running = false;
    private double beatLength;
    private double barLength;
    private double barSequenceLength;

    [SerializeField] public float bpm;
    public int beatsPerMeasure;

    public void flipRecordingState() {
        recordingStatus = !recordingStatus;
    }

    public bool isRecording(){
        return recordingStatus;
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (_PlayerSchedulerInstance != null && _PlayerSchedulerInstance != this) {
            Destroy(this.gameObject);
        } else {
            _PlayerSchedulerInstance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start()
    {
        Debug.Log("***********************PlayerSchedulerStarted");
        running = true;
        currentBeatStartTime = AudioSettings.dspTime;
        currentBeat = 0;
        instruments = new List<InstrumentPanel>();
        beatsPerMeasure = 16;

        bpm = 164;
        // Duration of one beat:
        beatLength = 60d/bpm;
        barLength = beatLength * 4;
        barSequenceLength = barLength * 8;
    }

    void Update()
    {
        if (!running) {
            return;
        }

        if (( AudioSettings.dspTime - currentBeatStartTime) >= (beatLength)) {
            currentBeat++;
            currentBeatStartTime = AudioSettings.dspTime;
            nextBeatStartTime = currentBeatStartTime + beatLength;
        }
    }

    public double getNextBeatStart() {
        return nextBeatStartTime;
    }

    public int getCurrentBeatInMeasure(){
        return currentBeat % beatsPerMeasure;
    }

    public int getNextBeatInMeasure(){
        int currBeat = getCurrentBeatInMeasure();
        return currBeat >= beatsPerMeasure ? 0 : currBeat;
    }

    public void addNewInstrument(InstrumentPanel instrument) {
        instruments.Add(instrument);
    }
}
