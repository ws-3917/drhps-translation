using UnityEngine;

public class INT_RunComponentOnChatIndex : MonoBehaviour
{
    public INT_Chat TargetChatStarter;

    public CHATBOXTEXT TargetChat;

    public ChatboxManager ChatManager;

    [Space(7.5f)]
    [Header("WARNING! This component is removed once ran")]
    public Component TargetComponent;

    [Space(5f)]
    public string MethodName;

    [Space(7.5f)]
    public int ChatTextIndexToPlay;

    [Header("For text that requires the player to interact multiple times")]
    public int AdditionalTextIndexToPlay;

    private bool CurrentActiveChatIsTarget;

    private bool hasRan;

    private void Update()
    {
        if (ChatManager.storedchatboxtext == TargetChat)
        {
            CurrentActiveChatIsTarget = true;
        }
        if (CurrentActiveChatIsTarget && ChatManager.CurrentTextIndex == ChatTextIndexToPlay && ChatManager.CurrentAdditionalTextIndex == AdditionalTextIndexToPlay && TargetChatStarter.FinishedText && !hasRan && TargetComponent != null && TargetComponent.GetType().GetMethod(MethodName) != null)
        {
            hasRan = true;
            TargetComponent.GetType().GetMethod(MethodName).Invoke(TargetComponent, null);
            Object.Destroy(this, 0.5f);
        }
    }
}
