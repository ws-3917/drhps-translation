using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BOOTUP_Manager : MonoBehaviour
{
    [SerializeField]
    private int MessengerCutsceneIndex;

    [SerializeField]
    private float CutsceneMultiplier = 1f;

    [SerializeField]
    private bool HasViewedDisclaimer;

    [Space(10f)]
    [Header("-= References =-")]
    [SerializeField]
    private Transform Camera;

    [SerializeField]
    private float CameraLerpSpeed = 5f;

    [SerializeField]
    private TRIG_LEVELTRANSITION LevelTransition;

    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioClip snd_jingle;

    [SerializeField]
    private AudioClip[] snd_typesounds;

    [SerializeField]
    private AudioClip snd_startup;

    [SerializeField]
    private AudioClip snd_notification;

    [SerializeField]
    private AudioClip snd_chatshow;

    [Header("Computer Screen")]
    [SerializeField]
    private SpriteRenderer ScreenButtonLight;

    [SerializeField]
    private GameObject ScreenWhite;

    [SerializeField]
    private GameObject HourGlass;

    [Header("BIOS loading Text")]
    [SerializeField]
    private TextMeshProUGUI BIOSTextMesh;

    [TextArea]
    [SerializeField]
    private string BIOSText;

    [SerializeField]
    private float BIOSTextCharPerFrame = 2f;

    [Header("Logo Glitched")]
    [SerializeField]
    private Image LogoGlitched;

    [Header("Messenger")]
    [SerializeField]
    private GameObject Messenger;

    [SerializeField]
    private TMP_InputField MessengerInputField;

    [SerializeField]
    private TextMeshProUGUI MessageWindow;

    [SerializeField]
    private bool ForceFocusOnText;

    [SerializeField]
    private string previousAnswer;

    [SerializeField]
    private List<string> CurrentHostMessageQueue = new List<string>();

    [SerializeField]
    private List<bool> CurrentHostMessageFinale = new List<bool>();

    [SerializeField]
    private List<float> CurrentHostMessageLengths = new List<float>();

    private void Start()
    {
        if (PlayerPrefs.GetInt("NewDisclaimerViewed", 0) == 1)
        {
            CutsceneMultiplier = 3f;
            HasViewedDisclaimer = true;
        }
        else
        {
            PlayerPrefs.SetInt("NewDisclaimerViewed", 1);
            HasViewedDisclaimer = false;
            CutsceneMultiplier = 1f;
        }
        Camera.position = new Vector3(0f, 0f, -14f);
        StartCoroutine(BOOTUP_Cutscene());
    }

    private IEnumerator BOOTUP_Cutscene()
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(1f / CutsceneMultiplier);
        ScreenButtonLight.color = new Color32(byte.MaxValue, 108, 0, byte.MaxValue);
        source.PlayOneShot(snd_startup, 0.5f);
        source.PlayDelayed(0.5f);
        ScreenButtonLight.enabled = true;
        yield return new WaitForSeconds(1f / CutsceneMultiplier);
        ScreenWhite.SetActive(value: true);
        yield return new WaitForSeconds(1f / CutsceneMultiplier);
        ScreenWhite.SetActive(value: false);
        yield return new WaitForSeconds(0.35f / CutsceneMultiplier);
        ScreenButtonLight.color = new Color32(0, byte.MaxValue, 0, byte.MaxValue);
        yield return new WaitForSeconds(1.5f / CutsceneMultiplier);
        int TargetVisibleCharacterCount = BIOSText.Length;
        BIOSTextMesh.maxVisibleCharacters = 0;
        BIOSTextMesh.text = BIOSText;
        StartCoroutine(BIOSSpeedRandomizer());
        while (BIOSTextMesh.maxVisibleCharacters < TargetVisibleCharacterCount)
        {
            yield return new WaitForSeconds(0f);
            BIOSTextMesh.maxVisibleCharacters += (int)(BIOSTextCharPerFrame * CutsceneMultiplier);
            if (BIOSTextMesh.maxVisibleCharacters < BIOSTextMesh.text.Length && BIOSTextMesh.text.ToCharArray()[BIOSTextMesh.maxVisibleCharacters] == '|')
            {
                yield return new WaitForSeconds(0.15f);
            }
        }
        yield return new WaitForSeconds(0.25f / CutsceneMultiplier);
        BIOSTextMesh.enabled = false;
        yield return new WaitForSeconds(0.5f / CutsceneMultiplier);
        HourGlass.SetActive(value: true);
        yield return new WaitForSeconds(0.5f / CutsceneMultiplier);
        HourGlass.SetActive(value: false);
        yield return new WaitForSeconds(0.25f / CutsceneMultiplier);
        LogoGlitched.fillAmount = 0.127f;
        yield return new WaitForSeconds(0.1f / CutsceneMultiplier);
        LogoGlitched.fillAmount = 0.256f;
        yield return new WaitForSeconds(0.32f / CutsceneMultiplier);
        LogoGlitched.fillAmount = 0.3f;
        yield return new WaitForSeconds(0.25f / CutsceneMultiplier);
        LogoGlitched.fillAmount = 0.73f;
        yield return new WaitForSeconds(0.5f / CutsceneMultiplier);
        LogoGlitched.fillAmount = 1f;
        yield return new WaitForSeconds(0.1f / CutsceneMultiplier);
        source.PlayOneShot(snd_jingle);
        yield return new WaitForSeconds(3f);
        LogoGlitched.enabled = false;
        yield return new WaitForSeconds(0.1f);
        if (HasViewedDisclaimer)
        {
            Vector3 TargetPos2 = new Vector3(0f, 0f, -8f);
            while ((double)Vector3.Distance(Camera.position, TargetPos2) > 0.1)
            {
                Camera.position = Vector3.Lerp(Camera.position, TargetPos2, CameraLerpSpeed * Time.deltaTime);
                yield return new WaitForSeconds(0f / CutsceneMultiplier);
            }
            LevelTransition.BeginTransition();
        }
        else
        {
            StartCoroutine(DisclaimerCutscene());
        }
    }

    private IEnumerator DisclaimerCutscene()
    {
        yield return new WaitForSeconds(1f);
        Messenger.SetActive(value: true);
        source.PlayOneShot(snd_chatshow, 0.5f);
        ForceFocusOnText = false;
        yield return new WaitForSeconds(1.5f);
        AddNewHostMessage("我們連上網絡了嗎？", EnableOnEnd: true);
        StartCoroutine(PostHostLoop());
    }

    public void PlayTypeSFX()
    {
        source.PlayOneShot(snd_typesounds[Random.Range(0, snd_typesounds.Length)], 0.25f);
    }

    private void RunMessengerCutscene()
    {
        switch (MessengerCutsceneIndex)
        {
            case 1:
                if (PreviousMessageHadValidAnswer())
                {
                    AddNewHostMessage("太棒了。", EnableOnEnd: false);
                    AddNewHostMessage("我們可以開始了。", EnableOnEnd: false);
                    AddNewHostMessage("", EnableOnEnd: false, 0.25f);
                    AddNewHostMessage("首先，這裡的一切均為<b>非官方內容<b>，純屬虛構。\n你是否知曉？", EnableOnEnd: true);
                }
                else
                {
                    AddNewHostMessage("怪了...", EnableOnEnd: false);
                    AddNewHostMessage("請不要做出不合邏輯的回應。", EnableOnEnd: false);
                    AddNewHostMessage("不管怎樣，我們可以開始了。", EnableOnEnd: false);
                    AddNewHostMessage("", EnableOnEnd: false, 0.25f);
                    AddNewHostMessage("首先，這裡的一切均為<b>非官方內容<b>，純屬虛構。\n你是否知曉？", EnableOnEnd: true);
                }
                break;
            case 2:
                if (PreviousMessageHadValidAnswer())
                {
                    AddNewHostMessage("明白。", EnableOnEnd: false);
                    AddNewHostMessage("我們繼續。", EnableOnEnd: false);
                    AddNewHostMessage("", EnableOnEnd: false, 0.25f);
                    AddNewHostMessage("你能否接受異常現象和可能存在的\n<b>奇怪之處<b>？", EnableOnEnd: true);
                }
                else
                {
                    AddNewHostMessage("那問題就大了。", EnableOnEnd: false);
                    AddNewHostMessage("記住，犬科動物是沒法在這個宇宙中\n生存下去的", EnableOnEnd: false, 3.5f);
                    AddNewHostMessage("這些實驗皆非官方所作。", EnableOnEnd: false);
                    AddNewHostMessage("我們繼續。", EnableOnEnd: false);
                    AddNewHostMessage("", EnableOnEnd: false, 0.25f);
                    AddNewHostMessage("你能否接受異常現象和可能存在的\n<b>奇怪之處<b>？", EnableOnEnd: true);
                }
                break;
            case 3:
                if (PreviousMessageHadValidAnswer())
                {
                    AddNewHostMessage("那我就放心了。", EnableOnEnd: false);
                    AddNewHostMessage("如果出現任何異常狀況...", EnableOnEnd: false);
                    AddNewHostMessage("請隨時聯繫... 製造商。", EnableOnEnd: false);
                    AddNewHostMessage("現在，我們來看最後一項。", EnableOnEnd: false);
                    AddNewHostMessage("", EnableOnEnd: false, 0.25f);
                    AddNewHostMessage("你是否有癲癇等症狀？", EnableOnEnd: true);
                }
                else
                {
                    AddNewHostMessage("有趣。", EnableOnEnd: false);
                    AddNewHostMessage("非常有趣。", EnableOnEnd: false);
                    AddNewHostMessage("如果你確實遇到異常狀況，請不要驚慌。", EnableOnEnd: false, 3.5f);
                    AddNewHostMessage("聯繫製造商，把你的問題反映給他們。", EnableOnEnd: false, 3.5f);
                    AddNewHostMessage("現在，我們來看最後一項。", EnableOnEnd: false);
                    AddNewHostMessage("", EnableOnEnd: false, 0.25f);
                    AddNewHostMessage("你是否有癲癇等症狀？", EnableOnEnd: true);
                }
                break;
            case 4:
                AddNewHostMessage("如果對頻閃感到不適，請在設置中啟用「簡化視覺特效」。", EnableOnEnd: false);
                AddNewHostMessage("記住，這很有用的。", EnableOnEnd: false, 3.5f);
                AddNewHostMessage("...", EnableOnEnd: false);
                AddNewHostMessage("至此，我們已經完成了所有確認步驟。", EnableOnEnd: false, 3.5f);
                AddNewHostMessage("顯然，你已準備好開始進行三角符文模擬實驗。", EnableOnEnd: false, 4f);
                AddNewHostMessage("我非常期待你的表現。", EnableOnEnd: false, 5f);
                AddNewHostMessage("只要我們共同努力，我們的夢想就一定能夠實現。", EnableOnEnd: false, 4f);
                AddNewHostMessage("...", EnableOnEnd: false);
                AddNewHostMessage("我不久後會再次聯繫你。", EnableOnEnd: false, 3.5f);
                AddNewHostMessage("十分感謝你能百忙之中抽出時間回答我。", EnableOnEnd: false);
                AddNewHostMessage("再見了，HYPOASTER博士。", EnableOnEnd: false, 3f);
                AddNewHostMessage("\"", EnableOnEnd: false, 1f);
                break;
            case 0:
                break;
        }
    }

    private bool PreviousMessageHadValidAnswer()
    {
        char[] array = previousAnswer.ToUpper().ToCharArray();
        string input = previousAnswer.ToUpper();
        if (array.Length == 0)
        {
            return false;
        }
        string pattern = "(YES|YEP|YEA(?:H)?|YUP|OKAY|ACKNOWLEDGED|UNDERSTAND|AGREE2ALL|MHM|OK|SI|SÍ|是(?:的|滴)?|對|好(?:的|滴)?|可以|嗯|當然|沒問題|能接受|接受|可以|能|當然|確認|收到|了解|明白|曉得|認可|贊同|同意|支持|能|理解|清楚|懂|可|行|成|沒毛病|沒錯|連(?:接)?上了|已閱|知道|知曉)";
        string pattern2 = "(否|不是(?:的|滴)?|不對|不可以|不贊同|沒有|沒|不同意|不接受|不能接受|不可以|不行|不能|不理解|不懂|沒連上|未連接|不清楚|不了解|不支持|不認可|沒明白|不明白|不知道|不知曉)";
        if (!Regex.IsMatch(input, pattern2, RegexOptions.IgnoreCase))
        {
            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }
        return false;
    }

    private void Update()
    {
        if (ForceFocusOnText)
        {
            EventSystem.current.SetSelectedGameObject(MessengerInputField.gameObject, null);
            MessengerInputField.gameObject.SetActive(value: true);
            MessengerInputField.OnPointerClick(new PointerEventData(EventSystem.current));
            if (Input.GetKeyDown(KeyCode.Return) && MessengerInputField.text.Length > 0)
            {
                MessengerCutsceneIndex++;
                AddNewClientMessage(MessengerInputField.text);
                previousAnswer = MessengerInputField.text;
                StartCoroutine(RunMessageCutsceneDelay());
                MessengerInputField.gameObject.SetActive(value: false);
                MessengerInputField.text = "";
                ForceFocusOnText = false;
            }
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null, null);
            MessengerInputField.gameObject.SetActive(value: false);
            MessengerInputField.text = "";
        }
    }

    private IEnumerator RunMessageCutsceneDelay()
    {
        yield return new WaitForSeconds(1.5f);
        RunMessengerCutscene();
    }

    private IEnumerator PostHostLoop()
    {
        yield return new WaitForSeconds(0.1f);
        if (CurrentHostMessageQueue.Count > 0)
        {
            yield return new WaitForSeconds(CurrentHostMessageLengths[0]);
            if ((double)CurrentHostMessageLengths[0] >= 0.25)
            {
                PlayHostMessage(CurrentHostMessageQueue[0]);
            }
            else
            {
                PlayHostMessage(CurrentHostMessageQueue[0], PlaySound: false);
            }
            if (CurrentHostMessageQueue[0] == "\"")
            {
                StartCoroutine(DelayUntilLevelTransition());
            }
            CurrentHostMessageQueue.RemoveAt(0);
            CurrentHostMessageLengths.RemoveAt(0);
            if (CurrentHostMessageFinale[0])
            {
                ForceFocusOnText = true;
            }
            CurrentHostMessageFinale.RemoveAt(0);
        }
        StartCoroutine(PostHostLoop());
    }

    private void PlayHostMessage(string message, bool PlaySound = true)
    {
        if (message != "")
        {
            if (message != "\"")
            {
                if (PlaySound)
                {
                    source.PlayOneShot(snd_notification, 0.5f);
                }
                TextMeshProUGUI messageWindow = MessageWindow;
                messageWindow.text = messageWindow.text + "> " + message + "\n\n";
            }
        }
        else
        {
            TextMeshProUGUI messageWindow2 = MessageWindow;
            messageWindow2.text = messageWindow2.text + message + "\n\n";
        }
    }

    private void AddNewHostMessage(string message, bool EnableOnEnd, float Length = 2.5f)
    {
        CurrentHostMessageQueue.Add(message.ToUpper() ?? "");
        CurrentHostMessageFinale.Add(EnableOnEnd);
        CurrentHostMessageLengths.Add(Length);
    }

    private IEnumerator DelayUntilLevelTransition()
    {
        yield return new WaitForSeconds(1f);
        LevelTransition.BeginTransition(0.35f);
    }

    private void AddNewClientMessage(string message)
    {
        TextMeshProUGUI messageWindow = MessageWindow;
        messageWindow.text = messageWindow.text + "< " + message + "\n\n";
    }

    private IEnumerator BIOSSpeedRandomizer()
    {
        if (BIOSTextMesh.maxVisibleCharacters < BIOSText.Length)
        {
            yield return new WaitForSeconds(Random.Range(0.25f, 2f) / CutsceneMultiplier);
            BIOSTextCharPerFrame = Random.Range(3, 6);
            StartCoroutine(BIOSSpeedRandomizer());
        }
    }
}
