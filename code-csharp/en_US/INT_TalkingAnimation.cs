using System.Collections.Generic;
using UnityEngine;

public class INT_TalkingAnimation : MonoBehaviour
{
    public INT_Chat TargetChat;

    [SerializeField]
    private Animator TargetAnimator;

    [Header("Used for more basic NPCs, that don't usually move")]
    [SerializeField]
    private string TalkAnimationName;

    [SerializeField]
    private string IdleAnimationName;

    [Header("For more advanced NPCs like Papyrus or Toriel")]
    [SerializeField]
    private bool UseTalkBool;

    [SerializeField]
    private string TalkBoolName;

    [Header("Listens to chatboxmanager instead of int_chat, so it's global")]
    [SerializeField]
    private bool ListenDialogueGlobally;

    [Header("Filter to only listen for specific characters, empty = every char valid")]
    [Header("Filter to only listen for specific characters, empty = every char valid")]
    [SerializeField]
    private List<CHATBOXCHAR> CharactersToListen = new List<CHATBOXCHAR>();

    private void Update()
    {
        bool flag = CharactersToListen.Count == 0 || CurrentCharacterValid();
        if (!ListenDialogueGlobally)
        {
            if (TargetChat.CurrentlyBeingUsed && flag)
            {
                if (TalkAnimationName != null && !UseTalkBool)
                {
                    TargetAnimator.Play(TalkAnimationName);
                }
                if (TalkBoolName != null && UseTalkBool)
                {
                    TargetAnimator.SetBool(TalkBoolName, value: true);
                }
            }
            else
            {
                if (IdleAnimationName != null && !UseTalkBool)
                {
                    TargetAnimator.Play(IdleAnimationName);
                }
                if (TalkBoolName != null && UseTalkBool)
                {
                    TargetAnimator.SetBool(TalkBoolName, value: false);
                }
            }
        }
        else if (ChatboxManager.Instance.TextIsCurrentlyTyping && ChatboxManager.Instance.storedchatboxtext != null && ChatboxManager.Instance.storedchatboxtext.Textboxes[ChatboxManager.Instance.CurrentAdditionalTextIndex].Character[ChatboxManager.Instance.CurrentTextIndex] != null && ChatboxManager.Instance.storedchatboxtext.Textboxes[ChatboxManager.Instance.CurrentAdditionalTextIndex].Character[ChatboxManager.Instance.CurrentTextIndex].TellRecieverIfChatting && flag)
        {
            if (TalkAnimationName != null && !UseTalkBool)
            {
                TargetAnimator.Play(TalkAnimationName);
            }
            if (TalkBoolName != null && UseTalkBool)
            {
                TargetAnimator.SetBool(TalkBoolName, value: true);
            }
        }
        else
        {
            if (IdleAnimationName != null && !UseTalkBool)
            {
                TargetAnimator.Play(IdleAnimationName);
            }
            if (TalkBoolName != null && UseTalkBool)
            {
                TargetAnimator.SetBool(TalkBoolName, value: false);
            }
        }
    }

    private bool CurrentCharacterValid()
    {
        if (ChatboxManager.Instance.storedchatboxtext != null && ChatboxManager.Instance.storedchatboxtext.Textboxes[ChatboxManager.Instance.CurrentAdditionalTextIndex].Character[ChatboxManager.Instance.CurrentTextIndex] != null && ChatboxManager.Instance.storedchatboxtext.Textboxes[ChatboxManager.Instance.CurrentAdditionalTextIndex].Character[ChatboxManager.Instance.CurrentTextIndex].TellRecieverIfChatting)
        {
            return CharactersToListen.Contains(ChatboxManager.Instance.storedchatboxtext.Textboxes[ChatboxManager.Instance.CurrentAdditionalTextIndex].Character[ChatboxManager.Instance.CurrentTextIndex]);
        }
        return false;
    }
}
