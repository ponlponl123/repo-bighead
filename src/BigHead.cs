using System.Collections;
using System.Collections.Generic;
using BepInEx;
using UnityEngine;
using UnityEngine.SceneManagement;

[BepInPlugin("Ponlponl123.BigHead", "BigHead", "0.1.0")]
[BepInDependency(REPOLib.MyPluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.HardDependency)]
public class BigHead : MonoBehaviour
{
    public float minScale = 1.0f;
    public float maxScale = 2.0f;
    public float scaleSpeed = 5.0f;

    List<MicrophoneVolumeListener> listeners = new List<MicrophoneVolumeListener>();

    void RegisterListeners()
    {
        // Clear old listeners
        foreach (var listener in listeners)
        {
            if (listener != null)
                Destroy(listener);
        }
        listeners.Clear();

        // Find all player heads and add new listeners
        GameObject[] allHeads = GameObject.FindGameObjectsWithTag("PlayerHead");
        foreach (var head in allHeads)
        {
            var listener = head.GetComponent<MicrophoneVolumeListener>();
            if (listener == null)
                listener = head.AddComponent<MicrophoneVolumeListener>();
            listeners.Add(listener);
        }
    }
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Level changed to: " + scene.name);
        RegisterListeners();
    }

    void Start()
    {
        Debug.Log("BigHead Mods Loaded!");
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        foreach (var listener in listeners)
        {
            if (listener == null) continue;
            Transform head = listener.transform;

            // Map volume (0..1) to scale (minScale..maxScale)
            float targetScale = Mathf.Lerp(minScale, maxScale, Mathf.Clamp01(listener.volume));

            // Smoothly interpolate current scale to target scale
            head.localScale = Vector3.Lerp(head.localScale, Vector3.one * targetScale, Time.deltaTime * scaleSpeed);
        }
    }
}
