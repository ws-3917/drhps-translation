using System.Collections;
using System.Reflection;
using TMPro;
using UnityEngine;

public class ChatboxSimpleText : MonoBehaviour
{
    public Transform TextboxObject;

    public TextMeshProUGUI TextUI;

    public AudioSource TextVoiceEmitter;

    public int CurrentTextIndex;

    public int CurrentAdditionalTextIndex;

    public int PauseCounter;

    public CHATBOXTEXT storedchatboxtext;

    public bool ChatIsCurrentlyRunning;

    public TMP_FontAsset DefaultFont;

    public AudioClip DefaultSpeakSound;

    public Sprite DefaultIcon;

    private float CurrentTextSpeedMultiplier;

    public void RunText(CHATBOXTEXT Chatbox, int index)
    {
        StopCoroutine("PlayText");
        ChatIsCurrentlyRunning = true;
        CurrentAdditionalTextIndex = index;
        CurrentTextSpeedMultiplier = 1f;
        TextUI.color = Color.white;
        if (Chatbox.Textboxes[index].Character[CurrentTextIndex] == null || Chatbox.Textboxes[index].Character[CurrentTextIndex].CharacterSound == null)
        {
            TextVoiceEmitter.clip = DefaultSpeakSound;
        }
        else
        {
            TextVoiceEmitter.clip = Chatbox.Textboxes[index].Character[CurrentTextIndex].CharacterSound;
        }
        TextUI.enabled = true;
        TextUI.text = "";
        storedchatboxtext = Chatbox;
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
        TextUI.enabled = false;
        storedchatboxtext = null;
        PauseCounter = 0;
        CurrentTextIndex = 0;
        CurrentAdditionalTextIndex = 0;
        StopCoroutine("PlayText");
        ChatIsCurrentlyRunning = false;
    }

    private IEnumerator PlayText()
    {
        string text = storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Text[CurrentTextIndex];
        text = text.Replace(';', '\n');
        TextUI.text = text;
        TextUI.maxVisibleCharacters = 0;
        AudioClip talkClip = ((!(storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Character[CurrentTextIndex] != null) || !(storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Character[CurrentTextIndex].CharacterSound != null)) ? DefaultSpeakSound : storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Character[CurrentTextIndex].CharacterSound);
        if (storedchatboxtext != null && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex] != null && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Action.Length != 0 && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Action[CurrentTextIndex] != null && !storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Action[CurrentTextIndex].RunActionOnChatEnd)
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
        AttemptRunSubActions();
        AttemptRunMultipleActions();
        int messageCharLength = text.Length;
        char[] messageCharacters = text.ToCharArray();
        while (TextUI.maxVisibleCharacters < messageCharLength)
        {
            char c = messageCharacters[TextUI.maxVisibleCharacters];
            if (c == ' ' || c == '(' || c == ')')
            {
                TextUI.maxVisibleCharacters++;
                yield return new WaitForSeconds(0.08f * CurrentTextSpeedMultiplier);
            }
            else
            {
                yield return new WaitForSeconds(0.08f * CurrentTextSpeedMultiplier);
                TextVoiceEmitter.PlayOneShot(talkClip);
                TextUI.maxVisibleCharacters++;
            }
        }
        if (storedchatboxtext != null && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex] != null && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Action.Length != 0 && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Action[CurrentTextIndex] != null && storedchatboxtext.Textboxes[CurrentAdditionalTextIndex].Action[CurrentTextIndex].RunActionOnChatEnd)
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
        AttemptRunSubActions(IsChatEnd: true);
        AttemptRunMultipleActions(IsChatEnd: true);
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
            AudioClip audioClip = cHATBOXACTION.PossibleSounds[Random.Range(0, cHATBOXACTION.PossibleSounds.Length)];
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
                    AudioClip audioClip = cHATBOXACTION.PossibleSounds[Random.Range(0, cHATBOXACTION.PossibleSounds.Length)];
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
}
