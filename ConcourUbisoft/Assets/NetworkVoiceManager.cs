using Photon.Voice.Unity;
using UnityEngine;

[RequireComponent(typeof(VoiceConnection))]
public class NetworkVoiceManager : MonoBehaviour
{
    private Recorder recorder;
    
    private void Awake()
    {
        recorder = GetComponent<Recorder>();
        recorder.IsRecording = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            recorder.IsRecording = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            recorder.IsRecording = false;
        }
    }
}
