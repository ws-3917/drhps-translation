using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_FADE : MonoBehaviour
{
    public Image FadeImage;

    private static UI_FADE instance;

    private Coroutine currentFadeCoroutine;

    public bool isFading;

    public static bool DarkworldMenu_PreviousState;

    public static bool LightworldMenu_PreviousState;

    public static UI_FADE Instance => instance;

    private void Awake()
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

    private void Start()
    {
        FadeImage.color = new Color(0f, 0f, 0f, 1f);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void StartFadeIn(int GoToScene, float SpeedMultiplier, bool UnpauseOnEnd = false, NewMainMenuManager.MainMenuStates targetmenuState = NewMainMenuManager.MainMenuStates.None)
    {
        if (isFading)
        {
            StopCoroutine(currentFadeCoroutine);
        }
        DarkworldMenu_PreviousState = DarkworldMenu.Instance.CanOpenMenu;
        LightworldMenu_PreviousState = LightworldMenu.Instance.CanOpenMenu;
        DarkworldMenu.Instance.CanOpenMenu = false;
        LightworldMenu.Instance.CanOpenMenu = false;
        StopCoroutine(DelayRevertCMenus());
        currentFadeCoroutine = StartCoroutine(FadeIn(GoToScene, SpeedMultiplier, targetmenuState, UnpauseOnEnd));
    }

    public void StartFadeOut(float SpeedMultiplier = 1f)
    {
        if (isFading)
        {
            StopCoroutine(currentFadeCoroutine);
        }
        currentFadeCoroutine = StartCoroutine(FadeOut(SpeedMultiplier));
    }

    private IEnumerator FadeIn(int GoToScene, float SpeedMultiplier, NewMainMenuManager.MainMenuStates targetmenuState, bool UnpauseOnEnd = false)
    {
        isFading = true;
        ChatboxManager.Instance.EndText();
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
        while (FadeImage.color.a < 1f)
        {
            FadeImage.color = new Color(0f, 0f, 0f, FadeImage.color.a + 0.15f * SpeedMultiplier);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        yield return new WaitForSecondsRealtime(0.1f);
        if (GoToScene > 0)
        {
            SceneManager.LoadScene(GoToScene);
        }
        ChatboxManager.Instance.EndText();
        if (UnpauseOnEnd)
        {
            GonerMenu.Instance.UnpauseGame();
            GonerMenu.Instance.ResetPlayer();
        }
        isFading = false;
        if (targetmenuState != 0)
        {
            yield return null;
            while (NewMainMenuManager.instance == null)
            {
                yield return null;
            }
            NewMainMenuManager.instance.SetMainMenuState(targetmenuState);
        }
    }

    private IEnumerator FadeOut(float SpeedMultiplier = 1f)
    {
        isFading = true;
        FadeImage.color = new Color(0f, 0f, 0f, 1f);
        while (FadeImage.color.a > 0f)
        {
            FadeImage.color = new Color(0f, 0f, 0f, FadeImage.color.a - 0.15f * SpeedMultiplier);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        isFading = false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Reverting CMenus to original states");
        StartCoroutine(DelayRevertCMenus());
    }

    private IEnumerator DelayRevertCMenus()
    {
        yield return new WaitForSeconds(0.25f);
        DarkworldMenu.Instance.CanOpenMenu = DarkworldMenu_PreviousState;
        LightworldMenu.Instance.CanOpenMenu = LightworldMenu_PreviousState;
    }
}
