using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroDisclaimer : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro DisclaimerText;

    [SerializeField]
    private AudioClip DenySFX;

    [SerializeField]
    private AudioClip ConfirmSFX;

    [SerializeField]
    private AudioClip FinishConfirmSFX;

    private string originalDisclaimerText;

    private int remainingConfirmPresses = 3;

    private bool canBeginConfirming;

    private void Start()
    {
        originalDisclaimerText = DisclaimerText.text;
        DisclaimerText.text = originalDisclaimerText;
        if (PlayerPrefs.GetInt("Setting_DyslexicText", 0) == 1)
        {
            DisclaimerText.font = SettingsManager.Instance.DyslexicFont;
            DisclaimerText.extraPadding = false;
        }
        StartCoroutine(Disclaimer());
    }

    private void Update()
    {
        if (canBeginConfirming)
        {
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm) && remainingConfirmPresses > 0)
            {
                CutsceneUtils.PlaySound(ConfirmSFX);
                remainingConfirmPresses--;
                DisclaimerText.text = originalDisclaimerText + $"\nPress <color=yellow>{PlayerInput.Instance.Key_Confirm}</color>, <color=yellow>{remainingConfirmPresses}</color> more times to confirm.";
            }
            else
            {
                DisclaimerText.text = originalDisclaimerText + $"\nPress <color=yellow>{PlayerInput.Instance.Key_Confirm}</color>, <color=yellow>{remainingConfirmPresses}</color> more times to confirm.";
            }
        }
    }

    private IEnumerator Disclaimer()
    {
        yield return new WaitForSeconds(1f);
        CutsceneUtils.PlaySound(DenySFX);
        CutsceneUtils.FadeInText3D(DisclaimerText, 0.5f);
        yield return new WaitForSeconds(1.5f);
        canBeginConfirming = true;
        while (remainingConfirmPresses > 0)
        {
            yield return null;
        }
        CutsceneUtils.PlaySound(FinishConfirmSFX);
        CutsceneUtils.FadeOutText3D(DisclaimerText);
        yield return new WaitForSeconds(1f);
        if (PlayerPrefs.GetInt("DisclaimerViewed") == 0)
        {
            SceneManager.LoadScene(22);
        }
        else
        {
            SceneManager.LoadScene(2);
        }
    }
}
