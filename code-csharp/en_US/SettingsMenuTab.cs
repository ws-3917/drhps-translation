using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SettingsMenuTab
{
    public GameObject Tab;

    public Image TabBackground;

    public RawImage TabIcon;

    public TextMeshProUGUI TabText;

    public float Tabs_TargetSize;

    public GonerMenu_Settings.SettingsMenuTabs TabToOpen;

    public GonerMenu_SettingsSection SettingSection;
}
