using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentPanel : MonoBehaviour
{
    // TODO: Figure out a different way to handle the AudioClip input. It is only an array right now because arrays are serializable while AudioClips are not
    [SerializeField] AudioClip instrument;
    private AudioRecorderManager recordingManager;
    private RecordedInstrument recordedInstrument;
    private AudioSource audioSource;
    private double lastPlayedTimestamp;
    private double clipDuration;
    private bool hasBeenAddedToManager = false;

    // Start is called before the first frame update
    void Start()
    {
        recordedInstrument = new RecordedInstrument(instrument);
        lastPlayedTimestamp = AudioSettings.dspTime;
        recordingManager = GameObject.Find("Input Receiver").GetComponent<AudioRecorderManager>();
        clipDuration = (double) instrument.samples / instrument.frequency;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter() {
        if ( !isFinishedPlaying() ) { 
            return;
        }

        lastPlayedTimestamp = AudioSettings.dspTime;
        recordedInstrument.playOneShot();

        if (recordingManager.isRecording()) {
            //Add instrument at timestamp to loop
            double timeElapsed = lastPlayedTimestamp - recordingManager.getStartTime();
            Debug.Log("********************** Adding New Sound: " + instrument.name + " at offset: " + timeElapsed);
            recordedInstrument.addNewRecording(audioSource, timeElapsed);
        }
    }

    private bool isFinishedPlaying() {
        return !((AudioSettings.dspTime - lastPlayedTimestamp) < clipDuration);
    }
}
