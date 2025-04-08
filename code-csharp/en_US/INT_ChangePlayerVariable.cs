using System.Collections;
using UnityEngine;

public class INT_ChangePlayerVariable : MonoBehaviour
{
    public PlayerManager PlayerManager;

    public object PlayerManagerObject;

    public string BoolToFind;

    public bool WhatToSetUponInteract;

    public bool CanUse = true;

    public void RUN()
    {
        if (BoolToFind == "SpeedrunText")
        {
            CanUse = false;
            StartCoroutine(DebounceInteract());
            return;
        }
        Debug.LogWarning("No bool of that name! | " + base.name + " " + base.transform.position.ToString() + " " + Time.time);
    }

    public IEnumerator DebounceInteract()
    {
        yield return new WaitForSeconds(0.3f);
        CanUse = true;
    }
}
