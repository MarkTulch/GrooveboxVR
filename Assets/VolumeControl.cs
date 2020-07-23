using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] TMP_Text volumeText;
    [SerializeField] GameObject joint;
    [SerializeField] InstrumentPanel parent;
    private int rotationPercentage;
    // Start is called before the first frame update
    void Start()
    {
        joint = gameObject.transform.Find("Internal/JointContainer/Joint").gameObject;
        parent = gameObject.transform.parent.gameObject.GetComponent<InstrumentPanel>();
        volumeText.text = "100%";
        rotationPercentage = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void updateRotation(){
        // rotationPercentage = calculateRotationPercentage();
        // parent.updateVolume(rotationPercentage);
        // volumeText.text = (rotationPercentage + "%");
    }

    public int getRotationPercent() {
        return rotationPercentage;
    }

    private int calculateRotationPercentage(){
        float degrees = joint.transform.rotation.eulerAngles.z/360f;
        int percentage = 100 - (int)(Mathf.Floor(degrees*100f));
        return percentage;
    }
}
