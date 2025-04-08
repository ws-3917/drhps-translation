using System;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class BattleBubbleChatbox : MonoBehaviour
{
    public Transform TextboxObject;

    public TextMeshPro TextUI;

    public AudioSource TextVoiceEmitter;

    private string CurrentText = "";

    public int CurrentTextIndex;

    public int CurrentAdditionalTextIndex;

    private int Text_SubtractedRichText;

    private int StoredAfterIndex;

    public int PauseCounter;

    public CHATBOXTEXT storedchatboxtext;

    public CHATBOXTEXT previouschatboxtext;

    public bool ChatIsCurrentlyRunning;

    public TMP_FontAsset DefaultFont;

    public TMP_FontAsset DyslexicFont;

    public AudioClip DefaultSpeakSound;

    private float CurrentTextSpeedMultiplier;

    public bool FinishedShowingText;

    private string FormatCurrentText(string TargetText, bool IncludeBulletPoint, bool ActivateNextDialogueCharacter)
    {
        string text = TargetText.Replace("#", "").Replace("@", "").Replace(";", "\n")
            .Replace("~", "\n");
        if (text.Contains('£') && ActivateNextDialogueCharacter)
        {
            if (storedchatboxtext != null && storedchatboxtext.Textboxes[StoredAfterIndex] != null && CurrentTextIndex + 1 != storedchatboxtext.Textboxes[StoredAfterIndex].Text.Length)
            {
                CurrentTextIndex++;
                RunText(storedchatboxtext, CurrentTextIndex, StoredAfterIndex);
            }
            else
            {
                EndText();
            }
        }
        return text.Replace("£", "");
    }

    private void FinishCurrentText()
    {
        FinishedShowingText = true;
        TextUI.text = FormatCurrentText(CurrentText, IncludeBulletPoint: true, ActivateNextDialogueCharacter: true);
        Text_SubtractedRichText = CountRichTextTagCharacters(TextUI.text);
        TextUI.maxVisibleCharacters = TextUI.text.Length - Text_SubtractedRichText;
    }

    public void RunText(CHATBOXTEXT Chatbox, int textindex, int additionalindex)
    {
        StopCoroutine("PlayText");
        ChatIsCurrentlyRunning = true;
        CurrentTextIndex = textindex;
        CurrentAdditionalTextIndex = additionalindex;
        CurrentTextSpeedMultiplier = 1f;
        TextUI.color = Color.black;
        Text_SubtractedRichText = 0;
        FinishedShowingText = false;
        if (Chatbox.Textboxes[additionalindex].Character.Length != 0 && Chatbox.Textboxes[additionalindex].Character[CurrentTextIndex] == null)
        {
            TextUI.color = Color.black;
        }
        else
        {
            CurrentTextSpeedMultiplier = Chatbox.Textboxes[additionalindex].Character[CurrentTextIndex].TextSpeedMultiplier;
            if (Chatbox.Textboxes[additionalindex].Character[CurrentTextIndex].TextColor == Color.white)
            {
                TextUI.color = Color.black;
            }
            else
            {
                TextUI.color = Chatbox.Textboxes[additionalindex].Character[CurrentTextIndex].TextColor;
            }
        }
        if (Chatbox.Textboxes[additionalindex].Character[CurrentTextIndex] == null || Chatbox.Textboxes[additionalindex].Character[CurrentTextIndex].CharacterSound == null)
        {
            TextVoiceEmitter.clip = DefaultSpeakSound;
        }
        else
        {
            TextVoiceEmitter.clip = Chatbox.Textboxes[additionalindex].Character[CurrentTextIndex].CharacterSound;
        }
        TextUI.enabled = true;
        TextUI.text = "";
        StoredAfterIndex = additionalindex;
        storedchatboxtext = Chatbox;
        if (PlayerPrefs.GetInt("Setting_DyslexicText", 0) == 1)
        {
            TextUI.font = DyslexicFont;
        }
        else if (PlayerPrefs.GetInt("Setting_NoFont", 0) == 0)
        {
            if (Chatbox.Textboxes[additionalindex].Character[CurrentTextIndex] != null && Chatbox.Textboxes[additionalindex].Character[CurrentTextIndex].CharacterFont != null)
            {
                TextUI.font = Chatbox.Textboxes[additionalindex].Character[CurrentTextIndex].CharacterFont;
            }
            else
            {
                TextUI.font = DefaultFont;
            }
        }
        else
        {
            TextUI.font = DefaultFont;
        }
        if (Chatbox.Textboxes[additionalindex].Character[CurrentTextIndex] != null && Chatbox.Textboxes[additionalindex].Character[CurrentTextIndex].CharacterFontSize != 0f)
        {
            TextUI.fontSize = Chatbox.Textboxes[additionalindex].Character[CurrentTextIndex].CharacterFontSize;
        }
        else
        {
            TextUI.fontSize = 64f;
        }
        StartCoroutine("PlayText");
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

    public void EndText()
    {
        previouschatboxtext = storedchatboxtext;
        TextUI.enabled = false;
        StoredAfterIndex = 0;
        storedchatboxtext = null;
        CurrentText = "";
        TextUI.text = "";
        PauseCounter = 0;
        Text_SubtractedRichText = 0;
        CurrentTextIndex = 0;
        CurrentAdditionalTextIndex = 0;
        StopCoroutine("PlayText");
        ChatIsCurrentlyRunning = false;
    }

    private void OnDestroy()
    {
        if (storedchatboxtext != null && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex] != null && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Action.Length != 0 && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Action[CurrentTextIndex] != null && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Action[CurrentTextIndex].RunActionOnChatEnd)
        {
            AttemptRunActions();
        }
        AttemptRunSubActions(IsChatEnd: true);
    }

    private IEnumerator PlayText()
    {
        string StoredText = storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Text[CurrentTextIndex];
        TextUI.text = StoredText;
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
                MaxVisibleCharacters++;
                yield return new WaitForSeconds(0.2f * CurrentTextSpeedMultiplier);
            }
            else if (StoredText[MaxVisibleCharacters].ToString() == "~")
            {
                StoredText = RemoveSelectedCharacter(StoredText, MaxVisibleCharacters);
                StoredText = AddSelectedCharacter(StoredText, MaxVisibleCharacters, "\n");
                TextUI.text = StoredText;
                CurrentText = StoredText;
                messageCharLength = StoredText.Length;
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
                    RunText(storedchatboxtext, CurrentTextIndex, StoredAfterIndex);
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
        FinishedShowingText = true;
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
}
