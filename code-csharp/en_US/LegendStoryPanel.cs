using System;
using UnityEngine;

[Serializable]
public class LegendStoryPanel
{
    [Header("Leave Empty to continue previous panel")]
    public string StoryPanelName;

    public int StoryPanelTextIndex;

    public float PanelTimeLength;

    public bool StartSecondaryMusic;
}
