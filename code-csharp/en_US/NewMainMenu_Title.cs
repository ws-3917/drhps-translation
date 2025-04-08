using System.Collections;
using UnityEngine;

public class NewMainMenu_Title : MonoBehaviour
{
    [Header("- References -")]
    [SerializeField]
    private Animator CameraAnimator;

    [SerializeField]
    private Animator TitleAnimator;

    [SerializeField]
    private GameObject ConsoleMenu;

    private bool CanPressInput;

    private void Start()
    {
        StartCoroutine(IntroAnimation());
    }

    private void Update()
    {
        if (CanPressInput && Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
        {
            NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuSelect);
            TitleAnimator.Play("MainMenu_TitleWindow_Confirm");
            CameraAnimator.SetTrigger("TransitionToMonitor");
            StartCoroutine(SelectAnimation());
            CanPressInput = false;
        }
    }

    private IEnumerator IntroAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        CanPressInput = true;
    }

    private IEnumerator SelectAnimation()
    {
        yield return new WaitForSeconds(2.5f);
        base.gameObject.SetActive(value: false);
        ConsoleMenu.SetActive(value: true);
    }
}
