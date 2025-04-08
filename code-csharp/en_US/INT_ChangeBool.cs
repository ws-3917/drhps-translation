using System.Collections;
using UnityEngine;

public class INT_ChangeBool : MonoBehaviour
{
    public Component Script;

    public string BoolToFind;

    public bool WhatToSetUponInteract;

    public bool Toggleable;

    public void RUN()
    {
    }

    public IEnumerator DebounceInteract()
    {
        yield return new WaitForSeconds(0.3f);
    }
}
