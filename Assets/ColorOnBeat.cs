using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorOnBeat : MonoBehaviour
{
    [Header("Behavior Settings")]
    public Transform _target;
    private MeshRenderer _meshRenderer;
    public Material _material;
    private Material _materialInstance;
    public Color _color;
    public string _colorProperty;

    private float _strength;
    [Range(0.8f,0.99f)]
    public float _fallbackFactor;
    [Range(1,4)]
    public float _colorMultiplier;

    private List<int> beatsToPlayOn;
    private InstrumentPanel instrumentReference;
    private PlayerScheduler playerScheduler;
    public int beatNumber;

    private bool _IsPlayable;

    // Start is called before the first frame update
    void Start()
    {
        if (_target != null) {
            _meshRenderer = _target.GetComponent<MeshRenderer>();
        } else {
            _meshRenderer = GetComponent<MeshRenderer>();
        }
        _strength = 0;
        _materialInstance = new Material(_material);
        _materialInstance.EnableKeyword("_EMISSION");
        _meshRenderer.material = _materialInstance;
        _IsPlayable = false;

        // TODO: Refactor this so I don't weep inside
        instrumentReference = transform.parent.parent.parent.gameObject.GetComponent<InstrumentPanel>();
        playerScheduler = GameObject.Find("Player").GetComponent<PlayerScheduler>();
    }

    // Update is called once per frame
    void Update()
    {
        beatsToPlayOn = instrumentReference.getBeatsToPlayOn();

        CheckBaseColor();
        if (_strength > 0){
            _strength *= _fallbackFactor;
        } else {
            _strength = 0;
        }

        CheckBeat();
        _materialInstance.SetColor(_colorProperty, _color * _strength *_colorMultiplier);
    }

    private void CheckBaseColor(){
        Debug.Log(beatsToPlayOn[beatNumber] + "_IS PLAYABLE " + _IsPlayable);
        if (beatsToPlayOn[beatNumber] == 1 && _IsPlayable == false) {
            Debug.Log("GREEN");
            _materialInstance.SetColor("_Color", Color.green);
            _IsPlayable = true;
        } else if (beatsToPlayOn[beatNumber] == 0 && _IsPlayable == true) {
            Debug.Log("BLACK ");
            _materialInstance.SetColor("_Color", Color.white);
            _IsPlayable = false;
        }
    }

    void Colorize(){
        _strength = 1;
    }

    private void CheckBeat() {
        // beatsToColorOn = instrumentReference.getRecordedInstrumentReference().getBeatsToColorOn();
        if ( playerScheduler.getCurrentBeatInMeasure() == beatNumber) {
            Colorize();
        }
    }
}
