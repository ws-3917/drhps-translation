using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatboxManager : MonoBehaviour
{
    [Header("- References -")]
    public Transform TextboxObject;

    public Image CharacterPortrait;

    public TextMeshProUGUI TextUI;

    public TextMeshProUGUI TextBulletpointUI;

    public AudioSource TextVoiceEmitter;

    public PlayerManager P_Manager;

    public ChatboxEffectTags EffectTagController;

    private PlayerManager.PlayerState PreviousPlayerState;

    [Header("- Text inner workings -")]
    private string CurrentText = "";

    public int CurrentTextIndex;

    public int CurrentAdditionalTextIndex;

    public bool CurrentChatOnTopPos;

    private int Text_SubtractedRichText;

    private int StoredAfterIndex;

    public int PauseCounter;

    [Header("- Text Settings -")]
    public bool FinishedShowingReactions;

    public CHATBOXTEXT storedchatboxtext;

    public CHATBOXTEXT previouschatboxtext;

    public bool ChatIsCurrentlyRunning;

    public bool AllowInput = true;

    public bool TextIsCurrentlyTyping;

    private INT_Chat storedreciever;

    public List<string> StoredAdditiveValues = new List<string>();

    [SerializeField]
    private Transform ReactionHolder;

    public GameObject ReactionTemplate;

    private List<GameObject> StoredReactions = new List<GameObject>();

    [SerializeField]
    private TMP_StyleSheet StyleSheet;

    public TMP_FontAsset DefaultFont;

    public TMP_FontAsset DyslexicFont;

    public Image[] ChatboxImages;

    public Sprite Kojima;

    public AudioClip DefaultSpeakSound;

    public Sprite DefaultIcon;

    public List<RectTransform> Options = new List<RectTransform>();

    public RectTransform HeartCursor;

    [SerializeField]
    private Transform ChoiceHolder;

    [SerializeField]
    private GameObject ChoicePrefab;

    public Animator ChatboxAnimator;

    public bool InDarkworld;

    public GameObject DarkworldTextShadow;

    public bool ChatboxInteractDebounce;

    private int ChoiceNumber;

    public int PreviousChosenChoiceIndex;

    private int cursorpos;

    private bool CurrentlyInChoice;

    private bool HideChoicesUntilFinish;

    private float CurrentTextSpeedMultiplier;

    private bool AllowPreviousStateTamper = true;

    private float TalkIconTimer;

    private float TalkIconInterval = 1f / 6f;

    private static ChatboxManager instance;

    public Action Event_OnLetterTyped;

    public static ChatboxManager Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {
        ProcessInput();
        TalkingIconProcess();
        SetHeartCursorPOS();
        if (InDarkworld)
        {
            ChatboxAnimator.Play("Chatbox_Darkworld");
            DarkworldTextShadow.SetActive(value: true);
        }
        else
        {
            ChatboxAnimator.Play("Chatbox_Lightworld");
            DarkworldTextShadow.SetActive(value: false);
        }
        if (StoredReactions.Count > 0 && !ChatIsCurrentlyRunning)
        {
            ClearReactions();
        }
    }

    private void ProcessInput()
    {
        bool key = Input.GetKey(PlayerInput.Instance.Key_Menu);
        if (CurrentText != "" || CurrentText != null)
        {
            PauseCounter = FormatCurrentText(CurrentText, IncludeBulletPoint: true, ActivateNextDialogueCharacter: false).Length - Text_SubtractedRichText;
        }
        if (P_Manager == null)
        {
            P_Manager = UnityEngine.Object.FindFirstObjectByType<PlayerManager>();
        }
        if (CurrentlyInChoice && TextUI.maxVisibleCharacters == PauseCounter && HideChoicesUntilFinish && !HeartCursor.gameObject.activeSelf)
        {
            HeartCursor.gameObject.SetActive(value: true);
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Left) && CurrentlyInChoice && AllowInput)
        {
            if (cursorpos >= 0 && cursorpos < Options.Count)
            {
                if (cursorpos - 1 >= 0)
                {
                    cursorpos--;
                }
            }
            else
            {
                int num = (int)Mathf.Floor((float)Options.Count / 2f - 0.2f);
                cursorpos = num;
            }
            SetHeartCursorPOS();
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Right) && CurrentlyInChoice && AllowInput)
        {
            if (cursorpos >= 0 && cursorpos < Options.Count)
            {
                if (cursorpos + 1 <= Options.Count - 1)
                {
                    cursorpos++;
                }
            }
            else
            {
                int num2 = (int)Mathf.Ceil((float)Options.Count / 2f);
                cursorpos = num2;
            }
            SetHeartCursorPOS();
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm) && cursorpos >= 0 && cursorpos < Options.Count && P_Manager._PlayerState != 0 && FinishedShowingReactions && CurrentlyInChoice && TextUI.maxVisibleCharacters == PauseCounter && AllowInput)
        {
            AttemptRunActions();
            AttemptRunSubActions(IsChatEnd: true);
            AttemptRunMultipleActions(IsChatEnd: true);
            ClearReactions();
            if (storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Choice[CurrentTextIndex] != null && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Choice[CurrentTextIndex].ChoiceTextResults[cursorpos] == null)
            {
                PreviousChosenChoiceIndex = cursorpos;
                if (CurrentTextIndex + 1 != storedchatboxtext.Textboxes[StoredAfterIndex].Text.Length && !LightworldMenu.Instance.MenuOpen && !DarkworldMenu.Instance.MenuOpen && storedchatboxtext != null)
                {
                    CurrentlyInChoice = false;
                    CurrentTextIndex++;
                    RunText(storedchatboxtext, StoredAfterIndex, storedreciever, ResetCurrentTextIndex: false);
                    MonoBehaviour.print("test");
                }
                else
                {
                    EndText();
                }
            }
            else
            {
                PreviousChosenChoiceIndex = cursorpos;
                CurrentlyInChoice = false;
                RunText(storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Choice[CurrentTextIndex].ChoiceTextResults[cursorpos], 0, storedreciever, ResetCurrentTextIndex: true);
            }
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Cancel) && P_Manager._PlayerState == PlayerManager.PlayerState.Cutscene && CurrentlyInChoice && AllowInput && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Choice[CurrentTextIndex] != null && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Choice[CurrentTextIndex].CanBackOut)
        {
            EndText();
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm) && !CurrentTextHasSkip(CurrentText) && P_Manager._PlayerState != 0 && FinishedShowingReactions && TextUI.maxVisibleCharacters == PauseCounter && !LightworldMenu.Instance.MenuOpen && !DarkworldMenu.Instance.MenuOpen && storedchatboxtext != null && !CurrentlyInChoice && AllowInput)
        {
            AttemptRunActions();
            AttemptRunSubActions(IsChatEnd: true);
            AttemptRunMultipleActions(IsChatEnd: true);
            if (storedchatboxtext != null && storedchatboxtext.Textboxes[StoredAfterIndex] != null && CurrentTextIndex + 1 != storedchatboxtext.Textboxes[StoredAfterIndex].Text.Length)
            {
                CurrentTextIndex++;
                RunText(storedchatboxtext, StoredAfterIndex, storedreciever, ResetCurrentTextIndex: false);
            }
            else
            {
                EndText();
            }
        }
        if (DRHDebugManager.instance.DebugModeEnabled && Input.GetKeyDown(PlayerInput.Instance.Key_Left) && !CurrentTextHasSkip(CurrentText) && P_Manager._PlayerState != 0 && FinishedShowingReactions && TextUI.maxVisibleCharacters == PauseCounter && !LightworldMenu.Instance.MenuOpen && !DarkworldMenu.Instance.MenuOpen && storedchatboxtext != null && !CurrentlyInChoice && AllowInput)
        {
            AttemptRunActions();
            AttemptRunSubActions(IsChatEnd: true);
            AttemptRunMultipleActions(IsChatEnd: true);
            if (storedchatboxtext != null && storedchatboxtext.Textboxes[StoredAfterIndex] != null && CurrentTextIndex + -1 >= 0)
            {
                CurrentTextIndex--;
                RunText(storedchatboxtext, StoredAfterIndex, storedreciever, ResetCurrentTextIndex: false);
            }
            else
            {
                BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_deny);
            }
        }
        if (DRHDebugManager.instance.DebugModeEnabled && Input.GetKeyDown(PlayerInput.Instance.Key_Up) && P_Manager._PlayerState != 0 && FinishedShowingReactions && TextUI.maxVisibleCharacters == PauseCounter && !LightworldMenu.Instance.MenuOpen && !DarkworldMenu.Instance.MenuOpen && storedchatboxtext != null && !CurrentlyInChoice && AllowInput)
        {
            AttemptRunActions();
            AttemptRunSubActions(IsChatEnd: true);
            AttemptRunMultipleActions(IsChatEnd: true);
            RunText(storedchatboxtext, StoredAfterIndex, storedreciever, ResetCurrentTextIndex: false);
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Cancel) && !CurrentTextHasSkip(CurrentText) && P_Manager._PlayerState != 0 && !DarkworldMenu.Instance.MenuOpen && !LightworldMenu.Instance.MenuOpen && TextUI.maxVisibleCharacters != PauseCounter && storedchatboxtext != null && AllowInput)
        {
            StopCoroutine("PlayText");
            FinishCurrentText();
        }
        if (!key || P_Manager._PlayerState == PlayerManager.PlayerState.Game || CurrentlyInChoice || LightworldMenu.Instance.MenuOpen || !(storedchatboxtext != null) || !AllowInput)
        {
            return;
        }
        AttemptRunActions();
        AttemptRunSubActions(IsChatEnd: true);
        AttemptRunMultipleActions(IsChatEnd: true);
        if (!CurrentlyInChoice)
        {
            if (storedchatboxtext != null && storedchatboxtext.Textboxes[StoredAfterIndex] != null && CurrentTextIndex + 1 != storedchatboxtext.Textboxes[StoredAfterIndex].Text.Length)
            {
                CurrentTextIndex++;
                if (storedreciever != null)
                {
                    RunText(storedchatboxtext, StoredAfterIndex, storedreciever, ResetCurrentTextIndex: false);
                }
                else
                {
                    RunText(storedchatboxtext, StoredAfterIndex, null, ResetCurrentTextIndex: false);
                }
            }
            else
            {
                EndText();
            }
        }
        else
        {
            StopCoroutine("PlayText");
            FinishCurrentText();
        }
    }

    private bool CurrentTextHasSkip(string Text)
    {
        if (Text == null || Text == "" || SettingsManager.Instance.GetBoolSettingValue("InstantText"))
        {
            return false;
        }
        if (Text.Contains('£') | Text.Contains('*'))
        {
            return true;
        }
        return false;
    }

    private string FormatCurrentText(string TargetText, bool IncludeBulletPoint, bool ActivateNextDialogueCharacter)
    {
        string input = TargetText;
        input = Regex.Replace(input, "(?<!\\=)#", "");
        input = input.Replace("@", "");
        input = input.Replace(";", "\n");
        input = input.Replace("~", "\n");
        input = input.Replace("&", Environment.UserName.ToUpper());
        while (input.Contains('|'))
        {
            int num = input.IndexOf('|');
            int num2 = num + 1;
            int i;
            for (i = num2; i < input.Length && char.IsDigit(input[i]); i++)
            {
            }
            if (int.TryParse(input.Substring(num2, i - num2), out var result) && result >= 0 && result < StoredAdditiveValues.Count)
            {
                string value = StoredAdditiveValues[result];
                input = input.Remove(num, i - num);
                input = input.Insert(num, value);
                continue;
            }
            Debug.LogError("Invalid index after '|' or index out of range.");
            break;
        }
        if (input.Contains('£') && ActivateNextDialogueCharacter && !SettingsManager.Instance.GetBoolSettingValue("InstantText"))
        {
            if (storedchatboxtext != null && storedchatboxtext.Textboxes[StoredAfterIndex] != null && CurrentTextIndex + 1 != storedchatboxtext.Textboxes[StoredAfterIndex].Text.Length)
            {
                CurrentTextIndex++;
                RunText(storedchatboxtext, StoredAfterIndex, storedreciever, ResetCurrentTextIndex: false);
            }
            else
            {
                EndText();
            }
        }
        input = input.Replace("£", "-");
        if (input.Contains('*') && ActivateNextDialogueCharacter && !SettingsManager.Instance.GetBoolSettingValue("InstantText"))
        {
            if (storedchatboxtext != null && storedchatboxtext.Textboxes[StoredAfterIndex] != null && CurrentTextIndex + 1 != storedchatboxtext.Textboxes[StoredAfterIndex].Text.Length)
            {
                CurrentTextIndex++;
                RunText(storedchatboxtext, StoredAfterIndex, storedreciever, ResetCurrentTextIndex: false);
            }
            else
            {
                EndText();
            }
        }
        return input.Replace("*", "-");
    }

    private void FinishCurrentText(bool TellRecieverIsFinished = true)
    {
        if (storedchatboxtext == null || storedchatboxtext.Textboxes[CurrentAdditionalTextIndex] == null)
        {
            return;
        }
        if (storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Character[CurrentTextIndex] != null)
        {
            if (storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Character[CurrentTextIndex].GiveCharacterBulletpoint)
            {
                TextBulletpointUI.text = "*";
            }
            else
            {
                TextBulletpointUI.text = "";
            }
        }
        else
        {
            TextBulletpointUI.text = "*";
        }
        if (storedreciever != null && TellRecieverIsFinished)
        {
            storedreciever.CurrentlyBeingUsed = false;
        }
        TextUI.text = FormatCurrentText(CurrentText, IncludeBulletPoint: true, ActivateNextDialogueCharacter: true);
        StartShowingReactions();
        if (ChoiceNumber > 0)
        {
            ShowChoices();
        }
        TextIsCurrentlyTyping = false;
        Text_SubtractedRichText = CountRichTextTagCharacters(TextUI.text);
        TextUI.maxVisibleCharacters = TextUI.text.Length - Text_SubtractedRichText;
        char[] array = storedchatboxtext.Textboxes[StoredAfterIndex].Text[CurrentTextIndex].ToCharArray();
        foreach (char num in array)
        {
            if (num == ';')
            {
                MonoBehaviour.print("DETECTED");
                if (storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Character[CurrentTextIndex] != null)
                {
                    if (storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Character[CurrentTextIndex].GiveCharacterBulletpoint)
                    {
                        TextBulletpointUI.text += "\n*";
                    }
                    else
                    {
                        TextBulletpointUI.text += "\n";
                    }
                }
                else
                {
                    TextBulletpointUI.text += "\n*";
                }
            }
            if (num != '~')
            {
                continue;
            }
            MonoBehaviour.print("DETECTED");
            if (storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Character[CurrentTextIndex] != null)
            {
                if (storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Character[CurrentTextIndex].GiveCharacterBulletpoint)
                {
                    TextBulletpointUI.text += "\n\n*";
                }
                else
                {
                    TextBulletpointUI.text += "\n";
                }
            }
            else
            {
                TextBulletpointUI.text += "\n\n*";
            }
        }
    }

    private void StartShowingReactions()
    {
        StartCoroutine(ShowCurrentTextReactions());
    }

    private IEnumerator ShowCurrentTextReactions()
    {
        FinishedShowingReactions = false;
        if (storedchatboxtext == null || !ChatIsCurrentlyRunning || storedchatboxtext.Textboxes == null || storedchatboxtext.Textboxes.Length == 0)
        {
            FinishedShowingReactions = true;
            yield break;
        }
        Textbox textbox = storedchatboxtext.Textboxes[CurrentAdditionalTextIndex];
        if (textbox == null || textbox.Reaction == null || textbox.Reaction.Length == 0)
        {
            FinishedShowingReactions = true;
            yield break;
        }
        CHATBOXREACTION cHATBOXREACTION = textbox.Reaction[CurrentTextIndex];
        if (cHATBOXREACTION == null || cHATBOXREACTION.Reaction == null)
        {
            FinishedShowingReactions = true;
            yield break;
        }
        REACTIONDATA[] reaction = cHATBOXREACTION.Reaction;
        foreach (REACTIONDATA rEACTIONDATA in reaction)
        {
            if (rEACTIONDATA == null || rEACTIONDATA.Character == null)
            {
                continue;
            }
            GameObject gameObject = UnityEngine.Object.Instantiate(ReactionTemplate, ReactionHolder);
            if (gameObject == null)
            {
                continue;
            }
            Image componentInChildren = gameObject.GetComponentInChildren<Image>();
            TextMeshProUGUI componentInChildren2 = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            if (componentInChildren2 == null || componentInChildren == null)
            {
                UnityEngine.Object.Destroy(gameObject);
                continue;
            }
            gameObject.SetActive(value: true);
            gameObject.GetComponent<RectTransform>().localPosition = rEACTIONDATA.Offset;
            componentInChildren2.text = rEACTIONDATA.Text;
            componentInChildren2.fontSize = rEACTIONDATA.Character.CharacterFontSize / 2f;
            componentInChildren2.color = rEACTIONDATA.Character.TextColor;
            if (PlayerPrefs.GetInt("Setting_DyslexicText", 0) == 0)
            {
                componentInChildren2.font = ((rEACTIONDATA.Character.CharacterFont != null) ? rEACTIONDATA.Character.CharacterFont : DefaultFont);
            }
            else
            {
                componentInChildren2.font = DyslexicFont;
            }
            if (rEACTIONDATA.Character.CharacterIcon != null)
            {
                componentInChildren.sprite = rEACTIONDATA.Character.CharacterIcon;
                componentInChildren.rectTransform.sizeDelta = new Vector2(rEACTIONDATA.Character.CharacterIconWidth / 2f, rEACTIONDATA.Character.CharacterIconHeight / 2f);
            }
            else
            {
                componentInChildren.sprite = DefaultIcon;
            }
            StoredReactions.Add(gameObject);
            yield return new WaitForSeconds(0.05f);
        }
        FinishedShowingReactions = true;
    }

    private void ClearReactions()
    {
        StopCoroutine("ShowCurrentTextReactions");
        foreach (GameObject storedReaction in StoredReactions)
        {
            UnityEngine.Object.Destroy(storedReaction);
        }
        StoredReactions.Clear();
    }

    private void TalkingIconProcess()
    {
        TalkIconTimer += Time.deltaTime;
        if (!ChatIsCurrentlyRunning)
        {
            return;
        }
        if (TextUI.maxVisibleCharacters < PauseCounter || SettingsManager.Instance.GetBoolSettingValue("InstantText"))
        {
            if (storedchatboxtext != null && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex] != null && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Character[CurrentTextIndex] != null && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Character[CurrentTextIndex].CharacterHasTalkingAnimation && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Character[CurrentTextIndex].CharacterIcon != null && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Character[CurrentTextIndex].CharacterTalkingIcon != null && TalkIconTimer >= TalkIconInterval)
            {
                MonoBehaviour.print("SUCCESS!");
                if (CharacterPortrait.sprite == storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Character[CurrentTextIndex].CharacterIcon)
                {
                    CharacterPortrait.sprite = storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Character[CurrentTextIndex].CharacterTalkingIcon;
                }
                else
                {
                    CharacterPortrait.sprite = storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Character[CurrentTextIndex].CharacterIcon;
                }
                TalkIconTimer = 0f;
            }
        }
        else if (storedchatboxtext != null && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex] != null && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Character[CurrentTextIndex] != null && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Character[CurrentTextIndex].CharacterIcon != null)
        {
            CharacterPortrait.sprite = storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Character[CurrentTextIndex].CharacterIcon;
        }
    }

    public void MoveTextboxToScreenPos(bool OnBottom)
    {
        if (OnBottom)
        {
            SetTextboxPosY(-280f);
            CurrentChatOnTopPos = false;
        }
        else
        {
            SetTextboxPosY(280f);
            CurrentChatOnTopPos = true;
        }
    }

    public static void MoveTextboxToTop()
    {
        Instance.MoveTextboxToScreenPos(OnBottom: false);
    }

    public static void MoveTextboxToBottom()
    {
        Instance.MoveTextboxToScreenPos(OnBottom: true);
    }

    private void AttemptRunActions()
    {
        Debug.Log("AttemptRunActions started.");
        if (storedchatboxtext.Textboxes == null)
        {
            Debug.LogError("storedchatboxtext.Textboxes is null");
            return;
        }
        if (storedchatboxtext.Textboxes[CurrentAdditionalTextIndex] == null)
        {
            Debug.LogError("storedchatboxtext.Textboxes[CurrentAdditionalTextIndex] is null");
            return;
        }
        Textbox textbox = storedchatboxtext.Textboxes[CurrentAdditionalTextIndex];
        if (textbox.Action == null)
        {
            Debug.LogError("currentTextbox.Action is null");
            return;
        }
        if (textbox.Action.Length == 0)
        {
            Debug.Log("currentTextbox.Action.Length is 0");
            return;
        }
        if (textbox.Action[CurrentTextIndex] == null)
        {
            Debug.Log("currentTextbox.Action[CurrentTextIndex] is null");
            return;
        }
        CHATBOXACTION cHATBOXACTION = textbox.Action[CurrentTextIndex];
        if (!cHATBOXACTION.RunActionOnChatEnd)
        {
            Debug.Log("action.RunActionOnChatEnd is false");
            return;
        }
        Debug.Log("Action will be run: " + cHATBOXACTION.ToString());
        if (cHATBOXACTION.PlaySound)
        {
            if (cHATBOXACTION.PossibleSounds == null)
            {
                Debug.LogError("action.PossibleSounds is null");
                return;
            }
            if (cHATBOXACTION.PossibleSounds.Length == 0)
            {
                Debug.LogError("action.PossibleSounds.Length is 0");
                return;
            }
            AudioClip audioClip = cHATBOXACTION.PossibleSounds[UnityEngine.Random.Range(0, cHATBOXACTION.PossibleSounds.Length)];
            Debug.Log("Playing sound: " + audioClip.name);
            TextVoiceEmitter.PlayOneShot(audioClip);
        }
        if (!cHATBOXACTION.RunComponentFunction)
        {
            return;
        }
        if (cHATBOXACTION.TargetComponentGameObjectName == null)
        {
            Debug.LogError("action.TargetComponentGameObjectName is null");
            return;
        }
        GameObject gameObject = GameObject.Find(cHATBOXACTION.TargetComponentGameObjectName);
        if (gameObject == null)
        {
            Debug.LogError("TargetGameObject not found: " + cHATBOXACTION.TargetComponentGameObjectName);
            return;
        }
        if (cHATBOXACTION.FunctionName == null)
        {
            Debug.LogError("action.FunctionName is null");
            return;
        }
        string targetComponentClassname = cHATBOXACTION.TargetComponentClassname;
        if (targetComponentClassname == null)
        {
            Debug.LogError("action.TargetComponentClassname is null");
            return;
        }
        Component component = gameObject.GetComponent(targetComponentClassname);
        if (component == null)
        {
            Debug.LogError("Couldn't find Component named: " + targetComponentClassname);
            return;
        }
        MethodInfo method = component.GetType().GetMethod(cHATBOXACTION.FunctionName);
        if (method == null)
        {
            Debug.LogError("Method not found: " + cHATBOXACTION.FunctionName);
            return;
        }
        Debug.Log("Invoking method: " + cHATBOXACTION.FunctionName);
        method.Invoke(component, null);
        Debug.Log("AttemptRunActions ended.");
    }

    private void AttemptRunMultipleActions(bool IsChatEnd = false)
    {
        Debug.Log("AttemptRunMultipleActions started.");
        if (storedchatboxtext == null)
        {
            Debug.Log("storedchatboxtext is null");
            return;
        }
        if (storedchatboxtext.Textboxes == null)
        {
            Debug.Log("storedchatboxtext.Textboxes is null");
            return;
        }
        if (storedchatboxtext.Textboxes[CurrentAdditionalTextIndex] == null)
        {
            Debug.LogError("storedchatboxtext.Textboxes[CurrentAdditionalTextIndex] is null");
            return;
        }
        Textbox textbox = storedchatboxtext.Textboxes[CurrentAdditionalTextIndex];
        if (textbox.SubActions == null)
        {
            Debug.Log("currentTextbox.SubActions is null");
            return;
        }
        if (textbox.SubActions.Length == 0)
        {
            Debug.Log("currentTextbox.SubActions.Length is 0");
            return;
        }
        if (textbox.SubActions[CurrentTextIndex].SubActions.Length == 0)
        {
            Debug.Log("currentTextbox.SubActions.SubActions.Length is 0");
            return;
        }
        CHATBOXSUBACTION cHATBOXSUBACTION = textbox.SubActions[CurrentTextIndex];
        CHATBOXACTION[] subActions = cHATBOXSUBACTION.SubActions;
        foreach (CHATBOXACTION cHATBOXACTION in subActions)
        {
            if (cHATBOXACTION == null)
            {
                Debug.Log("Action is null, skipping");
            }
            else
            {
                if (cHATBOXACTION.RunActionOnChatEnd != IsChatEnd)
                {
                    continue;
                }
                Debug.Log("Action will be run: " + cHATBOXSUBACTION.ToString());
                if (cHATBOXACTION.PlaySound)
                {
                    if (cHATBOXACTION.PossibleSounds == null)
                    {
                        Debug.LogError("action.PossibleSounds is null");
                        continue;
                    }
                    if (cHATBOXACTION.PossibleSounds.Length == 0)
                    {
                        Debug.LogError("action.PossibleSounds.Length is 0");
                        continue;
                    }
                    AudioClip audioClip = cHATBOXACTION.PossibleSounds[UnityEngine.Random.Range(0, cHATBOXACTION.PossibleSounds.Length)];
                    Debug.Log("Playing sound: " + audioClip.name);
                    TextVoiceEmitter.PlayOneShot(audioClip);
                }
                if (!cHATBOXACTION.RunComponentFunction)
                {
                    continue;
                }
                if (cHATBOXACTION.TargetComponentGameObjectName == null)
                {
                    Debug.LogError("action.TargetComponentGameObjectName is null");
                    continue;
                }
                GameObject gameObject = GameObject.Find(cHATBOXACTION.TargetComponentGameObjectName);
                if (gameObject == null)
                {
                    Debug.LogError("TargetGameObject not found: " + cHATBOXACTION.TargetComponentGameObjectName);
                    continue;
                }
                if (cHATBOXACTION.FunctionName == null)
                {
                    Debug.LogError("action.FunctionName is null");
                    continue;
                }
                string targetComponentClassname = cHATBOXACTION.TargetComponentClassname;
                if (targetComponentClassname == null)
                {
                    Debug.LogError("action.TargetComponentClassname is null");
                    continue;
                }
                Component component = gameObject.GetComponent(targetComponentClassname);
                if (component == null)
                {
                    Debug.LogError("Couldn't find Component named: " + targetComponentClassname);
                    continue;
                }
                MethodInfo method = component.GetType().GetMethod(cHATBOXACTION.FunctionName);
                if (method == null)
                {
                    Debug.LogError("Method not found: " + cHATBOXACTION.FunctionName);
                    continue;
                }
                Debug.Log("Invoking method: " + cHATBOXACTION.FunctionName);
                method.Invoke(component, null);
            }
        }
        Debug.Log("AttemptRunMultipleActions ended.");
    }

    private void AttemptRunSubActions(bool IsChatEnd = false)
    {
        if (storedchatboxtext == null)
        {
            return;
        }
        if (storedchatboxtext.Textboxes[CurrentAdditionalTextIndex] == null)
        {
            Debug.LogError("storedchatboxtext.Textboxes[CurrentAdditionalTextIndex] is null");
            return;
        }
        Textbox textbox = storedchatboxtext.Textboxes[CurrentAdditionalTextIndex];
        if (textbox.Action == null)
        {
            Debug.LogError("currentTextbox.Action is null");
            return;
        }
        if (textbox.Action.Length == 0)
        {
            Debug.Log("currentTextbox.Action.Length is 0");
            return;
        }
        if (textbox.Action[CurrentTextIndex] == null)
        {
            Debug.Log("currentTextbox.Action[CurrentTextIndex] is null");
            return;
        }
        CHATBOXACTION obj = textbox.Action[CurrentTextIndex];
        CHATBOXACTION_SubAction[] subActions = obj.SubActions;
        if (obj.RunComponentFunction && subActions != null && subActions.Length != 0)
        {
            CHATBOXACTION_SubAction[] array = subActions;
            foreach (CHATBOXACTION_SubAction cHATBOXACTION_SubAction in array)
            {
                if (cHATBOXACTION_SubAction.RunActionOnChatEnd != IsChatEnd)
                {
                    return;
                }
                if (cHATBOXACTION_SubAction.TargetComponentGameObjectName == null)
                {
                    Debug.LogError("action.TargetComponentGameObjectName is null");
                    return;
                }
                GameObject gameObject = GameObject.Find(cHATBOXACTION_SubAction.TargetComponentGameObjectName);
                if (gameObject == null)
                {
                    Debug.LogError("TargetGameObject not found: " + cHATBOXACTION_SubAction.TargetComponentGameObjectName);
                    return;
                }
                if (cHATBOXACTION_SubAction.FunctionName == null)
                {
                    Debug.LogError("action.FunctionName is null");
                    return;
                }
                string targetComponentClassname = cHATBOXACTION_SubAction.TargetComponentClassname;
                if (targetComponentClassname == null)
                {
                    Debug.LogError("action.TargetComponentClassname is null");
                    return;
                }
                Component component = gameObject.GetComponent(targetComponentClassname);
                if (component == null)
                {
                    Debug.LogError("Couldn't find Component named: " + targetComponentClassname);
                    return;
                }
                MethodInfo method = component.GetType().GetMethod(cHATBOXACTION_SubAction.FunctionName);
                if (method == null)
                {
                    Debug.LogError("Method not found: " + cHATBOXACTION_SubAction.FunctionName);
                    return;
                }
                Debug.Log("Invoking method: " + cHATBOXACTION_SubAction.FunctionName);
                method.Invoke(component, null);
            }
        }
        Debug.Log("AttemptRunActions ended.");
    }

    private void SetupChoices(CHATBOXTEXT chatbox, int index)
    {
        CleanupOptions();
        CHATBOXCHOICE cHATBOXCHOICE = chatbox.Textboxes[index].Choice[CurrentTextIndex];
        if (cHATBOXCHOICE != null)
        {
            CurrentlyInChoice = true;
            ChoiceNumber = cHATBOXCHOICE.Choices.Count;
            cursorpos = cHATBOXCHOICE.DefaultSelectedChoice;
            if (cHATBOXCHOICE.ShowOnTextScrollFinish)
            {
                HideChoices();
            }
            else
            {
                ShowChoices();
            }
            for (int i = 0; i < ChoiceNumber; i++)
            {
                GameObject obj = UnityEngine.Object.Instantiate(ChoicePrefab, ChoiceHolder);
                RectTransform component = obj.GetComponent<RectTransform>();
                TextMeshProUGUI component2 = obj.GetComponent<TextMeshProUGUI>();
                component2.text = Regex.Replace(cHATBOXCHOICE.Choices[i], "([‘’“”\\u201A-\\u201F\\u4E00-\\u9FFF\\u3000-\\u303F\\uFF00-\\uFFEF])", "$1 ");
                component2.text = component2.text.Replace('\n', '\n');
                if (PlayerPrefs.GetInt("Setting_DyslexicText", 0) == 1)
                {
                    component2.font = DyslexicFont;
                    component2.textStyle = StyleSheet.GetStyle(1867431062);
                    component2.extraPadding = false;
                }
                else if (PlayerPrefs.GetInt("Setting_NoFont", 0) == 0)
                {
                    if (cHATBOXCHOICE.ChoiceCharacterReference != null)
                    {
                        component2.font = DefaultFont;
                        component2.textStyle = StyleSheet.GetStyle((int)cHATBOXCHOICE.ChoiceCharacterReference.TextStyleSheet);
                        component2.extraPadding = true;
                        if (cHATBOXCHOICE.ChoiceCharacterReference.CharacterFont != null)
                        {
                            component2.font = cHATBOXCHOICE.ChoiceCharacterReference.CharacterFont;
                        }
                    }
                    else
                    {
                        component2.font = DefaultFont;
                        component2.textStyle = StyleSheet.GetStyle(1867431062);
                        component2.extraPadding = true;
                    }
                }
                else
                {
                    component2.font = DefaultFont;
                    component2.textStyle = StyleSheet.GetStyle(1867431062);
                    component2.extraPadding = true;
                }
                component.localPosition = cHATBOXCHOICE.ChoicePositions[i];
                Options.Add(component);
            }
            SetHeartCursorPOS();
        }
        else
        {
            CleanupOptions();
        }
    }

    private void CleanupOptions()
    {
        foreach (RectTransform option in Options)
        {
            UnityEngine.Object.Destroy(option.gameObject);
        }
        Options.Clear();
        HeartCursor.gameObject.SetActive(value: false);
    }

    private void SetHeartCursorPOS()
    {
        if (Options.Count == 0)
        {
            return;
        }
        if (cursorpos >= 0 && cursorpos <= Options.Count)
        {
            HeartCursor.localPosition = new Vector2(Options[cursorpos].localPosition.x - 35f, Options[cursorpos].localPosition.y);
        }
        else
        {
            HeartCursor.localPosition = new Vector2(0f, 0f);
        }
        for (int i = 0; i < Options.Count; i++)
        {
            if (cursorpos >= 0 && cursorpos < Options.Count)
            {
                if (i != cursorpos)
                {
                    Options[i].GetComponent<TextMeshProUGUI>().color = Color.white;
                }
                else
                {
                    Options[i].GetComponent<TextMeshProUGUI>().color = new Color(0.996f, 1f, 0f);
                }
            }
            else
            {
                Options[i].GetComponent<TextMeshProUGUI>().color = Color.white;
            }
        }
    }

    private void ShowChoices()
    {
        ChoiceHolder.gameObject.SetActive(value: true);
        HeartCursor.gameObject.SetActive(value: true);
        HideChoicesUntilFinish = false;
    }

    private void HideChoices()
    {
        ChoiceHolder.gameObject.SetActive(value: false);
        HeartCursor.gameObject.SetActive(value: false);
        HideChoicesUntilFinish = true;
    }

    private void SetTextboxPosY(float PosY)
    {
        TextboxObject.localPosition = new Vector2(TextboxObject.localPosition.x, PosY);
    }

    public void MimicInput_Confirm()
    {
        if (!CurrentTextHasSkip(CurrentText) && P_Manager._PlayerState != 0 && FinishedShowingReactions && TextUI.maxVisibleCharacters == PauseCounter && !LightworldMenu.Instance.MenuOpen && !DarkworldMenu.Instance.MenuOpen && storedchatboxtext != null && !CurrentlyInChoice)
        {
            AttemptRunActions();
            AttemptRunSubActions(IsChatEnd: true);
            AttemptRunMultipleActions(IsChatEnd: true);
            if (storedchatboxtext != null && storedchatboxtext.Textboxes[StoredAfterIndex] != null && CurrentTextIndex + 1 != storedchatboxtext.Textboxes[StoredAfterIndex].Text.Length)
            {
                CurrentTextIndex++;
                RunText(storedchatboxtext, StoredAfterIndex, storedreciever, ResetCurrentTextIndex: false);
            }
            else
            {
                EndText();
            }
        }
    }

    public void RunText(CHATBOXTEXT Chatbox, int index, INT_Chat reciever, bool ResetCurrentTextIndex)
    {
        if (ResetCurrentTextIndex)
        {
            CurrentTextIndex = 0;
        }
        StopCoroutine("PlayText");
        CleanupOptions();
        ClearReactions();
        ChatIsCurrentlyRunning = true;
        CurrentAdditionalTextIndex = index;
        CurrentTextSpeedMultiplier = 1f;
        TextUI.color = Color.white;
        ChatboxInteractDebounce = true;
        Text_SubtractedRichText = 0;
        if (AllowPreviousStateTamper)
        {
            PreviousPlayerState = P_Manager._PlayerState;
            AllowPreviousStateTamper = false;
        }
        Image[] chatboxImages = ChatboxImages;
        for (int i = 0; i < chatboxImages.Length; i++)
        {
            chatboxImages[i].enabled = true;
        }
        if (Chatbox.Textboxes[index].Character.Length != 0 && Chatbox.Textboxes[index].Character[CurrentTextIndex] == null)
        {
            CharacterPortrait.sprite = DefaultIcon;
            TextUI.margin = new Vector4(-145.0626f, 17.84375f, -20.04016f, 16.56921f);
            TextBulletpointUI.margin = new Vector4(-200.549f, 17.84375f, 800.6914f, 16.56921f);
        }
        else
        {
            (CharacterPortrait.transform as RectTransform).sizeDelta = new Vector2(Chatbox.Textboxes[index].Character[CurrentTextIndex].CharacterIconWidth, Chatbox.Textboxes[index].Character[CurrentTextIndex].CharacterIconHeight);
            TextUI.margin = new Vector4(112.481f, 17.84375f, -20.04016f, 16.56921f);
            TextBulletpointUI.margin = new Vector4(58.25925f, 17.84375f, 800.6914f, 16.56921f);
            UnityEngine.Random.Range(0, 10000);
            CurrentTextSpeedMultiplier = Chatbox.Textboxes[index].Character[CurrentTextIndex].TextSpeedMultiplier;
            TextUI.color = Chatbox.Textboxes[index].Character[CurrentTextIndex].TextColor;
            if (Chatbox.Textboxes[index].Character[CurrentTextIndex].CharacterIcon != null)
            {
                CharacterPortrait.sprite = Chatbox.Textboxes[index].Character[CurrentTextIndex].CharacterIcon;
            }
            else
            {
                CharacterPortrait.sprite = DefaultIcon;
                TextUI.margin = new Vector4(-145.0626f, 17.84375f, -20.04016f, 16.56921f);
                TextBulletpointUI.margin = new Vector4(-200.549f, 17.84375f, 800.6914f, 16.56921f);
            }
        }
        if (Chatbox.Textboxes[index].Character[CurrentTextIndex] == null || Chatbox.Textboxes[index].Character[CurrentTextIndex].CharacterSound == null)
        {
            TextVoiceEmitter.clip = DefaultSpeakSound;
        }
        else
        {
            TextVoiceEmitter.clip = Chatbox.Textboxes[index].Character[CurrentTextIndex].CharacterSound;
        }
        if (Chatbox.Textboxes[index] != null && Chatbox.Textboxes[index].Choice.Length != 0 && Chatbox.Textboxes[index].Choice[CurrentTextIndex] != null)
        {
            SetupChoices(Chatbox, index);
        }
        else
        {
            ChoiceNumber = 0;
        }
        TextUI.enabled = true;
        TextUI.text = "";
        StoredAfterIndex = index;
        storedchatboxtext = Chatbox;
        if (reciever != null)
        {
            storedreciever = reciever;
            if (Chatbox.Textboxes[index].Character[CurrentTextIndex] != null && Chatbox.Textboxes[index].Character[CurrentTextIndex].TellRecieverIfChatting)
            {
                reciever.CurrentlyBeingUsed = true;
            }
            else
            {
                reciever.CurrentlyBeingUsed = false;
            }
        }
        if (Camera.main != null && PlayerManager.Instance != null && storedreciever != null)
        {
            if (!storedreciever.ManualTextboxPosition)
            {
                if (PlayerManager.Instance.transform.position.y >= Camera.main.transform.position.y)
                {
                    SetTextboxPosY(-280f);
                    CurrentChatOnTopPos = false;
                }
                else
                {
                    SetTextboxPosY(280f);
                    CurrentChatOnTopPos = true;
                }
            }
            else if (storedreciever.OnBottom)
            {
                SetTextboxPosY(-280f);
                CurrentChatOnTopPos = false;
            }
            else
            {
                SetTextboxPosY(280f);
                CurrentChatOnTopPos = true;
            }
        }
        TextUI.font = DefaultFont;
        TextUI.textStyle = StyleSheet.GetStyle(1867431062);
        TextBulletpointUI.font = DefaultFont;
        TextUI.extraPadding = true;
        if (PlayerPrefs.GetInt("Setting_DyslexicText", 0) == 1)
        {
            TextUI.font = DyslexicFont;
            TextUI.textStyle = StyleSheet.GetStyle(1867431062);
            TextBulletpointUI.font = DefaultFont;
            TextUI.extraPadding = false;
        }
        else if (PlayerPrefs.GetInt("Setting_NoFont", 0) == 0 && Chatbox.Textboxes[index].Character[CurrentTextIndex] != null && Chatbox.Textboxes[index].Character[CurrentTextIndex].CharacterFont != null)
        {
            if (Chatbox.Textboxes[index].Character[CurrentTextIndex].CharacterFont != null)
            {
                TextUI.font = Chatbox.Textboxes[index].Character[CurrentTextIndex].CharacterFont;
            }
            TextUI.textStyle = StyleSheet.GetStyle((int)Chatbox.Textboxes[index].Character[CurrentTextIndex].TextStyleSheet);
            TextBulletpointUI.font = TextUI.font;
        }
        if (Chatbox.Textboxes[index].Character[CurrentTextIndex] != null && Chatbox.Textboxes[index].Character[CurrentTextIndex].CharacterFontSize != 0f)
        {
            TextUI.fontSize = Chatbox.Textboxes[index].Character[CurrentTextIndex].CharacterFontSize;
        }
        else
        {
            TextUI.fontSize = 64f;
        }
        if (LightworldMenu.Instance.MenuOpen)
        {
            LightworldMenu.Instance.ForceCloseMenu();
            PreviousPlayerState = PlayerManager.PlayerState.Game;
        }
        if (DarkworldMenu.Instance.MenuOpen)
        {
            DarkworldMenu.Instance.CloseMenu();
            PreviousPlayerState = PlayerManager.PlayerState.Game;
        }
        StartCoroutine("PlayText");
    }

    public void EndText()
    {
        Image[] chatboxImages = ChatboxImages;
        for (int i = 0; i < chatboxImages.Length; i++)
        {
            chatboxImages[i].enabled = false;
        }
        previouschatboxtext = storedchatboxtext;
        if (storedreciever != null)
        {
            storedreciever.FinishedText = true;
            storedreciever.CurrentlyBeingUsed = false;
        }
        TextUI.enabled = false;
        if (PreviousPlayerState == PlayerManager.PlayerState.Game)
        {
            P_Manager._PlayerState = PlayerManager.PlayerState.Game;
        }
        PreviousPlayerState = PlayerManager.PlayerState.Game;
        AllowPreviousStateTamper = true;
        TextIsCurrentlyTyping = false;
        StoredAfterIndex = 0;
        storedchatboxtext = null;
        CurrentText = "";
        TextBulletpointUI.text = "";
        PauseCounter = 0;
        cursorpos = 0;
        ChoiceNumber = 0;
        Text_SubtractedRichText = 0;
        StartCoroutine(DebounceDelay());
        ClearReactions();
        CleanupOptions();
        HeartCursor.gameObject.SetActive(value: false);
        SetHeartCursorPOS();
        CurrentTextIndex = 0;
        CurrentlyInChoice = false;
        CurrentAdditionalTextIndex = 0;
        if (storedreciever != null)
        {
            StartCoroutine(storedreciever.DebounceInteract());
            storedreciever.CurrentlyBeingUsed = false;
        }
        storedreciever = null;
        StopCoroutine("PlayText");
        ChatIsCurrentlyRunning = false;
    }

    private IEnumerator DebounceDelay()
    {
        yield return new WaitForSeconds(0.01f);
        ChatboxInteractDebounce = false;
    }

    public IEnumerator InputDebounceDelay()
    {
        bool previousInputState = AllowInput;
        AllowInput = false;
        yield return new WaitForSeconds(0.1f);
        AllowInput = previousInputState;
    }

    private IEnumerator PlayText()
    {
        string StoredText = storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Text[CurrentTextIndex];
        StoredText = StoredText.Replace("£", "*");
        StoredText = Regex.Replace(StoredText, "([‘’“”\\u201A-\\u201F\\u4E00-\\u9FFF\\u3000-\\u303F\\uFF00-\\uFFEF])", "$1 ");
        StoredText = Regex.Replace(StoredText, "([a-zA-Z0-9\\.\\-@#])([‘’“”\\u201A-\\u201F\\u4E00-\\u9FFF\\u3000-\\u303F\\uFF00-\\uFFEF])", "$1 $2");
        Debug.LogWarning(StoredText);
        TextUI.text = StoredText;
        if (storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Character[CurrentTextIndex] != null)
        {
            if (storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Character[CurrentTextIndex].GiveCharacterBulletpoint)
            {
                TextBulletpointUI.text = "*";
            }
            else
            {
                TextBulletpointUI.text = "";
            }
        }
        else
        {
            TextBulletpointUI.text = "*";
        }
        CurrentText = StoredText;
        TextUI.maxVisibleCharacters = 0;
        int MaxVisibleCharacters = 0;
        Text_SubtractedRichText = 0;
        bool ForcedFinishText = false;
        TextIsCurrentlyTyping = true;
        int messageCharLength = StoredText.Length;
        char[] messageCharacters = StoredText.ToCharArray();
        if (storedchatboxtext != null && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex] != null && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Action.Length != 0 && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Action[CurrentTextIndex] != null && !storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Action[CurrentTextIndex].RunActionOnChatEnd)
        {
            CHATBOXACTION cHATBOXACTION = storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Action[CurrentTextIndex];
            if (cHATBOXACTION.PlaySound && cHATBOXACTION.PossibleSounds.Length != 0)
            {
                AudioClip clip = cHATBOXACTION.PossibleSounds[UnityEngine.Random.Range(0, cHATBOXACTION.PossibleSounds.Length)];
                TextVoiceEmitter.PlayOneShot(clip);
            }
            GameObject gameObject = GameObject.Find(cHATBOXACTION.TargetComponentGameObjectName);
            if (cHATBOXACTION.RunComponentFunction && gameObject != null && cHATBOXACTION.FunctionName != null)
            {
                string targetComponentClassname = cHATBOXACTION.TargetComponentClassname;
                Component component = gameObject.GetComponent(targetComponentClassname);
                if (component != null)
                {
                    if (component.GetType().GetMethod(cHATBOXACTION.FunctionName) != null)
                    {
                        component.GetType().GetMethod(cHATBOXACTION.FunctionName).Invoke(component, null);
                    }
                    else
                    {
                        MonoBehaviour.print("did you forget to make the method public?");
                    }
                }
                else
                {
                    MonoBehaviour.print("Couldn't find Component named: " + targetComponentClassname);
                }
            }
        }
        AttemptRunSubActions();
        AttemptRunMultipleActions();
        if (AllowInput && SettingsManager.Instance.GetBoolSettingValue("InstantText"))
        {
            StartCoroutine(InstantText_PlayTalkSound(TextVoiceEmitter.clip));
            if (storedreciever != null)
            {
                storedreciever.CurrentlyBeingUsed = true;
            }
            TextIsCurrentlyTyping = false;
            FinishCurrentText(TellRecieverIsFinished: false);
            yield break;
        }
        while (MaxVisibleCharacters < messageCharLength)
        {
            if (messageCharacters[MaxVisibleCharacters].ToString() == " ")
            {
                MaxVisibleCharacters++;
                yield return new WaitForSeconds(0.0265f * CurrentTextSpeedMultiplier);
            }
            else if (StoredText[MaxVisibleCharacters] == '<')
            {
                int num = MaxVisibleCharacters;
                int num2 = StoredText.IndexOf('>', num);
                if (num2 != -1)
                {
                    int num3 = num2 - num + 1;
                    MaxVisibleCharacters += num3;
                    Text_SubtractedRichText += num3;
                }
            }
            else if (messageCharacters[MaxVisibleCharacters].ToString() == "(")
            {
                MaxVisibleCharacters++;
                yield return new WaitForSeconds(0.0265f * CurrentTextSpeedMultiplier);
            }
            else if (messageCharacters[MaxVisibleCharacters].ToString() == ")")
            {
                MaxVisibleCharacters++;
                yield return new WaitForSeconds(0.0265f * CurrentTextSpeedMultiplier);
            }
            else if (StoredText[MaxVisibleCharacters].ToString() == "@")
            {
                StoredText = StoredText.Remove(MaxVisibleCharacters, 1);
                TextUI.text = StoredText;
                CurrentText = StoredText;
                messageCharacters = StoredText.ToCharArray();
                messageCharLength = StoredText.Length;
                yield return new WaitForSeconds(0.125f * CurrentTextSpeedMultiplier);
            }
            else if (StoredText[MaxVisibleCharacters].ToString() == "#")
            {
                StoredText = StoredText.Remove(MaxVisibleCharacters, 1);
                TextUI.text = StoredText;
                CurrentText = StoredText;
                messageCharacters = StoredText.ToCharArray();
                messageCharLength = StoredText.Length;
                yield return new WaitForSeconds(0.5f * CurrentTextSpeedMultiplier);
            }
            else if (StoredText[MaxVisibleCharacters].ToString() == ";")
            {
                yield return new WaitForSeconds(0.2f * CurrentTextSpeedMultiplier);
                StoredText = RemoveSelectedCharacter(StoredText, MaxVisibleCharacters);
                StoredText = AddSelectedCharacter(StoredText, MaxVisibleCharacters, "\n");
                TextUI.text = StoredText;
                CurrentText = StoredText;
                messageCharLength = StoredText.Length;
                if (storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Character[CurrentTextIndex] != null)
                {
                    if (storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Character[CurrentTextIndex].GiveCharacterBulletpoint)
                    {
                        TextBulletpointUI.text += "\n*";
                    }
                    else
                    {
                        TextBulletpointUI.text += "\n";
                    }
                }
                else
                {
                    TextBulletpointUI.text += "\n*";
                }
                MaxVisibleCharacters++;
            }
            else if (StoredText[MaxVisibleCharacters] == '|')
            {
                int startIndex = MaxVisibleCharacters + 1;
                int endIndex;
                for (endIndex = startIndex; endIndex < StoredText.Length && char.IsDigit(StoredText[endIndex]); endIndex++)
                {
                    yield return null;
                }
                if (int.TryParse(StoredText.Substring(startIndex, endIndex - startIndex), out var result) && result >= 0 && result < StoredAdditiveValues.Count)
                {
                    string value = StoredAdditiveValues[result];
                    StoredText = StoredText.Remove(MaxVisibleCharacters, endIndex - MaxVisibleCharacters);
                    StoredText = StoredText.Insert(MaxVisibleCharacters, value);
                    TextUI.text = StoredText;
                    CurrentText = StoredText;
                    messageCharLength = StoredText.Length;
                    messageCharacters = StoredText.ToCharArray();
                    MaxVisibleCharacters++;
                    yield return new WaitForSeconds(0.0265f * CurrentTextSpeedMultiplier);
                }
                else
                {
                    Debug.LogError("Invalid index after '|' or index out of range.");
                    MaxVisibleCharacters++;
                    yield return new WaitForSeconds(0.0265f * CurrentTextSpeedMultiplier);
                }
            }
            else if (StoredText[MaxVisibleCharacters].ToString() == "~")
            {
                yield return new WaitForSeconds(0.2f * CurrentTextSpeedMultiplier);
                StoredText = RemoveSelectedCharacter(StoredText, MaxVisibleCharacters);
                StoredText = AddSelectedCharacter(StoredText, MaxVisibleCharacters, "\n");
                TextUI.text = StoredText;
                CurrentText = StoredText;
                messageCharLength = StoredText.Length;
                if (storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Character[CurrentTextIndex] != null)
                {
                    if (storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Character[CurrentTextIndex].GiveCharacterBulletpoint)
                    {
                        TextBulletpointUI.text += "\n\n*";
                    }
                    else
                    {
                        TextBulletpointUI.text += "\n";
                    }
                }
                else
                {
                    TextBulletpointUI.text += "\n\n*";
                }
                MaxVisibleCharacters++;
            }
            else if (StoredText[MaxVisibleCharacters].ToString() == ",")
            {
                MaxVisibleCharacters++;
                TextUI.maxVisibleCharacters = MaxVisibleCharacters - Text_SubtractedRichText;
                yield return new WaitForSeconds(0.165f * CurrentTextSpeedMultiplier);
            }
            else if (StoredText[MaxVisibleCharacters].ToString() == ".")
            {
                MaxVisibleCharacters++;
                yield return new WaitForSeconds(0.0265f * CurrentTextSpeedMultiplier);
            }
            else if (StoredText[MaxVisibleCharacters].ToString() == "&")
            {
                StoredText = RemoveSelectedCharacter(StoredText, MaxVisibleCharacters);
                string input = Environment.UserName.ToUpper();
                input = Regex.Replace(input, " ", string.Empty);
                input = input.Substring(0, 6);
                StoredText = AddSelectedCharacter(StoredText, MaxVisibleCharacters, input);
                TextUI.text = StoredText;
                CurrentText = StoredText;
                messageCharLength = StoredText.Length;
                messageCharacters = StoredText.ToCharArray();
                yield return new WaitForSeconds(0f);
            }
            else if (StoredText[MaxVisibleCharacters].ToString() == "£")
            {
                CurrentText = "";
                TextUI.text = "";
                MaxVisibleCharacters = 0;
                TextUI.maxVisibleCharacters = 0;
                CurrentTextIndex++;
                StopCoroutine("PlayText");
                if (CurrentTextIndex + 1 < storedchatboxtext.Textboxes[StoredAfterIndex].Text.Length)
                {
                    if (storedreciever != null)
                    {
                        RunText(storedchatboxtext, StoredAfterIndex, storedreciever, ResetCurrentTextIndex: false);
                    }
                    else
                    {
                        RunText(storedchatboxtext, StoredAfterIndex, null, ResetCurrentTextIndex: false);
                    }
                }
                else
                {
                    EndText();
                }
            }
            else if (StoredText[MaxVisibleCharacters].ToString() == "*")
            {
                CurrentText = "";
                TextUI.text = "";
                MaxVisibleCharacters = 0;
                TextUI.maxVisibleCharacters = 0;
                CurrentTextIndex++;
                StopCoroutine("PlayText");
                if (CurrentTextIndex + 1 < storedchatboxtext.Textboxes[StoredAfterIndex].Text.Length)
                {
                    if (storedreciever != null)
                    {
                        RunText(storedchatboxtext, StoredAfterIndex, storedreciever, ResetCurrentTextIndex: false);
                    }
                    else
                    {
                        RunText(storedchatboxtext, StoredAfterIndex, null, ResetCurrentTextIndex: false);
                    }
                }
                else
                {
                    EndText();
                }
            }
            else if (!ForcedFinishText)
            {
                MaxVisibleCharacters++;
                TextVoiceEmitter.PlayOneShot(TextVoiceEmitter.clip);
                CallEvent_OnLetterTyped();
                yield return new WaitForSeconds(0.0265f * CurrentTextSpeedMultiplier);
            }
            else
            {
                MaxVisibleCharacters++;
            }
            TextUI.maxVisibleCharacters = MaxVisibleCharacters - Text_SubtractedRichText;
        }
        if (storedreciever != null)
        {
            storedreciever.CurrentlyBeingUsed = false;
        }
        TextIsCurrentlyTyping = false;
        StartShowingReactions();
        if (ChoiceNumber > 0)
        {
            ShowChoices();
        }
    }

    public void CallEvent_OnLetterTyped()
    {
        if (Event_OnLetterTyped != null)
        {
            Event_OnLetterTyped?.Invoke();
        }
    }

    private string RemoveSelectedCharacter(string text, int Index)
    {
        return text.Remove(Index, 1);
    }

    private string AddSelectedCharacter(string text, int Index, string Insert)
    {
        return text.Insert(Index, Insert);
    }

    public int CountOfStringInText(string TargetText, char Counter)
    {
        int num = 0;
        bool flag = false;
        char[] array = TargetText.ToCharArray();
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == Counter)
            {
                flag = true;
            }
            else if (false)
            {
                num++;
            }
        }
        return num;
    }

    public void EnableTextInput()
    {
        AllowInput = true;
    }

    public void DisableTextInput()
    {
        AllowInput = false;
    }

    public static string RemoveRichTextTags(string input)
    {
        string pattern = "<.*?>";
        return Regex.Replace(input, pattern, "");
    }

    public int CountRichTextTagCharacters(string input)
    {
        string pattern = "<.*?>";
        MatchCollection matchCollection = Regex.Matches(input, pattern);
        int num = 0;
        foreach (Match item in matchCollection)
        {
            num += item.Length;
        }
        return num;
    }

    private IEnumerator InstantText_PlayTalkSound(AudioClip talkSound)
    {
        if (talkSound != null && !Input.GetKey(PlayerInput.Instance.Key_Menu))
        {
            TextVoiceEmitter.clip = talkSound;
            for (int i = 0; i < 4; i++)
            {
                TextVoiceEmitter.PlayOneShot(TextVoiceEmitter.clip);
                CallEvent_OnLetterTyped();
                yield return new WaitForSeconds(0.03975f * CurrentTextSpeedMultiplier);
            }
        }
    }
}
