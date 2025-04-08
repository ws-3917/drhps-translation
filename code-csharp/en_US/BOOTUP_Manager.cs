using System.Collections;
using System.Collections.Generic;
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
        AddNewHostMessage("ARE WE CONNECTED TO THE NETWORK?", EnableOnEnd: true);
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
                    AddNewHostMessage("EXCELLENT.", EnableOnEnd: false);
                    AddNewHostMessage("WE MAY BEGIN.", EnableOnEnd: false);
                    AddNewHostMessage("", EnableOnEnd: false, 0.25f);
                    AddNewHostMessage("WERE YOU AWARE THIS IS ALL FICTITIOUS AND <b>UNOFFICIAL<b>?", EnableOnEnd: true);
                }
                else
                {
                    AddNewHostMessage("HOW PECULIAR..", EnableOnEnd: false);
                    AddNewHostMessage("PLEASE REFRAIN FROM ILLOGICAL RESPONSES.", EnableOnEnd: false);
                    AddNewHostMessage("ANYWHO, WE MAY BEGIN.", EnableOnEnd: false);
                    AddNewHostMessage("", EnableOnEnd: false, 0.25f);
                    AddNewHostMessage("WERE YOU AWARE THIS IS ALL FICTITIOUS AND <b>UNOFFICIAL<b>?", EnableOnEnd: true);
                }
                break;
            case 2:
                if (PreviousMessageHadValidAnswer())
                {
                    AddNewHostMessage("UNDERSTOOD.", EnableOnEnd: false);
                    AddNewHostMessage("MOVING ON.", EnableOnEnd: false);
                    AddNewHostMessage("", EnableOnEnd: false, 0.25f);
                    AddNewHostMessage("ARE YOU CONTENT WITH IRREGULARITIES AND POSSIBLE <b>INACCURACIES<b>?", EnableOnEnd: true);
                }
                else
                {
                    AddNewHostMessage("THIS WILL BE AN ISSUE.", EnableOnEnd: false);
                    AddNewHostMessage("THIS COSMOS DOES NOT HAVE RELATION TO ANY CANINES.", EnableOnEnd: false, 3.5f);
                    AddNewHostMessage("THESE EXPERIMENTS ARE NOT OFFICIAL.", EnableOnEnd: false);
                    AddNewHostMessage("MOVING ON.", EnableOnEnd: false);
                    AddNewHostMessage("", EnableOnEnd: false, 0.25f);
                    AddNewHostMessage("ARE YOU CONTENT WITH IRREGULARITIES AND POSSIBLE <b>INACCURACIES<b>?", EnableOnEnd: true);
                }
                break;
            case 3:
                if (PreviousMessageHadValidAnswer())
                {
                    AddNewHostMessage("THIS IS A RELIEF.", EnableOnEnd: false);
                    AddNewHostMessage("FEEL FREE TO CONTACT.. THE MANUFACTURER.", EnableOnEnd: false);
                    AddNewHostMessage("IF ANY INACCURATE INFORMATION CROPS UP.", EnableOnEnd: false);
                    AddNewHostMessage("NOW WE MOVE ONTO THE FINALE.", EnableOnEnd: false);
                    AddNewHostMessage("", EnableOnEnd: false, 0.25f);
                    AddNewHostMessage("ARE YOU PRONE TO PAIN AND OR SEIZURE?", EnableOnEnd: true);
                }
                else
                {
                    AddNewHostMessage("INTERESTING.", EnableOnEnd: false);
                    AddNewHostMessage("VERY INTERESTING.", EnableOnEnd: false);
                    AddNewHostMessage("IF YOU TAKE TRUE ISSUE OF IRREGULARITIES, DO NOT PANIC.", EnableOnEnd: false, 3.5f);
                    AddNewHostMessage("CONTACT THE MANUFACTURER AND INFORM THEM OF YOUR ISSUE.", EnableOnEnd: false, 3.5f);
                    AddNewHostMessage("NOW WE MOVE ONTO THE FINALE.", EnableOnEnd: false);
                    AddNewHostMessage("", EnableOnEnd: false, 0.25f);
                    AddNewHostMessage("ARE YOU PRONE TO PAIN AND OR SEIZURE?", EnableOnEnd: true);
                }
                break;
            case 4:
                AddNewHostMessage("HEED THIS USEFUL TIP.", EnableOnEnd: false);
                AddNewHostMessage("ENABLE \"SIMPLE VFX\" IN SETTINGS IF FLASHING LIGHTS ARE A CONCERN. ", EnableOnEnd: false, 3.5f);
                AddNewHostMessage("...", EnableOnEnd: false);
                AddNewHostMessage("AND WITH THAT, WE HAVE REACHED THE END OF THE CURRENT TRANSMISSION.", EnableOnEnd: false, 3.5f);
                AddNewHostMessage("YOU ARE READY TO BEGIN EXPERIMENTING WITH THE DELTARUNE SIMULATION.", EnableOnEnd: false, 4f);
                AddNewHostMessage("I'M VERY EXCITED TO SEE WHAT YOU BRING TO THE TABLE.", EnableOnEnd: false, 5f);
                AddNewHostMessage("WITH ALL OUR COMBINED EFFORTS, WE MAY ACTUALIZE OUR DREAMS.", EnableOnEnd: false, 4f);
                AddNewHostMessage("...", EnableOnEnd: false);
                AddNewHostMessage("I WILL REACH OUT TO YOU AGAIN SOON.", EnableOnEnd: false, 3.5f);
                AddNewHostMessage("I TRULY APPRECIATE YOUR TIME.", EnableOnEnd: false);
                AddNewHostMessage("GOODBYE FOR NOW, DR HYPOASTER.", EnableOnEnd: false, 3f);
                AddNewHostMessage("\"", EnableOnEnd: false, 1f);
                break;
            case 0:
                break;
        }
    }

    private bool PreviousMessageHadValidAnswer()
    {
        char[] array = previousAnswer.ToUpper().ToCharArray();
        string text = previousAnswer.ToUpper();
        if (array.Length != 0)
        {
            if (text.Contains("YES") || text.Contains("YEP") || text.Contains("YEA") || text.Contains("YEAH") || text.Contains("YUP") || text.Contains("OKAY") || text.Contains("ACKNOWLEDGED") || text.Contains("UNDERSTAND") || text.Contains("AGREE2ALL") || text.Contains("MHM") || text.Contains("OK") || text.Contains("SI") || text.Contains("SÍ"))
            {
                return true;
            }
            return false;
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
