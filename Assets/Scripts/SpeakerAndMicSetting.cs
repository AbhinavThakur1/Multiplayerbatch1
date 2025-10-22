using UnityEngine;
using Photon.Pun;
using Photon.Voice.Unity;
using UnityEngine.UI;

public class SpeakerAndMicSetting : MonoBehaviourPunCallbacks
{
    [SerializeField] Button _micOnOff;
    [SerializeField] Sprite _micOnSP, _micOffSP;
    [SerializeField] Recorder _audioRecorder;
    [SerializeField] Slider _speakerSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _micOnOff.onClick.AddListener(MicOnOff);
        _speakerSlider.onValueChanged.AddListener(SpeakerVolume);
    }

    void MicOnOff()
    {
        if (_audioRecorder.TransmitEnabled)
        {
            _audioRecorder.TransmitEnabled = false;
            _micOnOff.GetComponent<Image>().sprite = _micOffSP;
        }
        else
        {
            _audioRecorder.TransmitEnabled = true;
            _micOnOff.GetComponent<Image>().sprite = _micOnSP;
        }
    }

    void SpeakerVolume(float _value)
    {
        foreach(GameObject speaker in GameObject.FindGameObjectsWithTag("Speaker"))
        {
            speaker.GetComponent<AudioSource>().volume = _value;
        }
    }

}
