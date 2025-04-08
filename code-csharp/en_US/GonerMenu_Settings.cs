using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GonerMenu_Settings : MonoBehaviour
{
    public enum CurrentSettingsMenuState
    {
        NoTabSelected = 0,
        TabSelected = 1,
        OptionSelected = 2
    }

    public enum SettingsMenuTabs
    {
        GAME = 0,
        AUDIO = 1,
        CONTROLS = 2,
        ACCESSIBILITY = 3
    }

    public bool SettingsMenuOpen;

    [Header("-= Main =-")]
    public CurrentSettingsMenuState CurrentMenuState;

    public SettingsMenuTabs CurrentSelectedTab;

    [Header("-= References =-")]
    [SerializeField]
    private SettingsMenuTab[] SelectableTabs;

    [SerializeField]
    private RectTransform Soul;

    [SerializeField]
    private GonerMenu_SettingsSection[] SettingSections;

    [SerializeField]
    private GonerMenu GonerMenu;

    [SerializeField]
    private GameObject NewMainMenu_ConsoleMenu;

    [SerializeField]
    private GameObject NewMainMenu_SettingsParentGameobject;

    [Header("Description")]
    [SerializeField]
    private TextMeshProUGUI CurrentSelectedTitle;

    [SerializeField]
    private TextMeshProUGUI CurrentSelectedDescription;

    [Header("-= Sounds =-")]
    public AudioSource source;

    public AudioClip SettingSound_Move;

    public AudioClip SettingSound_Select;

    public AudioClip SettingSound_Tick;

    public AudioClip SettingSound_Blip;

    public AudioClip SettingSound_BackOut;

    public AudioClip SettingSound_Explosion;

    [Header("-= Dyslexic Font Setup -=")]
    [SerializeField]
    private List<TextMeshProUGUI> FontsAffectedByDyslexic = new List<TextMeshProUGUI>();

    [SerializeField]
    private TMP_FontAsset defaultFont;

    [SerializeField]
    private TMP_FontAsset dyslexicFont;

    private int CurrentTabSelected;

    private int CurrentSettingSelected;

    private bool hasUpdatedFontsDyslexic;

    private GonerMenu_SettingsSection CurrentSection;

    private static GonerMenu_Settings instance;

    public static GonerMenu_Settings Instance => instance;

    private void Awake()
    {
        if (GonerMenu != null)
        {
            if (instance != null && instance != this)
            {
                Object.Destroy(base.gameObject);
            }
            else
            {
                instance = this;
            }
        }
    }

    private void Update()
    {
        if (SettingsMenuOpen)
        {
            if (CurrentMenuState == CurrentSettingsMenuState.NoTabSelected)
            {
                NoTabSelectedUpdate();
            }
            else if (CurrentMenuState == CurrentSettingsMenuState.TabSelected)
            {
                TabSelectedUpdate();
            }
            else
            {
                OptionSelectedUpdate();
            }
            if (!hasUpdatedFontsDyslexic)
            {
                hasUpdatedFontsDyslexic = true;
                UpdateSettingFontsDyslexic();
            }
        }
        else
        {
            hasUpdatedFontsDyslexic = false;
            SettingsMenuOffUdpate();
        }
        if (!GonerMenu.Instance.GonerMenuOpen && GonerMenu != null)
        {
            SettingsMenuOpen = false;
            CurrentMenuState = CurrentSettingsMenuState.NoTabSelected;
        }
        if (GonerMenu == null)
        {
            SettingsMenuOpen = true;
        }
    }

    private void SettingsMenuOffUdpate()
    {
        for (int i = 0; i < SelectableTabs.Length; i++)
        {
            SelectableTabs[i].TabBackground.color = Color.white;
            SelectableTabs[i].TabIcon.color = Color.white;
            SelectableTabs[i].TabText.color = Color.white;
            SelectableTabs[i].Tabs_TargetSize = 0.95f;
        }
    }

    private void NoTabSelectedUpdate()
    {
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Left))
        {
            CurrentTabSelected--;
            source.PlayOneShot(SettingSound_Move);
            CurrentSettingSelected = 0;
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Right))
        {
            CurrentTabSelected++;
            source.PlayOneShot(SettingSound_Move);
            CurrentSettingSelected = 0;
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm))
        {
            source.PlayOneShot(SettingSound_Select);
            OpenSettingsTab(SelectableTabs[CurrentTabSelected].TabToOpen);
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Down))
        {
            source.PlayOneShot(SettingSound_Move);
            CurrentSettingSelected = 0;
            OpenSettingsTab(SelectableTabs[CurrentTabSelected].TabToOpen);
        }
        if (CurrentTabSelected < 0)
        {
            CurrentTabSelected = SelectableTabs.Length - 1;
        }
        if (CurrentTabSelected > SelectableTabs.Length - 1)
        {
            CurrentTabSelected = 0;
        }
        GonerMenu_SettingsSection[] settingSections = SettingSections;
        for (int i = 0; i < settingSections.Length; i++)
        {
            settingSections[i].gameObject.SetActive(value: false);
        }
        SettingSections[CurrentTabSelected].gameObject.SetActive(value: true);
        for (int j = 0; j < SelectableTabs.Length; j++)
        {
            SelectableTabs[j].TabBackground.color = Color.white;
            SelectableTabs[j].TabIcon.color = Color.white;
            SelectableTabs[j].TabText.color = Color.white;
            SelectableTabs[j].Tabs_TargetSize = 0.95f;
            SelectableTabs[CurrentTabSelected].Tabs_TargetSize = 1.3f;
            if (GonerMenu != null)
            {
                SelectableTabs[j].Tab.transform.localScale = Vector3.Lerp(SelectableTabs[j].Tab.transform.localScale, Vector3.one * SelectableTabs[j].Tabs_TargetSize, 15f * Time.unscaledDeltaTime);
            }
            SelectableTabs[CurrentTabSelected].TabBackground.color = Color.yellow;
            SelectableTabs[CurrentTabSelected].TabIcon.color = Color.yellow;
            SelectableTabs[CurrentTabSelected].TabText.color = Color.yellow;
        }
        if (GonerMenu != null)
        {
            GonerMenu.TargetSoulPosition = new Vector2(-1995f, GonerMenu.TargetSoulPosition.y);
        }
        else
        {
            Soul.position = new Vector2(-1935f, 5000f);
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Cancel))
        {
            if (GonerMenu != null)
            {
                SettingsMenuOpen = false;
                GonerMenu.CurrentMenuState = "Default";
                GonerMenu.CurrentCursorPosition = 1;
                GonerMenu.GonerMenuSource.PlayOneShot(SettingSound_BackOut);
            }
            else
            {
                SettingsMenuOpen = false;
                NewMainMenuManager.instance.MenuSource.PlayOneShot(SettingSound_BackOut);
                NewMainMenu_SettingsParentGameobject.SetActive(value: false);
                NewMainMenu_ConsoleMenu.SetActive(value: true);
            }
        }
    }

    private void TabSelectedUpdate()
    {
        for (int i = 0; i < SelectableTabs.Length; i++)
        {
            SelectableTabs[i].TabBackground.color = Color.grey;
            SelectableTabs[i].TabIcon.color = Color.grey;
            SelectableTabs[i].TabText.color = Color.grey;
            SelectableTabs[i].Tabs_TargetSize = 0.7f;
            SelectableTabs[CurrentTabSelected].Tabs_TargetSize = 1f;
            if (GonerMenu != null)
            {
                SelectableTabs[i].Tab.transform.localScale = Vector3.Lerp(SelectableTabs[i].Tab.transform.localScale, Vector3.one * SelectableTabs[i].Tabs_TargetSize, 15f * Time.unscaledDeltaTime);
            }
            SelectableTabs[CurrentTabSelected].TabBackground.color = Color.yellow;
            SelectableTabs[CurrentTabSelected].TabIcon.color = Color.yellow;
            SelectableTabs[CurrentTabSelected].TabText.color = Color.yellow;
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Up))
        {
            CurrentSettingSelected--;
            source.PlayOneShot(SettingSound_Move);
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Down))
        {
            CurrentSettingSelected++;
            source.PlayOneShot(SettingSound_Move);
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm))
        {
            CurrentSection.Settings[CurrentSettingSelected].CurrentlySelected = true;
            CurrentMenuState = CurrentSettingsMenuState.OptionSelected;
            source.PlayOneShot(SettingSound_Select);
        }
        if (CurrentSettingSelected < 0)
        {
            CurrentSettingSelected = CurrentSection.Settings.Length - 1;
        }
        if (CurrentSettingSelected > CurrentSection.Settings.Length - 1)
        {
            CurrentSettingSelected = 0;
        }
        if (CurrentSection != null)
        {
            CurrentSelectedTitle.text = CurrentSection.Settings[CurrentSettingSelected].SettingsElementName;
            CurrentSelectedDescription.text = CurrentSection.Settings[CurrentSettingSelected].SettingsDescription;
        }
        if (GonerMenu != null)
        {
            GonerMenu.TargetSoulPosition = new Vector2(-550f, CurrentSection.Settings[CurrentSettingSelected].transform.localPosition.y - 126.5f);
        }
        else
        {
            Soul.localPosition = new Vector2(-550f * base.transform.localScale.x, (CurrentSection.Settings[CurrentSettingSelected].transform.localPosition.y - 126.5f) * base.transform.localScale.x);
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Cancel))
        {
            CurrentMenuState = CurrentSettingsMenuState.NoTabSelected;
            CurrentSection = null;
            CurrentSelectedTitle.text = "";
            CurrentSelectedDescription.text = "";
            source.PlayOneShot(SettingSound_BackOut);
        }
    }

    private void OptionSelectedUpdate()
    {
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Cancel) && CurrentSection.Settings[CurrentSettingSelected].CanBeCanceled)
        {
            CurrentSection.Settings[CurrentSettingSelected].CurrentlySelected = false;
            CurrentMenuState = CurrentSettingsMenuState.TabSelected;
            source.PlayOneShot(SettingSound_BackOut);
        }
    }

    public void CancelOutOfCurrentSetting()
    {
        CurrentSection.Settings[CurrentSettingSelected].CurrentlySelected = false;
        CurrentMenuState = CurrentSettingsMenuState.TabSelected;
        source.PlayOneShot(SettingSound_BackOut);
    }

    private void OpenSettingsTab(SettingsMenuTabs tab)
    {
        CurrentSelectedTab = tab;
        CurrentMenuState = CurrentSettingsMenuState.TabSelected;
        GonerMenu_SettingsSection[] settingSections = SettingSections;
        foreach (GonerMenu_SettingsSection gonerMenu_SettingsSection in settingSections)
        {
            if (gonerMenu_SettingsSection.ThisAssosciatedTab == CurrentSelectedTab)
            {
                CurrentSection = gonerMenu_SettingsSection;
            }
        }
        Soul.localPosition = new Vector2(-760f, 200f);
    }

    public void UpdateSettingFontsDyslexic()
    {
        bool boolSettingValue = SettingsManager.Instance.GetBoolSettingValue("DyslexicText");
        foreach (TextMeshProUGUI item in FontsAffectedByDyslexic)
        {
            if (boolSettingValue)
            {
                item.font = dyslexicFont;
            }
            else
            {
                item.font = defaultFont;
            }
        }
    }
}
