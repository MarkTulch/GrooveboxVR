using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] TMP_Text volumeText;
    [SerializeField] GameObject joint;
    // Start is called before the first frame update
    void Start()
    {
        joint = gameObject.transform.Find("Internal/JointContainer/Joint").gameObject;
        volumeText.text = "100%";
    }

    // Update is called once per frame
    void Update()
    {
        volumeText.text = calculateRotationPercentage();
    }

    private string calculateRotationPercentage(){
        float percentage = joint.transform.rotation.eulerAngles.z/360f;
        int displayVal = 100 - (int)(Mathf.Floor(percentage*100f));
        return (displayVal + "%");
    }
}
