using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_KrisPianoAnimation : MonoBehaviour
{
    [Serializable]
    public class KeyPress
    {
        public int Key;

        public float TimeSinceLast;

        public KeyPress(int key, float timeSinceLast)
        {
            Key = key;
            TimeSinceLast = timeSinceLast;
        }
    }

    public List<KeyPress> keyPresses = new List<KeyPress>();

    public List<KeyPress> DWkeyPresses = new List<KeyPress>();

    private float lastPressTime;

    private PlayerManager Kris;

    public bool IsDarkworldVarient;

    private void Start()
    {
        Kris = PlayerManager.Instance;
    }

    private void Update()
    {
        if (DRHDebugManager.instance.DebugModeEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                RecordKey(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                RecordKey(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                RecordKey(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                RecordKey(4);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Playback());
            }
        }
    }

    public void PlayRecording()
    {
        StartCoroutine(Playback());
        StartCoroutine(DWPlayback());
    }

    private void RecordKey(int key)
    {
        float time = Time.time;
        float num;
        if (IsDarkworldVarient)
        {
            num = ((DWkeyPresses.Count == 0) ? 0f : (time - lastPressTime));
            lastPressTime = time;
            DWkeyPresses.Add(new KeyPress(key, num));
        }
        else
        {
            num = ((keyPresses.Count == 0) ? 0f : (time - lastPressTime));
            lastPressTime = time;
            keyPresses.Add(new KeyPress(key, num));
        }
        Debug.Log($"Recorded key {key} with time since last: {num}");
    }

    private IEnumerator Playback()
    {
        Debug.Log("Starting playback...");
        foreach (KeyPress keyPress in keyPresses)
        {
            yield return new WaitForSeconds(keyPress.TimeSinceLast);
            Kris._PMove._anim.Play($"Kris_CM_LWPianoNote{keyPress.Key}");
            Debug.Log($"Key: {keyPress.Key}");
        }
        Debug.Log("Playback finished.");
    }

    private IEnumerator DWPlayback()
    {
        foreach (KeyPress keyPress in DWkeyPresses)
        {
            yield return new WaitForSeconds(keyPress.TimeSinceLast);
            if (IsDarkworldVarient)
            {
                Kris._PMove._anim.Play($"Kris_CM_DWPianoNote{keyPress.Key}");
            }
            Debug.Log($"Key: {keyPress.Key}");
        }
    }
}
