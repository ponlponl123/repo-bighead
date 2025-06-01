using UnityEngine;

public class MicrophoneVolumeListener : MonoBehaviour
{
    public string deviceName;
    public float volume;
    private AudioClip micClip;
    private int sampleWindow = 128;

    void OnEnable()
    {
        if (Microphone.devices.Length > 0)
        {
            deviceName = Microphone.devices[0];
            micClip = Microphone.Start(deviceName, true, 1, 44100);
        }
    }

    void OnDisable()
    {
        if (micClip != null)
        {
            Microphone.End(deviceName);
        }
    }

    void Update()
    {
        volume = GetMaxVolume();
    }

    float GetMaxVolume()
    {
        if (micClip == null) return 0f;
        int micPosition = Microphone.GetPosition(deviceName) - sampleWindow + 1;
        if (micPosition < 0) return 0f;
        float[] samples = new float[sampleWindow];
        micClip.GetData(samples, micPosition);
        float max = 0;
        foreach (var sample in samples)
        {
            float abs = Mathf.Abs(sample);
            if (abs > max) max = abs;
        }
        return max;
    }
}
