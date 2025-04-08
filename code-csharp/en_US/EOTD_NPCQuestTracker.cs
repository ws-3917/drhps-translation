using UnityEngine;

public class EOTD_NPCQuestTracker : MonoBehaviour
{
    public INT_Chat Interact;

    public string NPCName;

    [SerializeField]
    private HypothesisGoal Goal;

    [SerializeField]
    private int PreviouslyUsed;

    private bool FinishedUsing;

    private void Start()
    {
        PreviouslyUsed = PlayerPrefs.GetInt(NPCName + "_Interacted", 0);
    }

    private void Update()
    {
        if (!FinishedUsing && PreviouslyUsed == 0 && Interact.FirstTextPlayed)
        {
            HypotheticalGoalManager.Instance.IncrementGoal(Goal, 1);
            FinishedUsing = true;
            PlayerPrefs.SetInt(NPCName + "_Interacted", 1);
        }
    }
}
