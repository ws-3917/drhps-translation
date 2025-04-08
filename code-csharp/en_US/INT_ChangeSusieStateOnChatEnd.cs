using UnityEngine;

public class INT_ChangeSusieStateOnChatEnd : MonoBehaviour
{
    [Header("WARNING! This component is removed once ran")]
    public INT_Chat TargetChat;

    public Susie_Follower Susie;

    public bool AllowSusieFollow;

    private void Update()
    {
        if (TargetChat.FinishedText)
        {
            Susie.FollowingEnabled = AllowSusieFollow;
            Object.Destroy(this);
        }
    }
}
