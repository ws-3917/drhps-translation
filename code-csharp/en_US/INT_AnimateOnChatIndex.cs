using UnityEngine;

public class INT_AnimateOnChatIndex : MonoBehaviour
{
    public INT_Chat TargetChat;

    [Space(7.5f)]
    public Animator TargetAnimator;

    public string StateToPlay;

    public bool PlayStateWhenFinishedChat;

    public string StateToFinish;

    [Space(7.5f)]
    public int ChatTextIndexToPlay;

    [Header("For text that requires the player to interact multiple times")]
    public int AdditionalTextIndexToPlay;

    private bool CurrentActiveChatIsTarget;

    private int PreviousChatTextIndex = -1;

    private int PreviousAdditionalTextIndex = -1;

    private bool AbleToRevert;

    private void Update()
    {
        if (ChatboxManager.Instance.storedchatboxtext == TargetChat.Text)
        {
            CurrentActiveChatIsTarget = true;
        }
        else
        {
            CurrentActiveChatIsTarget = false;
            if (AbleToRevert)
            {
                TargetAnimator.Play(StateToFinish);
                AbleToRevert = false;
            }
        }
        if (CurrentActiveChatIsTarget && (ChatboxManager.Instance.CurrentTextIndex != PreviousChatTextIndex || ChatboxManager.Instance.CurrentAdditionalTextIndex != PreviousAdditionalTextIndex))
        {
            OnChatmanagerTextAdvance();
        }
    }

    private void OnChatmanagerTextAdvance()
    {
        if (ChatboxManager.Instance.CurrentTextIndex == ChatTextIndexToPlay && ChatboxManager.Instance.CurrentAdditionalTextIndex == AdditionalTextIndexToPlay)
        {
            TargetAnimator.Play(StateToPlay);
            AbleToRevert = true;
        }
    }
}
