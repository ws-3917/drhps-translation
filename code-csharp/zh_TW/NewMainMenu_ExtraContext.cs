using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewMainMenu_ExtraContext : MonoBehaviour
{
    public List<TextMeshProUGUI> BackgroundTextLines = new List<TextMeshProUGUI>();

    [SerializeField]
    private List<int> BackgroundTextMaxCount = new List<int>();

    [SerializeField]
    private NewMainMenu_ExtraSubMenu extrasSubMenu;

    [SerializeField]
    private TextMeshProUGUI optionText;

    private Coroutine currentMainTextWriting;

    private GameObject prefabStored;

    [SerializeField]
    private GameObject prefabHolder;

    private int CurrentSelected;

    private int previousSelected = -1;

    private bool simplesfx_on;

    public ExtraMenuContent CurrentExtraContent;

    [SerializeField]
    private AudioSource commentaryAudioSource;

    private bool hasCommentary;

    [SerializeField]
    private TextMeshProUGUI ExtrasTitle;

    [SerializeField]
    private TextMeshProUGUI ExtrasDescription;

    [SerializeField]
    private RawImage CommentaryButton;

    [SerializeField]
    private Sprite PlaySprite;

    [SerializeField]
    private Sprite PauseSprite;

    [SerializeField]
    private Sprite NoIconSprites;

    private bool AllTextShown;

    private bool wasEnabled;

    private bool AllowInput = true;

    private void Awake()
    {
        foreach (TextMeshProUGUI backgroundTextLine in BackgroundTextLines)
        {
            BackgroundTextMaxCount.Add(backgroundTextLine.text.Length);
            backgroundTextLine.maxVisibleCharacters = 0;
        }
    }

    private void Update()
    {
        if (commentaryAudioSource.clip != null && !simplesfx_on)
        {
            if (commentaryAudioSource.isPlaying)
            {
                CommentaryButton.texture = PauseSprite.texture;
                CameraManager.instance.ReverbFilter.enabled = false;
            }
            else
            {
                CommentaryButton.texture = PlaySprite.texture;
                CameraManager.instance.ReverbFilter.enabled = true;
            }
        }
        else
        {
            CameraManager.instance.ReverbFilter.enabled = true;
        }
        if (AllowInput)
        {
            if (hasCommentary)
            {
                if (Input.GetKeyDown(PlayerInput.Instance.Key_Right))
                {
                    CurrentSelected++;
                    CheckCurrentSelectedOutsideBounds();
                    CheckUpdateScreenText();
                }
                if (Input.GetKeyDown(PlayerInput.Instance.Key_Left))
                {
                    CurrentSelected--;
                    CheckCurrentSelectedOutsideBounds();
                    CheckUpdateScreenText();
                }
            }
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm) || Input.GetKeyDown(PlayerInput.Instance.Key_Menu))
            {
                SelectOption();
            }
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Cancel))
            {
                NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuDeny);
                extrasSubMenu.AllowInput = true;
                base.gameObject.SetActive(value: false);
            }
        }
        if (!wasEnabled && base.gameObject.activeSelf)
        {
            previousSelected = -1;
            CurrentSelected = 0;
            wasEnabled = true;
            AllowInput = true;
            StartCoroutine(ShowBackgroundText());
            CheckUpdateScreenText();
            SetupExtraContent();
        }
    }

    private void SelectOption()
    {
        AllowInput = false;
        previousSelected = -1;
        NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuSelect);
        StartCoroutine(SelectOptionTimer());
        CheckUpdateScreenText();
    }

    private IEnumerator SelectOptionTimer()
    {
        yield return new WaitForSeconds(0.5f);
        if (CurrentSelected == 0)
        {
            if (hasCommentary)
            {
                if (commentaryAudioSource.isPlaying)
                {
                    commentaryAudioSource.Stop();
                }
                else
                {
                    commentaryAudioSource.Play();
                }
                AllowInput = true;
                CheckUpdateScreenText();
            }
            else
            {
                NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuDeny);
                extrasSubMenu.AllowInput = true;
                base.gameObject.SetActive(value: false);
            }
        }
        else
        {
            NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuDeny);
            extrasSubMenu.AllowInput = true;
            base.gameObject.SetActive(value: false);
        }
    }

    private void CheckUpdateScreenText()
    {
        if (previousSelected != CurrentSelected)
        {
            previousSelected = CurrentSelected;
            string text = "";
            string[] array = (hasCommentary ? new string[2] { "聽一聽 ", "返回" } : new string[1] { "返回" });
            for (int i = 0; i < array.Length; i++)
            {
                text = ((CurrentSelected != i) ? (text + array[i]) : (AllowInput ? (text + "<color=yellow>-> " + array[i] + "</color>") : (text + "<color=green>-> " + array[i] + "</color>")));
            }
            ChangeMainText(text, AllowAnimation: true);
        }
    }

    private void CheckCurrentSelectedOutsideBounds()
    {
        if (hasCommentary)
        {
            if (CurrentSelected < 0)
            {
                CurrentSelected = 1;
            }
            if (CurrentSelected > 1)
            {
                CurrentSelected = 0;
            }
        }
        else
        {
            CurrentSelected = 0;
        }
        NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuMove);
    }

    private void SetupExtraContent()
    {
        if (CurrentExtraContent.Item != null)
        {
            ExtrasTitle.text = CurrentExtraContent.Item.ItemTitle;
            ExtrasDescription.text = CurrentExtraContent.Item.ItemDescription;
            simplesfx_on = SettingsManager.Instance.GetBoolSettingValue("SimpleSFX");
            if (CurrentExtraContent.Item.Prefab != null)
            {
                prefabStored = Object.Instantiate(CurrentExtraContent.Item.Prefab, prefabHolder.transform.position, Quaternion.identity);
                prefabStored.transform.SetParent(prefabHolder.transform, worldPositionStays: false);
            }
            if (CurrentExtraContent.Item.DeveloperCommentary != null)
            {
                commentaryAudioSource.clip = CurrentExtraContent.Item.DeveloperCommentary;
                CommentaryButton.texture = PlaySprite.texture;
                hasCommentary = true;
            }
            else
            {
                commentaryAudioSource.clip = null;
                CommentaryButton.texture = NoIconSprites.texture;
                hasCommentary = false;
            }
            previousSelected = -1;
            CheckUpdateScreenText();
        }
    }

    private void OnDisable()
    {
        wasEnabled = false;
        AllTextShown = false;
        ChangeMainText("", AllowAnimation: false);
        CameraManager.instance.ReverbFilter.enabled = true;
        if (prefabStored != null)
        {
            Object.Destroy(prefabStored);
        }
        foreach (TextMeshProUGUI backgroundTextLine in BackgroundTextLines)
        {
            backgroundTextLine.maxVisibleCharacters = 0;
        }
        currentMainTextWriting = null;
    }

    private IEnumerator ShowBackgroundText()
    {
        if (!SettingsManager.Instance.GetBoolSettingValue("SimpleVFX"))
        {
            List<TextMeshProUGUI> finishedTexts = new List<TextMeshProUGUI>();
            while (!AllTextShown)
            {
                yield return null;
                foreach (TextMeshProUGUI backgroundTextLine in BackgroundTextLines)
                {
                    if (!finishedTexts.Contains(backgroundTextLine) && backgroundTextLine.maxVisibleCharacters >= backgroundTextLine.text.Length)
                    {
                        finishedTexts.Add(backgroundTextLine);
                    }
                    else if (backgroundTextLine.maxVisibleCharacters < backgroundTextLine.text.Length)
                    {
                        float num = 100 + Random.Range(-60, -10);
                        backgroundTextLine.maxVisibleCharacters += Mathf.CeilToInt(num * Time.deltaTime);
                        backgroundTextLine.maxVisibleCharacters = Mathf.Clamp(backgroundTextLine.maxVisibleCharacters, 0, backgroundTextLine.text.Length);
                        NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuClick);
                    }
                }
                if (CompareTextLists(finishedTexts, BackgroundTextLines))
                {
                    AllTextShown = true;
                    MonoBehaviour.print("all text shown");
                }
            }
            yield break;
        }
        foreach (TextMeshProUGUI backgroundTextLine2 in BackgroundTextLines)
        {
            backgroundTextLine2.maxVisibleCharacters = backgroundTextLine2.text.Length;
        }
    }

    private bool CompareTextLists(List<TextMeshProUGUI> list1, List<TextMeshProUGUI> list2)
    {
        if (list1.Count != list2.Count)
        {
            return false;
        }
        for (int i = 0; i < list1.Count; i++)
        {
            if (list1[i].text != list2[i].text)
            {
                return false;
            }
        }
        return true;
    }

    public void ChangeMainText(string text, bool AllowAnimation)
    {
        optionText.text = text;
        if (!SettingsManager.Instance.GetBoolSettingValue("SimpleVFX") && AllowAnimation)
        {
            optionText.maxVisibleCharacters = 0;
            if (currentMainTextWriting != null)
            {
                StopCoroutine(currentMainTextWriting);
            }
            currentMainTextWriting = StartCoroutine(ScrollMainText());
        }
        else
        {
            optionText.maxVisibleCharacters = text.Length;
        }
    }

    private IEnumerator ScrollMainText()
    {
        yield return null;
        while (optionText.maxVisibleCharacters < optionText.text.Length)
        {
            float num = 600 + Random.Range(-120, 0);
            optionText.maxVisibleCharacters += Mathf.CeilToInt(num * Time.deltaTime);
            optionText.maxVisibleCharacters = Mathf.Clamp(optionText.maxVisibleCharacters, 0, optionText.text.Length);
            NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuClick);
            yield return null;
        }
    }
}
