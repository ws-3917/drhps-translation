using System.Collections;
using UnityEngine;

public class INT_Chat : MonoBehaviour
{
    public CHATBOXTEXT Text;

    private ChatboxManager CHATManager;

    private PlayerManager PLAYERManager;

    [Space(5f)]
    [Header("Chat Variables")]
    public int CurrentIndex;

    [HideInInspector]
    public bool FinishedText;

    public bool CanUse = true;

    public bool CurrentlyBeingUsed;

    [HideInInspector]
    public bool FirstTextPlayed;

    [Space(5f)]
    public bool LoopCertainText;

    public int IndexToLoop;

    [Space(5f)]
    public bool ManualTextboxPosition;

    public bool OnBottom;

    private void Start()
    {
        CHATManager = ChatboxManager.Instance;
        PLAYERManager = PlayerManager.Instance;
    }

    public void RUN()
    {
        if (CanUse && !CHATManager.ChatIsCurrentlyRunning)
        {
            if (!FirstTextPlayed)
            {
                FirstTextPlayed = true;
            }
            else if (!LoopCertainText)
            {
                CurrentIndex++;
            }
            else if (CurrentIndex < IndexToLoop)
            {
                CurrentIndex++;
            }
            if (CurrentIndex < Text.Textboxes.Length)
            {
                CanUse = false;
                CHATManager.RunText(Text, CurrentIndex, this, ResetCurrentTextIndex: false);
                PLAYERManager._PlayerState = PlayerManager.PlayerState.Cutscene;
            }
        }
    }

    public IEnumerator DebounceInteract()
    {
        yield return new WaitForSeconds(0.3f);
        CanUse = true;
    }
}
