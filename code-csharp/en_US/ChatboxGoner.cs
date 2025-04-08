using System.Collections;
using TMPro;
using UnityEngine;

public class ChatboxGoner : MonoBehaviour
{
    public Transform TextboxObject;

    public TextMeshProUGUI TextUI;

    public TextMeshProUGUI TextBulletpointUI;

    public AudioSource TextVoiceEmitter;

    private string CurrentText = "";

    public int CurrentTextIndex;

    public int CurrentAdditionalTextIndex;

    private int StoredAfterIndex;

    public int PauseCounter;

    public CHATBOXTEXT storedchatboxtext;

    public CHATBOXTEXT previouschatboxtext;

    public bool ChatIsCurrentlyRunning;

    public bool AllowInput = true;

    private INT_Chat storedreciever;

    public Transform SoulIcon;

    public Material HighlightedMaterial;

    public Material NotHighlightedMaterial;

    public TMP_FontAsset DefaultFont;

    public AudioClip DefaultSpeakSound;

    public Sprite DefaultIcon;

    public RectTransform[] Options;

    public RectTransform HeartCursor;

    private int ChoiceNumber;

    public int PreviousChosenChoiceIndex;

    private int cursorpos;

    private bool CurrentlyInChoice;

    private bool HideChoicesUntilFinish;

    private float CurrentTextSpeedMultiplier;

    private void Start()
    {
        SoulIcon.GetComponent<Animator>().speed = 666f;
    }

    private void Update()
    {
        ProcessInput();
        SetHeartCursorPOS();
    }

