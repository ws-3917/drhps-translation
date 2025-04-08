using UnityEngine;

public class NewMainMenuManager : MonoBehaviour
{
    public enum MainMenuStates
    {
        None = 0,
        Title = 1,
        Console = 2,
        Hypothetical = 3,
        Logs = 4,
        Credits = 5,
        Extras = 6
    }

    public static NewMainMenuManager instance;

    [SerializeField]
    private GameObject FogCylinder;

    [SerializeField]
    private Light globalLight;

    public Animator CameraAnimator;

    public AudioSource MenuSource;

    public AudioClip SFX_MenuSelect;

    public AudioClip SFX_MenuMove;

    public AudioClip SFX_MenuDeny;

    public AudioClip SFX_MenuClick;

    public AudioClip SFX_ComputerBootOff;

    [SerializeField]
    private GameObject TitleMenu;

    [SerializeField]
    private GameObject ConsoleMenu;

    [SerializeField]
    private GameObject HypotheticalMenu;

    [SerializeField]
    private GameObject LogMenu;

    [SerializeField]
    private GameObject CreditMenu;

    [SerializeField]
    private GameObject ExtraMenu;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (SettingsManager.Instance.GetBoolSettingValue("SimpleVFX"))
        {
            Object.Destroy(FogCylinder);
            globalLight.shadows = LightShadows.None;
        }
        UI_LoadingIcon.ToggleLoadingIcon(showIcon: false);
        GonerMenu.CurrentActiveHypothesis = null;
        GonerMenu.RecoveringFromGameOver = false;
        LightworldMenu.Instance.CanOpenMenu = false;
        DarkworldMenu.Instance.CanOpenMenu = false;
        PartyMemberSystem.Instance.RemoveAllPartyMember();
        PlayerPrefs.SetInt("TimesPlayed", PlayerPrefs.GetInt("TimesPlayed", 0) + 1);
    }

    private void Update()
    {
        LightworldMenu.Instance.CanOpenMenu = false;
        DarkworldMenu.Instance.CanOpenMenu = false;
    }

    public void SetMainMenuState(MainMenuStates state)
    {
        switch (state)
        {
            case MainMenuStates.Console:
                TitleMenu.SetActive(value: false);
                ConsoleMenu.SetActive(value: true);
                CameraAnimator.Play("MainMenu_Camera_MonitorIdle");
                break;
            case MainMenuStates.Hypothetical:
                TitleMenu.SetActive(value: false);
                HypotheticalMenu.SetActive(value: true);
                CameraAnimator.Play("MainMenu_Camera_MonitorZoomIn", 0, 1f);
                break;
            case MainMenuStates.Logs:
                TitleMenu.SetActive(value: false);
                HypotheticalMenu.SetActive(value: true);
                CameraAnimator.Play("MainMenu_Camera_MonitorZoomIn", 0, 1f);
                break;
            case MainMenuStates.Credits:
                TitleMenu.SetActive(value: false);
                HypotheticalMenu.SetActive(value: true);
                CameraAnimator.Play("MainMenu_Camera_MonitorZoomIn", 0, 1f);
                break;
            case MainMenuStates.Extras:
                TitleMenu.SetActive(value: false);
                HypotheticalMenu.SetActive(value: true);
                CameraAnimator.Play("MainMenu_Camera_MonitorZoomIn", 0, 1f);
                break;
            case MainMenuStates.Title:
                break;
        }
    }
}
