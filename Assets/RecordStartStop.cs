using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecordStartStop : MonoBehaviour
{
    public PlayerScheduler playerScheduler;

    public TMP_Text recordButtonText;

    // Start is called before the first frame update
    void Start()
    {
        recordButtonText.text = "Off";
    }

    private void OnTriggerEnter() {
        // Flip between start/stop
        playerScheduler.flipRecordingState();
        // Get current recording state and update appearance
        switch (playerScheduler.isRecording()){
            case true:
                recordButtonText.text = "Recording...";
                break;
            case false:
                recordButtonText.text = "Off";
                break;
        }
    }
}
