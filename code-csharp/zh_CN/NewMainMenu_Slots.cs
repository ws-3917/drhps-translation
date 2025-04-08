using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewMainMenu_Slots : MonoBehaviour
{
    [Serializable]
    public class SlotSymbol
    {
        public Sprite sprite;

        public float odds;

        public float triplepayout;

        public AudioClip selectsound;

        public AudioClip triplesound;
    }

    [Header("-= Slot Settings =-")]
    public RectTransform[] columns;

    public int visibleRows = 3;

    public float spinDuration = 2f;

    public float spinSpeed = 500f;

    public float alignLerpSpeed = 5f;

    [SerializeField]
    private TextMeshProUGUI TextMoney;

    [SerializeField]
    private TextMeshProUGUI ControlsText;

    [Header("-= Slot Sound =-")]
    [SerializeField]
    private AudioClip SFX_SlotClick;

    [SerializeField]
    private AudioClip SFX_SpendMoney;

    [SerializeField]
    private AudioClip Mus_Gambling;

    [SerializeField]
    private AudioClip mus_woody;

    [Header("-= Symbols =-")]
    public List<SlotSymbol> slotSymbols = new List<SlotSymbol>();

    public List<Image[]> columnImages = new List<Image[]>();

    private float slotHeight;

    private int symbolsPerColumn;

    private bool currentlySpinning;

    [SerializeField]
    private GameObject ConsoleMenu;

    public List<Image> WinnerColumns = new List<Image>();

    private bool AllTextShown;

    private bool wasEnabled;

    public List<TextMeshProUGUI> BackgroundTextLines = new List<TextMeshProUGUI>();

    [SerializeField]
    private List<int> BackgroundTextMaxCount = new List<int>();

    [Header("-= Woody =-")]
    [SerializeField]
    private bool WoodyActive;

    [SerializeField]
    private GameObject WoodyGiygas;

    [SerializeField]
    private Image WoodyFade;

    [SerializeField]
    private Material WoodyMaterial;

    private int spinCounter;

    private void Start()
    {
        symbolsPerColumn = visibleRows + 2;
        slotHeight = columns[0].rect.height / (float)visibleRows;
        slotSymbols[5].odds = PlayerPrefs.GetInt("Gambling_WoodyChance", 100);
        RectTransform[] array = columns;
        foreach (RectTransform rectTransform in array)
        {
            Image[] array2 = new Image[symbolsPerColumn];
            for (int j = 0; j < symbolsPerColumn; j++)
            {
                GameObject gameObject = new GameObject($"Slot_{j}", typeof(Image));
                gameObject.transform.SetParent(rectTransform, worldPositionStays: false);
                array2[j] = gameObject.GetComponent<Image>();
                array2[j].rectTransform.sizeDelta = new Vector2(rectTransform.rect.width - 20f, slotHeight);
                float num = slotHeight * (float)(symbolsPerColumn - visibleRows) / 2f;
                array2[j].rectTransform.anchoredPosition = new Vector2(0f, num - (float)j * slotHeight);
                if (j == 1)
                {
                    WinnerColumns.Add(array2[j]);
                }
            }
            columnImages.Add(array2);
        }
        foreach (TextMeshProUGUI backgroundTextLine in BackgroundTextLines)
        {
            BackgroundTextMaxCount.Add(backgroundTextLine.text.Length);
            backgroundTextLine.maxVisibleCharacters = 0;
        }
        ResetColumns();
    }

    private void OnDisable()
    {
        wasEnabled = false;
        AllTextShown = false;
        foreach (TextMeshProUGUI backgroundTextLine in BackgroundTextLines)
        {
            backgroundTextLine.maxVisibleCharacters = 0;
        }
        currentlySpinning = false;
        StopCoroutine("SpinRoutine");
    }

    private void Update()
    {
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm) && !currentlySpinning && !WoodyActive)
        {
            if (SecurePlayerPrefs.GetSecureInt("TotalCash") >= 10)
            {
                Spin();
                NewMainMenuManager.instance.MenuSource.PlayOneShot(SFX_SpendMoney);
                SecurePlayerPrefs.SetSecureInt("TotalCash", SecurePlayerPrefs.GetSecureInt("TotalCash") - 10);
                TextMoney.text = string.Format("{0}$", SecurePlayerPrefs.GetSecureInt("TotalCash"));
            }
            else
            {
                NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuDeny);
            }
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Cancel) && !currentlySpinning && !WoodyActive)
        {
            NewMainMenuManager.instance.MenuSource.PlayOneShot(NewMainMenuManager.instance.SFX_MenuDeny);
            ConsoleMenu.SetActive(value: true);
            base.gameObject.SetActive(value: false);
            MusicManager.StopSong(Fade: false, 0f);
            CameraManager.instance.ReverbFilter.reverbPreset = AudioReverbPreset.Generic;
        }
        if (!wasEnabled && base.gameObject.activeSelf)
        {
            wasEnabled = true;
            TextMoney.text = string.Format("{0}$", SecurePlayerPrefs.GetSecureInt("TotalCash"));
            StartCoroutine(ShowBackgroundText());
            CameraManager.instance.ReverbFilter.reverbPreset = AudioReverbPreset.Off;
            MusicManager.PlaySong(Mus_Gambling, FadePreviousSong: false, 0f);
            ControlsText.text = $"按 {PlayerInput.Instance.Key_Confirm} 摇奖\n按 {PlayerInput.Instance.Key_Cancel} 返回";
        }
    }

    public void ResetColumns()
    {
        foreach (Image[] columnImage in columnImages)
        {
            for (int i = 0; i < columnImage.Length; i++)
            {
                columnImage[i].sprite = GetRandomSymbol(columnImage[i]);
                columnImage[i].rectTransform.anchoredPosition = new Vector2(0f, slotHeight * (float)(symbolsPerColumn - visibleRows) / 2f - (float)i * slotHeight);
            }
        }
    }

    public void Spin()
    {
        currentlySpinning = true;
        spinCounter++;
        if (spinCounter % 3 == 0)
        {
            ResetColumns();
        }
        StartCoroutine(SpinRoutine());
        DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.CheekyGrin);
    }

    private IEnumerator SpinRoutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < spinDuration)
        {
            float num = Mathf.Clamp01(elapsedTime / (spinDuration / 4f));
            foreach (Image[] columnImage in columnImages)
            {
                for (int i = 0; i < columnImage.Length; i++)
                {
                    RectTransform rectTransform = columnImage[i].rectTransform;
                    rectTransform.anchoredPosition += new Vector2(0f, (0f - spinSpeed) * num * Time.deltaTime);
                    if (rectTransform.anchoredPosition.y <= (0f - slotHeight) * (float)(symbolsPerColumn - 1))
                    {
                        rectTransform.anchoredPosition += new Vector2(0f, slotHeight * (float)symbolsPerColumn + slotHeight);
                        columnImage[i].sprite = GetRandomSymbol(columnImage[i], enableForgiveness: true);
                        CutsceneUtils.PlaySound(SFX_SlotClick, CutsceneUtils.DRH_MixerChannels.Effect, 0.5f, UnityEngine.Random.Range(0.9f, 1.1f));
                    }
                }
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (PlayerPrefs.GetInt("Gambling_WoodyChance", 100) > 0)
        {
            PlayerPrefs.SetInt("Gambling_WoodyChance", PlayerPrefs.GetInt("Gambling_WoodyChance", 100) + 25);
            slotSymbols[5].odds = PlayerPrefs.GetInt("Gambling_WoodyChance", 100);
        }
        AlignColumns();
    }

    private Sprite GetRandomSymbol(Image img, bool enableForgiveness = false)
    {
        float num = 0f;
        foreach (SlotSymbol slotSymbol in slotSymbols)
        {
            num += slotSymbol.odds;
        }
        float num2 = UnityEngine.Random.Range(0f, num);
        float num3 = 0f;
        if (enableForgiveness && (double)UnityEngine.Random.value < 0.3 && img == WinnerColumns[2] && WinnerColumns[0].sprite == WinnerColumns[1].sprite)
        {
            return WinnerColumns[0].sprite;
        }
        foreach (SlotSymbol slotSymbol2 in slotSymbols)
        {
            num3 += slotSymbol2.odds;
            if (num2 <= num3)
            {
                return slotSymbol2.sprite;
            }
        }
        return slotSymbols[0].sprite;
    }

    private void AlignColumns()
    {
        StartCoroutine(AlignRoutine());
    }

    private IEnumerator AlignRoutine()
    {
        bool aligning = true;
        while (aligning)
        {
            aligning = false;
            for (int columnIndex = 0; columnIndex < columnImages.Count; columnIndex++)
            {
                Image[] array = columnImages[columnIndex];
                for (int i = 0; i < array.Length; i++)
                {
                    RectTransform rectTransform = array[i].rectTransform;
                    float num = Mathf.Round(rectTransform.anchoredPosition.y / slotHeight) * slotHeight;
                    float y = Mathf.Lerp(rectTransform.anchoredPosition.y, num, Time.deltaTime * alignLerpSpeed);
                    rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, y);
                    if (Mathf.Abs(rectTransform.anchoredPosition.y - num) > 0.1f)
                    {
                        aligning = true;
                    }
                }
                yield return null;
            }
        }
        bool playWinSound = false;
        AudioClip winSound = null;
        for (int columnIndex = 0; columnIndex < columnImages.Count; columnIndex++)
        {
            Image[] array2 = columnImages[columnIndex];
            int num2 = visibleRows / 2;
            Image image = array2[num2];
            SlotSymbol symbolFromImage = GetSymbolFromImage(image.sprite);
            if (symbolFromImage != null)
            {
                MonoBehaviour.print($"Column {columnIndex + 1}: {symbolFromImage.sprite.name}");
                CutsceneUtils.PlaySound(symbolFromImage.selectsound, CutsceneUtils.DRH_MixerChannels.Effect, 0.8f);
                if (columnImages[0][visibleRows / 2].sprite == columnImages[1][visibleRows / 2].sprite && columnImages[1][visibleRows / 2].sprite == columnImages[2][visibleRows / 2].sprite)
                {
                    Sprite sprite = columnImages[0][visibleRows / 2].sprite;
                    SlotSymbol symbolFromImage2 = GetSymbolFromImage(sprite);
                    if (symbolFromImage2 != null)
                    {
                        playWinSound = true;
                        winSound = symbolFromImage2.triplesound;
                        float triplepayout = symbolFromImage2.triplepayout;
                        SecurePlayerPrefs.SetSecureInt("TotalCash", SecurePlayerPrefs.GetSecureInt("TotalCash") + Mathf.RoundToInt(triplepayout), disableTotalCashDisplay: true);
                        TextMoney.text = string.Format("{0}$", SecurePlayerPrefs.GetSecureInt("TotalCash"));
                        DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Shock_NoSound);
                        MonoBehaviour.print($"Payout awarded: {triplepayout}$");
                        if (symbolFromImage2 == slotSymbols[5])
                        {
                            StartCoroutine(WoodyScene());
                            PlayerPrefs.SetInt("Gambling_WoodyChance", 0);
                            slotSymbols[5].odds = 0f;
                            NewMainMenu_Trophys.instance.UpdateTrophys();
                            PlayerPrefs.SetInt("Trophy_Woody", 1);
                        }
                    }
                }
                else
                {
                    DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Annoyed);
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
        if (playWinSound && winSound != null)
        {
            MusicManager.PauseMusic();
            CutsceneUtils.PlaySound(winSound);
            yield return new WaitForSeconds(winSound.length);
            DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Crying);
            MusicManager.ResumeMusic();
        }
        currentlySpinning = false;
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
                        float num = 100 + UnityEngine.Random.Range(-60, -10);
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

    private SlotSymbol GetSymbolFromImage(Sprite sprite)
    {
        foreach (SlotSymbol slotSymbol in slotSymbols)
        {
            if (slotSymbol.sprite == sprite)
            {
                return slotSymbol;
            }
        }
        return null;
    }

    private IEnumerator WoodyScene()
    {
        WoodyActive = true;
        WoodyFade.enabled = true;
        WoodyGiygas.SetActive(value: true);
        MusicManager.PlaySong(mus_woody, FadePreviousSong: false, 0f);
        float duration = 90f;
        float elapsedTime = 0f;
        Color fadeColor = WoodyFade.color;
        while (elapsedTime < duration)
        {
            fadeColor.a = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            WoodyFade.color = fadeColor;
            float num = ((!(elapsedTime <= 53f)) ? 8.5f : 1.25f);
            float num2 = Mathf.PerlinNoise(Time.time * 0.5f, 0f) * num;
            float num3 = Mathf.PerlinNoise(0f, Time.time * 0.5f) * num;
            WoodyMaterial.SetFloat("_TimeScalingX", 32f + elapsedTime * 2f + num2);
            WoodyMaterial.SetFloat("_TimeScalingY", 32f + elapsedTime * 2f + num3);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        ResetColumns();
        WoodyMaterial.SetFloat("_TimeScalingX", 32f);
        WoodyMaterial.SetFloat("_TimeScalingY", 32f);
        fadeColor.a = 0f;
        WoodyFade.color = fadeColor;
        yield return new WaitForSeconds(2f);
        WoodyGiygas.SetActive(value: false);
        WoodyFade.enabled = false;
        WoodyActive = false;
        MusicManager.PlaySong(Mus_Gambling, FadePreviousSong: false, 0f);
    }
}
