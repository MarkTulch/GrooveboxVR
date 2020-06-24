using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoopRecorder : MonoBehaviour
{
    public float bpm = 140.0f;
    public int numBeatsPerSegment = 16;
    public AudioClip[] clips = new AudioClip[4];

    private double nextEventTime;
    private int audioSourceIterator = 0;
    private AudioSource[] audioSources = new AudioSource[4];
    private bool running = false;
    // Start is called before the first frame update
    void Start()
    {
        for ( int i = 0; i < clips.Length; i ++ ){
            GameObject child = new GameObject("Player");
            child.transform.parent = gameObject.transform;
            Debug.Log("Processed: " + i);
            audioSources[i] = child.AddComponent<AudioSource>();
        }
        nextEventTime = AudioSettings.dspTime + 2.0f;
        running = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!running) {
            return;
        }

        double time = AudioSettings.dspTime;

        if (time + 1.0f > nextEventTime ) {
            audioSources[audioSourceIterator].clip = clips[audioSourceIterator];
            audioSources[audioSourceIterator].PlayScheduled(nextEventTime);
            
            Debug.Log("Scheduled source " + audioSourceIterator + " to start at time " + nextEventTime);

            nextEventTime += 60.0f / bpm * numBeatsPerSegment;
            if ( audioSourceIterator + 1 < clips.Length ) {
                audioSourceIterator += 1;
            }
            else {
                audioSourceIterator = 0;
            }
        }
    }
}
