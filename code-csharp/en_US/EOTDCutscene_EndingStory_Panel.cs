using System;
using UnityEngine;

[Serializable]
public class EOTDCutscene_EndingStory_Panel
{
    [Header("Panel Settings")]
    public Sprite PanelSprite;

    public float PanelLength = 1f;

    [Header("Fade Settings")]
    public bool FadeIn = true;

    public bool FadeOut = true;

    public Color FadeColor;

    public float FadeSpeedMultiplier = 1f;

    [Header("Misc")]
    public bool IsThreeCharacterPanel;
}