    private void ProcessInput()
    {
        if (CurrentText != "" || CurrentText != null)
        {
            PauseCounter = FormatCurrentText(CurrentText, IncludeBulletPoint: true).Length;
        }
        if (CurrentlyInChoice && TextUI.maxVisibleCharacters == PauseCounter && HideChoicesUntilFinish && !HeartCursor.gameObject.activeSelf)
        {
            Options[0].gameObject.SetActive(value: true);
            Options[1].gameObject.SetActive(value: true);
            HeartCursor.gameObject.SetActive(value: true);
            SoulIcon.gameObject.SetActive(value: true);
        }
        if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.RightShift))
        {
            CurrentTextSpeedMultiplier = 0.33f;
        }
        else
        {
            CurrentTextSpeedMultiplier = 0.85f;
        }
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && CurrentlyInChoice && AllowInput && cursorpos - 1 > -1)
        {
            cursorpos--;
            SetHeartCursorPOS();
        }
        if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && CurrentlyInChoice && AllowInput && cursorpos + 1 <= ChoiceNumber - 1)
        {
            cursorpos++;
            SetHeartCursorPOS();
        }
        if ((!Input.GetKeyDown(KeyCode.Z) && !Input.GetKeyDown(KeyCode.Return)) || !CurrentlyInChoice || TextUI.maxVisibleCharacters != PauseCounter || !AllowInput)
        {
            return;
        }
        if (storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Choice[CurrentTextIndex] != null && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Choice[CurrentTextIndex].ChoiceTextResults[cursorpos] == null)
        {
            PreviousChosenChoiceIndex = cursorpos;
            if (CurrentTextIndex + 1 != storedchatboxtext.Textboxes[StoredAfterIndex].Text.Length && storedchatboxtext != null)
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

    private string FormatCurrentText(string TargetText, bool IncludeBulletPoint)
    {
        return TargetText.Replace("#", "").Replace("@", "").Replace(";", "\n")
            .Replace("~", "\n");
    }

    private void SetupChoices(CHATBOXTEXT chatbox, int index)
    {
        if (chatbox.Textboxes[index].Choice[CurrentTextIndex] != null)
        {
            CurrentlyInChoice = true;
            CHATBOXCHOICE cHATBOXCHOICE = chatbox.Textboxes[index].Choice[CurrentTextIndex];
            ChoiceNumber = cHATBOXCHOICE.Choices.Count;
            SetHeartCursorPOS();
            if (chatbox.Textboxes[index].Choice[CurrentTextIndex].ShowOnTextScrollFinish)
            {
                Options[0].GetComponent<Animator>().Play("GonerMenu_OptionFadeIn");
                Options[1].GetComponent<Animator>().Play("GonerMenu_OptionFadeIn");
                SoulIcon.GetComponent<Animator>().Play("GonerMenu_HeartFadeIn");
                HideChoicesUntilFinish = true;
            }
            else
            {
                Options[0].GetComponent<Animator>().Play("GonerMenu_OptionFadeIn");
                Options[1].GetComponent<Animator>().Play("GonerMenu_OptionFadeIn");
                SoulIcon.GetComponent<Animator>().Play("GonerMenu_HeartFadeIn");
                HideChoicesUntilFinish = false;
            }
            if (ChoiceNumber == 2)
            {
                if (chatbox.Textboxes[index].Character[CurrentTextIndex] != null && chatbox.Textboxes[index].Character[CurrentTextIndex].CharacterIcon != null)
                {
                    Options[0].GetComponent<TextMeshProUGUI>().text = cHATBOXCHOICE.Choices[0];
                    Options[1].GetComponent<TextMeshProUGUI>().text = cHATBOXCHOICE.Choices[1];
                }
                else
                {
                    Options[0].GetComponent<TextMeshProUGUI>().text = cHATBOXCHOICE.Choices[0];
                    Options[1].GetComponent<TextMeshProUGUI>().text = cHATBOXCHOICE.Choices[1];
                }
            }
        }
        else
        {
            Options[0].GetComponent<Animator>().Play("GonerMenu_OptionFadeOut");
            Options[1].GetComponent<Animator>().Play("GonerMenu_OptionFadeOut");
            SoulIcon.GetComponent<Animator>().Play("GonerMenu_HeartFadeOut");
        }
    }

    private void CleanupOptions()
    {
        Options[0].GetComponent<Animator>().Play("GonerMenu_OptionFadeOut");
        Options[1].GetComponent<Animator>().Play("GonerMenu_OptionFadeOut");
        SoulIcon.GetComponent<Animator>().Play("GonerMenu_HeartFadeOut");
        SoulIcon.GetComponent<Animator>().speed = 1f;
    }

    private void SetHeartCursorPOS()
    {
        HeartCursor.localPosition = new Vector2(Options[cursorpos].localPosition.x - 90f, Options[cursorpos].localPosition.y);
        SoulIcon.position = Vector3.Lerp(SoulIcon.position, HeartCursor.position, 12f * Time.deltaTime);
        for (int i = 0; i < Options.Length; i++)
        {
            if (i != cursorpos)
            {
                Options[i].GetComponent<TextMeshProUGUI>().fontSharedMaterial = NotHighlightedMaterial;
                Options[i].GetComponent<TextMeshProUGUI>().color = Color.white;
            }
            else
            {
                Options[i].GetComponent<TextMeshProUGUI>().fontSharedMaterial = HighlightedMaterial;
                Options[i].GetComponent<TextMeshProUGUI>().color = Color.yellow;
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
        ChatIsCurrentlyRunning = true;
        CurrentAdditionalTextIndex = index;
        TextUI.color = Color.white;
        if (Chatbox.Textboxes[index].Choice[CurrentTextIndex] != null)
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
        cursorpos = 0;
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
        if (Chatbox.Textboxes[index].Character[CurrentTextIndex] != null && Chatbox.Textboxes[index].Character[CurrentTextIndex].CharacterFontSize != 0f)
        {
            TextUI.fontSize = Chatbox.Textboxes[index].Character[CurrentTextIndex].CharacterFontSize;
        }
        else
        {
            TextUI.fontSize = 64f;
        }
        StartCoroutine("PlayText");
    }

    public void EndText()
    {
        if (storedreciever != null)
        {
            storedreciever.FinishedText = true;
        }
        TextUI.enabled = false;
        StoredAfterIndex = 0;
        storedchatboxtext = null;
        CurrentText = "";
        TextBulletpointUI.text = "";
        PauseCounter = 0;
        cursorpos = 0;
        ChoiceNumber = 0;
        HeartCursor.gameObject.SetActive(value: false);
        SoulIcon.gameObject.SetActive(value: false);
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
        }
        storedreciever = null;
        StopCoroutine("PlayText");
        ChatIsCurrentlyRunning = false;
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
        bool ForcedFinishText = false;
        int messageCharLength = StoredText.Length;
        char[] messageCharacters = StoredText.ToCharArray();
        if (storedchatboxtext.Textboxes[CurrentAdditionalTextIndex] != null && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Action.Length != 0 && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Action[CurrentTextIndex] != null && !storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Action[CurrentTextIndex].RunActionOnChatEnd)
        {
            CHATBOXACTION cHATBOXACTION = storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Action[CurrentTextIndex];
            if (cHATBOXACTION.PlaySound && cHATBOXACTION.PossibleSounds.Length != 0)
            {
                AudioClip clip = cHATBOXACTION.PossibleSounds[Random.Range(0, cHATBOXACTION.PossibleSounds.Length)];
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
        while (TextUI.maxVisibleCharacters < messageCharLength)
        {
            if (messageCharacters[TextUI.maxVisibleCharacters].ToString() == " ")
            {
                TextUI.maxVisibleCharacters++;
                yield return new WaitForSeconds(0.08f * CurrentTextSpeedMultiplier);
            }
            else if (messageCharacters[TextUI.maxVisibleCharacters].ToString() == "(")
            {
                TextUI.maxVisibleCharacters++;
                yield return new WaitForSeconds(0.08f * CurrentTextSpeedMultiplier);
            }
            else if (messageCharacters[TextUI.maxVisibleCharacters].ToString() == ")")
            {
                TextUI.maxVisibleCharacters++;
                yield return new WaitForSeconds(0.08f * CurrentTextSpeedMultiplier);
            }
            else if (StoredText[TextUI.maxVisibleCharacters].ToString() == "@")
            {
                StoredText = RemoveSelectedCharacter(StoredText, TextUI.maxVisibleCharacters);
                TextUI.text = StoredText;
                CurrentText = StoredText;
                messageCharLength = StoredText.Length;
                yield return new WaitForSeconds(0.125f * CurrentTextSpeedMultiplier);
            }
            else if (StoredText[TextUI.maxVisibleCharacters].ToString() == "#")
            {
                StoredText = RemoveSelectedCharacter(StoredText, TextUI.maxVisibleCharacters);
                TextUI.text = StoredText;
                CurrentText = StoredText;
                messageCharLength = StoredText.Length;
                yield return new WaitForSeconds(0.8f * CurrentTextSpeedMultiplier);
            }
            else if (StoredText[TextUI.maxVisibleCharacters].ToString() == ";")
            {
                StoredText = RemoveSelectedCharacter(StoredText, TextUI.maxVisibleCharacters);
                StoredText = AddSelectedCharacter(StoredText, TextUI.maxVisibleCharacters, "\n");
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
                TextUI.maxVisibleCharacters++;
                yield return new WaitForSeconds(0.8f * CurrentTextSpeedMultiplier);
            }
            else if (StoredText[TextUI.maxVisibleCharacters].ToString() == "~")
            {
                StoredText = RemoveSelectedCharacter(StoredText, TextUI.maxVisibleCharacters);
                StoredText = AddSelectedCharacter(StoredText, TextUI.maxVisibleCharacters, "\n");
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
                TextUI.maxVisibleCharacters++;
                yield return new WaitForSeconds(0.8f * CurrentTextSpeedMultiplier);
            }
            else if (StoredText[TextUI.maxVisibleCharacters].ToString() == ",")
            {
                TextUI.maxVisibleCharacters++;
                yield return new WaitForSeconds(0.2f * CurrentTextSpeedMultiplier);
            }
            else if (StoredText[TextUI.maxVisibleCharacters].ToString() == ".")
            {
                TextUI.maxVisibleCharacters++;
                yield return new WaitForSeconds(0.08f * CurrentTextSpeedMultiplier);
            }
            else if (StoredText[TextUI.maxVisibleCharacters].ToString() == "<")
            {
                TextUI.maxVisibleCharacters++;
            }
            else if (StoredText[TextUI.maxVisibleCharacters].ToString() == ">")
            {
                TextUI.maxVisibleCharacters++;
            }
            else if (StoredText[TextUI.maxVisibleCharacters].ToString() == "£")
            {
                StoredText = RemoveSelectedCharacter(StoredText, TextUI.maxVisibleCharacters);
                TextUI.text = StoredText;
                CurrentText = StoredText;
                messageCharLength = StoredText.Length;
                ForcedFinishText = true;
            }
            else if (!ForcedFinishText)
            {
                TextUI.maxVisibleCharacters++;
                yield return new WaitForSeconds(0.08f * CurrentTextSpeedMultiplier);
            }
            else
            {
                TextUI.maxVisibleCharacters++;
            }
        }
        if (CurrentTextSpeedMultiplier == 0.85f)
        {
            yield return new WaitForSeconds(2f);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }
        if (TextUI.maxVisibleCharacters == PauseCounter && storedchatboxtext != null && !CurrentlyInChoice)
        {
            if (storedchatboxtext.Textboxes[CurrentAdditionalTextIndex] != null && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Action.Length != 0 && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Action[CurrentTextIndex] != null && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Action[CurrentTextIndex].RunActionOnChatEnd)
            {
                CHATBOXACTION cHATBOXACTION2 = storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Action[CurrentTextIndex];
                if (cHATBOXACTION2.PlaySound && cHATBOXACTION2.PossibleSounds.Length != 0)
                {
                    AudioClip clip2 = cHATBOXACTION2.PossibleSounds[Random.Range(0, cHATBOXACTION2.PossibleSounds.Length)];
                    TextVoiceEmitter.PlayOneShot(clip2);
                }
                GameObject gameObject2 = GameObject.Find(cHATBOXACTION2.TargetComponentGameObjectName);
                if (cHATBOXACTION2.RunComponentFunction && gameObject2 != null && cHATBOXACTION2.FunctionName != null)
                {
                    string targetComponentClassname2 = cHATBOXACTION2.TargetComponentClassname;
                    Component component2 = gameObject2.GetComponent(targetComponentClassname2);
                    if (component2 != null)
                    {
                        if (component2.GetType().GetMethod(cHATBOXACTION2.FunctionName) != null)
                        {
                            component2.GetType().GetMethod(cHATBOXACTION2.FunctionName).Invoke(component2, null);
                        }
                        else
                        {
                            MonoBehaviour.print("did you forget to make the method public?");
                        }
                    }
                    else
                    {
                        MonoBehaviour.print("Couldn't find Component named: " + targetComponentClassname2);
                    }
                }
            }
            CurrentTextIndex++;
            RunText(storedchatboxtext, StoredAfterIndex, storedreciever, ResetCurrentTextIndex: false);
        }
        else
        {
            if (TextUI.maxVisibleCharacters != PauseCounter || CurrentlyInChoice)
            {
                yield break;
            }
            if (storedchatboxtext != null && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex] != null && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Action.Length != 0 && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Action[CurrentTextIndex] != null && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Action[CurrentTextIndex].RunActionOnChatEnd)
            {
                CHATBOXACTION cHATBOXACTION3 = storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Action[CurrentTextIndex];
                if (cHATBOXACTION3.PlaySound && cHATBOXACTION3.PossibleSounds.Length != 0)
                {
                    AudioClip clip3 = cHATBOXACTION3.PossibleSounds[Random.Range(0, cHATBOXACTION3.PossibleSounds.Length)];
                    TextVoiceEmitter.PlayOneShot(clip3);
                }
                GameObject gameObject3 = GameObject.Find(cHATBOXACTION3.TargetComponentGameObjectName);
                if (cHATBOXACTION3.RunComponentFunction && gameObject3 != null && cHATBOXACTION3.FunctionName != null)
                {
                    string targetComponentClassname3 = cHATBOXACTION3.TargetComponentClassname;
                    Component component3 = gameObject3.GetComponent(targetComponentClassname3);
                    if (component3 != null)
                    {
                        if (component3.GetType().GetMethod(cHATBOXACTION3.FunctionName) != null)
                        {
                            component3.GetType().GetMethod(cHATBOXACTION3.FunctionName).Invoke(component3, null);
                        }
                        else
                        {
                            MonoBehaviour.print("did you forget to make the method public?");
                        }
                    }
                    else
                    {
                        MonoBehaviour.print("Couldn't find Component named: " + targetComponentClassname3);
                    }
                }
            }
            EndText();
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
                }
                else if (c.ToString() == "@")
                {
                    yield return new WaitForSeconds(0.125f);
                }
                else if (c.ToString() == ",")
                {
                    TextUI.text += c;
                    yield return new WaitForSeconds(0.2f);
                }
                else if (c.ToString() == ".")
                {
                    TextUI.text += c;
                    yield return new WaitForSeconds(0.02f);
                }
                else if (c.ToString() == "?")
                {
                    TextUI.text += c;
                    yield return new WaitForSeconds(0.02f);
                }
                else if (c.ToString() == "!")
                {
                    TextUI.text += c;
                    yield return new WaitForSeconds(0.02f);
                }
                else if (c.ToString() == " ")
                {
                    TextUI.text += c;
                    yield return new WaitForSeconds(0.02f);
                }
                else
                {
                    TextUI.text += c;
                    TextVoiceEmitter.PlayOneShot(TextVoiceEmitter.clip);
                    yield return new WaitForSeconds(0.02f);
                }
            }
        }
    }
}
