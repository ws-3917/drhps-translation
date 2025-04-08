using System.Collections;
using UnityEngine;

public class INT_NextINTCHATindex : MonoBehaviour
{
    public INT_Chat Chat;

    public int ChatIndexToRun;

    public int SetIndexTo;

    public bool RanChat;

    private void Update()
    {
        if (Chat.FinishedText && Chat.CurrentIndex == ChatIndexToRun && !RanChat)
        {
            RanChat = true;
            Chat.CurrentIndex = SetIndexTo;
            StartCoroutine(ChatDelay());
        }
    }

    private IEnumerator ChatDelay()
    {
        yield return new WaitForSeconds(0.02f);
        Chat.RUN();
    }
}
