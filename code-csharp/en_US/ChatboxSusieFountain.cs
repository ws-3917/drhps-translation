using System.Collections;
using TMPro;
using UnityEngine;

public class ChatboxSusieFountain : MonoBehaviour
{
    public TextMeshProUGUI TextUI;

    public AudioSource TextVoiceEmitter;

    private string CurrentText = "";

    public int CurrentTextIndex;

    public int CurrentAdditionalTextIndex;

    private int StoredAfterIndex;

    public int PauseCounter;

    public CHATBOXTEXT storedchatboxtext;

    public CHATBOXTEXT previouschatboxtext;

    public bool ChatIsCurrentlyRunning;

    private INT_Chat storedreciever;

    public TMP_FontAsset DefaultFont;

    public TMP_FontAsset DyslexicFont;

    public AudioClip DefaultSpeakSound;

    public Sprite DefaultIcon;

    private float CurrentTextSpeedMultiplier;

    public float FountainTextSpeedMarkiplier = 1f;

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
        if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.RightShift))
        {
            CurrentTextSpeedMultiplier = 0.33f * FountainTextSpeedMarkiplier;
        }
        else
        {
            CurrentTextSpeedMultiplier = 0.85f * FountainTextSpeedMarkiplier;
        }
    }

    private string FormatCurrentText(string TargetText, bool IncludeBulletPoint)
    {
        return TargetText.Replace("#", "").Replace("@", "").Replace(";", "\n")
            .Replace("~", "\n");
    }

    private void SetupChoices(CHATBOXTEXT chatbox, int index)
    {
    }

    private void CleanupOptions()
    {
    }

    private void SetHeartCursorPOS()
    {
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
        TextUI.enabled = true;
        TextUI.text = "";
        StoredAfterIndex = index;
        storedchatboxtext = Chatbox;
        if (Chatbox.Textboxes[index].Character[CurrentTextIndex] != null && Chatbox.Textboxes[index].Character[CurrentTextIndex].CharacterFont != null)
        {
            TextUI.font = Chatbox.Textboxes[index].Character[CurrentTextIndex].CharacterFont;
        }
        else
        {
            TextUI.font = DefaultFont;
        }
        if (Chatbox.Textboxes[index].Character[CurrentTextIndex] != null && Chatbox.Textboxes[index].Character[CurrentTextIndex].CharacterFontSize != 0f)
        {
            TextUI.fontSize = Chatbox.Textboxes[index].Character[CurrentTextIndex].CharacterFontSize;
        }
        else
        {
            TextUI.fontSize = 64f;
        }
        if (PlayerPrefs.GetInt("Setting_DyslexicText", 0) == 1)
        {
            TextUI.font = DyslexicFont;
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
        PauseCounter = 0;
        SetHeartCursorPOS();
        CurrentTextIndex = 0;
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
            else
            {
                if ((bool)storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Character[CurrentTextIndex].CharacterSound)
                {
                    TextVoiceEmitter.PlayOneShot(storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Character[CurrentTextIndex].CharacterSound);
                }
                if (!ForcedFinishText)
                {
                    TextUI.maxVisibleCharacters++;
                    yield return new WaitForSeconds(0.08f * CurrentTextSpeedMultiplier);
                }
                else
                {
                    TextUI.maxVisibleCharacters++;
                }
            }
        }
        if (CurrentTextSpeedMultiplier == 0.85f)
        {
            yield return new WaitForSeconds(2f * CurrentTextSpeedMultiplier);
        }
        else
        {
            yield return new WaitForSeconds(0.5f * CurrentTextSpeedMultiplier);
        }
        if (TextUI.maxVisibleCharacters == PauseCounter && storedchatboxtext != null)
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
            if (CurrentTextIndex < storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Text.Length - 1)
            {
                CurrentTextIndex++;
                RunText(storedchatboxtext, StoredAfterIndex, storedreciever, ResetCurrentTextIndex: false);
            }
            else
            {
                EndText();
            }
        }
        else
        {
            if (TextUI.maxVisibleCharacters != PauseCounter)
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
