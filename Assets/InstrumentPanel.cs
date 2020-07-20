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
    
    [SerializeField] private GameObject gameObjectReference;
    [SerializeField] private RecordedInstrument recordedInstrumentReference;

    // Start is called before the first frame update
    void Start()
    {
        addToRecordingManager();
        lastPlayedTimestamp = AudioSettings.dspTime;
        clipDuration = (double) instrument.samples / instrument.frequency;
    }

    public void updateVolume(float newVolume){
        recordedInstrument.setNewVolume(newVolume);
    }
    public void updateNumSteps(float newNumSteps){
        recordedInstrument.setNumSteps((int)newNumSteps);
    }

    public void updateOffset(float newOffset){
        recordedInstrument.setOffset((int)newOffset);
    }

    private void addToRecordingManager() {
        playerScheduler = GameObject.Find("Player").GetComponent<PlayerScheduler>();
        recordedInstrument = playerScheduler.addNewInstrument(instrument);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter() {
        if ( !isFinishedPlaying() || recordedInstrument.numOfSteps > 0) { 
            return;
        }

        lastPlayedTimestamp = AudioSettings.dspTime;
        recordedInstrument.playOneShot();

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
}
