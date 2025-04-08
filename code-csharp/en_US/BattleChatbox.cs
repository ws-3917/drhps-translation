using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleChatbox : MonoBehaviour
{
    public Transform TextboxObject;

    public Image CharacterPortrait;

    public TextMeshProUGUI TextUI;

    public TextMeshProUGUI TextBulletpointUI;

    public AudioSource TextVoiceEmitter;

    private string CurrentText = "";

    public int CurrentTextIndex;

    public int CurrentAdditionalTextIndex;

    private int Text_SubtractedRichText;

    private int StoredAfterIndex;

    public int PauseCounter;

    public bool FinishedShowingReactions;

    public CHATBOXTEXT storedchatboxtext;

    public CHATBOXTEXT previouschatboxtext;

    public bool ChatIsCurrentlyRunning;

    public bool AllowInput = true;

    private INT_Chat storedreciever;

    public List<string> StoredAdditiveValues = new List<string>();

    [SerializeField]
    private Transform ReactionHolder;

    public GameObject ReactionTemplate;

    private List<GameObject> StoredReactions = new List<GameObject>();

    public TMP_FontAsset DefaultFont;

    public TMP_FontAsset DyslexicFont;

    public Image[] ChatboxImages;

    public Sprite Kojima;

    public AudioClip DefaultSpeakSound;

    public Sprite DefaultIcon;

    public RectTransform[] Options;

    public RectTransform HeartCursor;

    public bool ChatboxInteractDebounce;

    private int ChoiceNumber;

    public int PreviousChosenChoiceIndex;

    private int cursorpos;

    private bool CurrentlyInChoice;

    private bool HideChoicesUntilFinish;

    private float CurrentTextSpeedMultiplier;

    public bool TextVisible = true;

    private float TalkIconTimer;

    private float TalkIconInterval = 1f / 6f;

    private static BattleChatbox instance;

    public static BattleChatbox Instance => instance;

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
        if (!GonerMenu.Instance.gonerMenuWasOpen && !GonerMenu.Instance.GonerMenuOpen)
        {
            ProcessInput();
        }
        TalkingIconProcess();
        SetHeartCursorPOS();
        if (StoredReactions.Count > 0 && !ChatIsCurrentlyRunning)
        {
            ClearReactions();
        }
        if (TextVisible)
        {
            TextUI.enabled = true;
            TextBulletpointUI.enabled = true;
            CharacterPortrait.enabled = true;
            SetVisibilityOfReactions(Visible: true);
        }
        else
        {
            TextUI.enabled = false;
            TextBulletpointUI.enabled = false;
            CharacterPortrait.enabled = false;
            SetVisibilityOfReactions(Visible: false);
        }
    }

    private void ProcessInput()
    {
        bool key = Input.GetKey(PlayerInput.Instance.Key_Menu);
        if (CurrentText != "" || CurrentText != null)
        {
            PauseCounter = FormatCurrentText(CurrentText, IncludeBulletPoint: false, ActivateNextDialogueCharacter: false, ConvertSpecialCharacters: false).Length - Text_SubtractedRichText;
        }
        if (CurrentlyInChoice && TextUI.maxVisibleCharacters == PauseCounter && HideChoicesUntilFinish && !HeartCursor.gameObject.activeSelf)
        {
            Options[0].gameObject.SetActive(value: true);
            Options[1].gameObject.SetActive(value: true);
            Options[2].gameObject.SetActive(value: true);
            Options[3].gameObject.SetActive(value: true);
            HeartCursor.gameObject.SetActive(value: true);
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Left) && CurrentlyInChoice && AllowInput && cursorpos - 1 > -1)
        {
            cursorpos--;
            SetHeartCursorPOS();
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Right) && CurrentlyInChoice && AllowInput && cursorpos + 1 <= ChoiceNumber - 1)
        {
            cursorpos++;
            SetHeartCursorPOS();
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm) && FinishedShowingReactions && CurrentlyInChoice && TextUI.maxVisibleCharacters == PauseCounter && AllowInput)
        {
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
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Cancel) && CurrentlyInChoice && AllowInput && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Choice[CurrentTextIndex] != null && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Choice[CurrentTextIndex].CanBackOut)
        {
            EndText();
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm) && !CurrentTextHasSkip(CurrentText) && FinishedShowingReactions && TextUI.maxVisibleCharacters == PauseCounter && !LightworldMenu.Instance.MenuOpen && !DarkworldMenu.Instance.MenuOpen && storedchatboxtext != null && !CurrentlyInChoice && AllowInput)
        {
            AttemptRunActions();
            AttemptRunSubActions(IsChatEnd: true);
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
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Cancel) && !CurrentTextHasSkip(CurrentText) && !DarkworldMenu.Instance.MenuOpen && !LightworldMenu.Instance.MenuOpen && TextUI.maxVisibleCharacters != PauseCounter && storedchatboxtext != null && AllowInput)
        {
            StopCoroutine("PlayText");
            FinishCurrentText();
        }
        if (!key || LightworldMenu.Instance.MenuOpen || !(storedchatboxtext != null) || !AllowInput)
        {
            return;
        }
        AttemptRunActions();
        AttemptRunSubActions(IsChatEnd: true);
        if (!CurrentlyInChoice)
        {
            if (storedchatboxtext.Textboxes[StoredAfterIndex] != null && CurrentTextIndex + 1 != storedchatboxtext.Textboxes[StoredAfterIndex].Text.Length)
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
        if (Text == null || Text == "")
        {
            return false;
        }
        if (Text.Contains('£'))
        {
            return true;
        }
        return false;
    }

    private string FormatCurrentText(string TargetText, bool IncludeBulletPoint, bool ActivateNextDialogueCharacter, bool ConvertSpecialCharacters = true)
    {
        string text = TargetText;
        while (text.Contains('|'))
        {
            int num = text.IndexOf('|');
            int num2 = num + 1;
            int i;
            for (i = num2; i < text.Length && char.IsDigit(text[i]); i++)
            {
            }
            if (int.TryParse(text.Substring(num2, i - num2), out var result) && result >= 0 && result < StoredAdditiveValues.Count)
            {
                string value = StoredAdditiveValues[result];
                text = text.Remove(num, i - num);
                text = text.Insert(num, value);
                continue;
            }
            Debug.LogError("Invalid index after '|' or index out of range.");
            break;
        }
        if (IncludeBulletPoint)
        {
            char[] array = text.ToCharArray();
            foreach (char num3 in array)
            {
                if (num3 == ';')
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
                if (num3 != '~')
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
        if (ConvertSpecialCharacters)
        {
            text = Regex.Replace(text, "(?<!\\=)#", "");
            text = text.Replace("@", "");
            text = text.Replace(";", "\n");
            text = text.Replace("~", "\n");
        }
        if (text.Contains('£') && ActivateNextDialogueCharacter)
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
        if (ConvertSpecialCharacters)
        {
            text = text.Replace("£", "");
        }
        return text;
    }

    private void FinishCurrentText()
    {
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
        if (storedreciever != null)
        {
            storedreciever.CurrentlyBeingUsed = false;
        }
        TextUI.text = FormatCurrentText(storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Text[CurrentTextIndex], IncludeBulletPoint: true, ActivateNextDialogueCharacter: true);
        StartShowingReactions();
        Text_SubtractedRichText = CountRichTextTagCharacters(TextUI.text);
        TextUI.maxVisibleCharacters = TextUI.text.Length - Text_SubtractedRichText;
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

    private void SetVisibilityOfReactions(bool Visible)
    {
        foreach (GameObject storedReaction in StoredReactions)
        {
            Image componentInChildren = storedReaction.GetComponentInChildren<Image>();
            TextMeshProUGUI componentInChildren2 = storedReaction.GetComponentInChildren<TextMeshProUGUI>();
            componentInChildren.enabled = Visible;
            componentInChildren2.enabled = Visible;
        }
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
        if (TextUI.maxVisibleCharacters < PauseCounter)
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
        if (cHATBOXACTION.RunComponentFunction)
        {
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
        }
        Debug.Log("AttemptRunActions ended.");
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
        if (chatbox.Textboxes[index].Choice[CurrentTextIndex] != null)
        {
            CurrentlyInChoice = true;
            CHATBOXCHOICE cHATBOXCHOICE = chatbox.Textboxes[index].Choice[CurrentTextIndex];
            ChoiceNumber = cHATBOXCHOICE.Choices.Count;
            SetHeartCursorPOS();
            float num = -82.5f;
            if (chatbox.Textboxes[index].Choice[CurrentTextIndex].ShowOnTextScrollFinish)
            {
                Options[0].gameObject.SetActive(value: false);
                Options[1].gameObject.SetActive(value: false);
                Options[2].gameObject.SetActive(value: false);
                Options[3].gameObject.SetActive(value: false);
                HeartCursor.gameObject.SetActive(value: false);
                HideChoicesUntilFinish = true;
            }
            else
            {
                Options[0].gameObject.SetActive(value: true);
                Options[1].gameObject.SetActive(value: true);
                Options[2].gameObject.SetActive(value: true);
                Options[3].gameObject.SetActive(value: true);
                HeartCursor.gameObject.SetActive(value: true);
                HideChoicesUntilFinish = false;
            }
            num = ((!(chatbox.Textboxes[index].Text[CurrentTextIndex] == "") && chatbox.Textboxes[index].Text[CurrentTextIndex] != null) ? (-82.5f) : 82.5f);
            if (ChoiceNumber == 2)
            {
                if (chatbox.Textboxes[index].Character[CurrentTextIndex] != null && chatbox.Textboxes[index].Character[CurrentTextIndex].CharacterIcon != null)
                {
                    Options[0].localPosition = new Vector2(-150f, num);
                    Options[0].GetComponent<TextMeshProUGUI>().text = cHATBOXCHOICE.Choices[0];
                    Options[1].localPosition = new Vector2(150f, num);
                    Options[1].GetComponent<TextMeshProUGUI>().text = cHATBOXCHOICE.Choices[1];
                    Options[2].localPosition = new Vector2(0f, 1500f);
                    Options[3].localPosition = new Vector2(0f, 1500f);
                }
                else
                {
                    Options[0].localPosition = new Vector2(-253.4f, num);
                    Options[0].GetComponent<TextMeshProUGUI>().text = cHATBOXCHOICE.Choices[0];
                    Options[1].localPosition = new Vector2(46.6f, num);
                    Options[1].GetComponent<TextMeshProUGUI>().text = cHATBOXCHOICE.Choices[1];
                    Options[2].localPosition = new Vector2(0f, 1500f);
                    Options[3].localPosition = new Vector2(0f, 1500f);
                }
            }
            else
            {
                _ = ChoiceNumber;
                _ = 2;
            }
            if (ChoiceNumber == 3)
            {
                Options[0].localPosition = new Vector2(-200f, num);
                Options[0].GetComponent<TextMeshProUGUI>().text = cHATBOXCHOICE.Choices[0];
                Options[1].localPosition = new Vector2(0f, num);
                Options[1].GetComponent<TextMeshProUGUI>().text = cHATBOXCHOICE.Choices[1];
                Options[2].localPosition = new Vector2(200f, num);
                Options[2].GetComponent<TextMeshProUGUI>().text = cHATBOXCHOICE.Choices[2];
                Options[3].localPosition = new Vector2(0f, 1500f);
            }
            if (ChoiceNumber == 4)
            {
                Options[0].localPosition = new Vector2(-400f, num);
                Options[0].GetComponent<TextMeshProUGUI>().text = cHATBOXCHOICE.Choices[0];
                Options[1].localPosition = new Vector2(-150f, num);
                Options[1].GetComponent<TextMeshProUGUI>().text = cHATBOXCHOICE.Choices[1];
                Options[2].localPosition = new Vector2(150f, num);
                Options[2].GetComponent<TextMeshProUGUI>().text = cHATBOXCHOICE.Choices[2];
                Options[3].localPosition = new Vector2(400f, num);
                Options[3].GetComponent<TextMeshProUGUI>().text = cHATBOXCHOICE.Choices[3];
            }
        }
        else
        {
            Options[0].localPosition = new Vector2(0f, 1500f);
            Options[1].localPosition = new Vector2(0f, 1500f);
            Options[2].localPosition = new Vector2(0f, 1500f);
            Options[3].localPosition = new Vector2(0f, 1500f);
        }
    }

    private void CleanupOptions()
    {
        Options[0].localPosition = new Vector2(0f, 1500f);
        Options[1].localPosition = new Vector2(0f, 1500f);
        Options[2].localPosition = new Vector2(0f, 1500f);
        Options[3].localPosition = new Vector2(0f, 1500f);
        HeartCursor.gameObject.SetActive(value: false);
    }

    private void SetHeartCursorPOS()
    {
        HeartCursor.localPosition = new Vector2(Options[cursorpos].localPosition.x - 35f, Options[cursorpos].localPosition.y);
        for (int i = 0; i < Options.Length; i++)
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
        if (ChoiceNumber < 0)
        {
            HeartCursor.localPosition = new Vector2(0f, 1500f);
        }
    }

    private void SetTextboxPosY(float PosY)
    {
        TextboxObject.localPosition = new Vector2(TextboxObject.localPosition.x, PosY);
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
        Image[] chatboxImages = ChatboxImages;
        for (int i = 0; i < chatboxImages.Length; i++)
        {
            chatboxImages[i].enabled = true;
        }
        if (Chatbox.Textboxes[index].Character.Length != 0 && Chatbox.Textboxes[index].Character[CurrentTextIndex] == null)
        {
            CharacterPortrait.sprite = DefaultIcon;
            TextUI.margin = new Vector4(120f, 95f, 120f, 10f);
            TextBulletpointUI.margin = new Vector4(60f, 95f, 1180f, 10f);
        }
        else
        {
            (CharacterPortrait.transform as RectTransform).sizeDelta = new Vector2(Chatbox.Textboxes[index].Character[CurrentTextIndex].CharacterIconWidth * 0.9f, Chatbox.Textboxes[index].Character[CurrentTextIndex].CharacterIconHeight * 0.9f);
            TextUI.margin = new Vector4(290f, 95f, 120f, 10f);
            TextBulletpointUI.margin = new Vector4(240f, 95f, 980f, 10f);
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
                TextUI.margin = new Vector4(120f, 95f, 120f, 10f);
                TextBulletpointUI.margin = new Vector4(60f, 95f, 1180f, 10f);
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
        cursorpos = 0;
        if (Camera.main != null && PlayerManager.Instance != null && storedreciever != null)
        {
            if (!storedreciever.ManualTextboxPosition)
            {
                if (PlayerManager.Instance.transform.position.y >= Camera.main.transform.position.y)
                {
                    SetTextboxPosY(-280f);
                }
                else
                {
                    SetTextboxPosY(280f);
                }
            }
            else if (storedreciever.OnBottom)
            {
                SetTextboxPosY(-280f);
            }
            else
            {
                SetTextboxPosY(280f);
            }
        }
        if (PlayerPrefs.GetInt("Setting_DyslexicText", 0) == 1)
        {
            TextUI.font = DyslexicFont;
            TextBulletpointUI.font = DefaultFont;
        }
        else if (PlayerPrefs.GetInt("Setting_NoFont", 0) == 0)
        {
            if (Chatbox.Textboxes[index].Character[CurrentTextIndex] != null && Chatbox.Textboxes[index].Character[CurrentTextIndex].CharacterFont != null)
            {
                TextUI.font = Chatbox.Textboxes[index].Character[CurrentTextIndex].CharacterFont;
                TextBulletpointUI.font = TextUI.font;
            }
            else
            {
                TextUI.font = DefaultFont;
                TextBulletpointUI.font = DefaultFont;
            }
        }
        else
        {
            TextUI.font = DefaultFont;
            TextBulletpointUI.font = DefaultFont;
        }
        if (Chatbox.Textboxes[index].Character[CurrentTextIndex] != null && Chatbox.Textboxes[index].Character[CurrentTextIndex].CharacterFontSize != 0f)
        {
            TextUI.fontSize = Chatbox.Textboxes[index].Character[CurrentTextIndex].CharacterFontSize;
        }
        else
        {
            TextUI.fontSize = 64f;
        }
        if (base.gameObject.activeInHierarchy)
        {
            StartCoroutine("PlayText");
        }
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
        }
        TextUI.enabled = false;
        StoredAfterIndex = 0;
        storedchatboxtext = null;
        CurrentText = "";
        TextUI.text = "";
        TextBulletpointUI.text = "";
        CharacterPortrait.enabled = false;
        CharacterPortrait.sprite = DefaultIcon;
        PauseCounter = 0;
        cursorpos = 0;
        ChoiceNumber = 0;
        Text_SubtractedRichText = 0;
        ChatIsCurrentlyRunning = false;
        StartCoroutine(DebounceDelay());
        ClearReactions();
        HeartCursor.gameObject.SetActive(value: false);
        SetHeartCursorPOS();
        CurrentTextIndex = 0;
        CurrentlyInChoice = false;
        Options[0].localPosition = new Vector2(0f, 1500f);
        Options[1].localPosition = new Vector2(0f, 1500f);
        Options[2].localPosition = new Vector2(0f, 1500f);
        Options[3].localPosition = new Vector2(0f, 1500f);
        CurrentAdditionalTextIndex = 0;
        if (storedreciever != null)
        {
            StartCoroutine(storedreciever.DebounceInteract());
            storedreciever.CurrentlyBeingUsed = false;
        }
        storedreciever = null;
        StopCoroutine("PlayText");
    }

    private IEnumerator DebounceDelay()
    {
        yield return new WaitForSeconds(0.01f);
        ChatboxInteractDebounce = false;
    }

    private IEnumerator PlayText()
    {
        string StoredText = storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Text[CurrentTextIndex];
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
                StoredText = RemoveSelectedCharacter(StoredText, MaxVisibleCharacters);
                TextUI.text = StoredText;
                CurrentText = StoredText;
                messageCharLength = StoredText.Length;
                yield return new WaitForSeconds(0.125f * CurrentTextSpeedMultiplier);
            }
            else if (StoredText[MaxVisibleCharacters].ToString() == "#")
            {
                StoredText = RemoveSelectedCharacter(StoredText, MaxVisibleCharacters);
                TextUI.text = StoredText;
                CurrentText = StoredText;
                messageCharLength = StoredText.Length;
                yield return new WaitForSeconds(0.5f * CurrentTextSpeedMultiplier);
            }
            else if (StoredText[MaxVisibleCharacters].ToString() == ";")
            {
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
                yield return new WaitForSeconds(0.2f * CurrentTextSpeedMultiplier);
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
                yield return new WaitForSeconds(0.2f * CurrentTextSpeedMultiplier);
            }
            else if (StoredText[MaxVisibleCharacters].ToString() == ",")
            {
                MaxVisibleCharacters++;
                yield return new WaitForSeconds(0.0265f * CurrentTextSpeedMultiplier);
            }
            else if (StoredText[MaxVisibleCharacters].ToString() == ".")
            {
                MaxVisibleCharacters++;
                yield return new WaitForSeconds(0.0265f * CurrentTextSpeedMultiplier);
            }
            else if (StoredText[MaxVisibleCharacters].ToString() == "&")
            {
                StoredText = RemoveSelectedCharacter(StoredText, MaxVisibleCharacters);
                StoredText = AddSelectedCharacter(StoredText, MaxVisibleCharacters, Environment.UserName.ToUpper());
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
            else if (!ForcedFinishText)
            {
                MaxVisibleCharacters++;
                TextVoiceEmitter.PlayOneShot(TextVoiceEmitter.clip);
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
        StartShowingReactions();
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

    private IEnumerator PlayTextBackup()
    {
        string StoredText = storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Text[CurrentTextIndex];
        TextUI.text = "* ";
        string currentText = CurrentText;
        for (int i = 0; i < currentText.Length; i++)
        {
            char c = currentText[i];
            if (storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Text[CurrentTextIndex] == StoredText)
            {
                if (c.ToString() == "#")
                {
                    yield return new WaitForSeconds(0.5f);
                    continue;
                }
                if (c.ToString() == "@")
                {
                    yield return new WaitForSeconds(0.125f);
                    continue;
                }
                if (c.ToString() == ",")
                {
                    TextUI.text += c;
                    yield return new WaitForSeconds(0.2f);
                    continue;
                }
                if (c.ToString() == ".")
                {
                    TextUI.text += c;
                    yield return new WaitForSeconds(0.02f);
                    continue;
                }
                if (c.ToString() == "?")
                {
                    TextUI.text += c;
                    yield return new WaitForSeconds(0.02f);
                    continue;
                }
                if (c.ToString() == "!")
                {
                    TextUI.text += c;
                    yield return new WaitForSeconds(0.02f);
                    continue;
                }
                if (c.ToString() == " ")
                {
                    TextUI.text += c;
                    yield return new WaitForSeconds(0.02f);
                    continue;
                }
                TextUI.text += c;
                TextVoiceEmitter.Stop();
                TextVoiceEmitter.PlayOneShot(TextVoiceEmitter.clip);
                yield return new WaitForSeconds(0.02f);
            }
        }
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
}
