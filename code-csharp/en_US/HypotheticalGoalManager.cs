using UnityEngine;
using UnityEngine.UI;

public class HypotheticalGoalManager : MonoBehaviour
{
    private static HypotheticalGoalManager instance;

    [SerializeField]
    private Animator GoalCheckmarkAnimator;

    [SerializeField]
    private AudioSource GoalSource;

    [SerializeField]
    private AudioClip GoalCompleteSound;

    [SerializeField]
    private RawImage GoalCheckmark_LogPreview;

    public static HypotheticalGoalManager Instance => instance;

    public void CompleteGoal(HypothesisGoal goal, bool PlaySound = true)
    {
        int requiredTasks = goal.RequiredTasks;
        string key = "HypothesisGoal_" + goal.GoalPlayprefName;
        int @int = PlayerPrefs.GetInt(key, 0);
        PlayerPrefs.SetInt(key, requiredTasks);
        Debug.Log($"Goal: {goal.GoalPlayprefName}, Current: {@int}, Required: {requiredTasks}");
        if (@int >= requiredTasks)
        {
            Debug.Log("Goal already completed.");
            return;
        }
        bool flag = true;
        HypothesisGoal[] hypothesisGoals = goal.ParentHypothesis.HypothesisGoals;
        foreach (HypothesisGoal hypothesisGoal in hypothesisGoals)
        {
            int int2 = PlayerPrefs.GetInt("HypothesisGoal_" + hypothesisGoal.GoalPlayprefName, 0);
            Debug.Log($"Checking goal {hypothesisGoal.GoalPlayprefName}: {int2}/{hypothesisGoal.RequiredTasks}");
            if (int2 < hypothesisGoal.RequiredTasks)
            {
                flag = false;
            }
        }
        if (flag)
        {
            Debug.Log("All goals completed!");
            if (goal.ParentHypothesis.LogRewardedOnGoalsCompleted != null)
            {
                CollectibleLog logRewardedOnGoalsCompleted = goal.ParentHypothesis.LogRewardedOnGoalsCompleted;
                PlayerPrefs.SetInt(logRewardedOnGoalsCompleted.LogPlayerPrefName, 1);
                GoalCheckmark_LogPreview.texture = logRewardedOnGoalsCompleted.LogPreview.texture;
                GoalCheckmark_LogPreview.rectTransform.sizeDelta = logRewardedOnGoalsCompleted.LogPreviewScale / 5f;
                GoalCheckmarkAnimator.Play("ContextGoal_GoalComplete_NewLog", -1, 0f);
            }
            else
            {
                GoalCheckmarkAnimator.Play("ContextGoal_GoalComplete", -1, 0f);
            }
        }
        else
        {
            Debug.Log("Not all goals completed.");
            GoalCheckmarkAnimator.Play("ContextGoal_GoalComplete", -1, 0f);
        }
        if (PlaySound)
        {
            GoalSource.PlayOneShot(GoalCompleteSound);
        }
    }

    public void IncrementGoal(HypothesisGoal goal, int amount)
    {
        int requiredTasks = goal.RequiredTasks;
        int @int = PlayerPrefs.GetInt("HypothesisGoal_" + goal.GoalPlayprefName, 0);
        int num = @int;
        if (@int + amount >= requiredTasks)
        {
            num = requiredTasks;
            CompleteGoal(goal);
        }
        else
        {
            num = @int + amount;
        }
        PlayerPrefs.SetInt("HypothesisGoal_" + goal.GoalPlayprefName, num);
    }

    public void IncrementGoal_Silent(HypothesisGoal goal, int amount)
    {
        int requiredTasks = goal.RequiredTasks;
        int @int = PlayerPrefs.GetInt("HypothesisGoal_" + goal.GoalPlayprefName, 0);
        int num = @int;
        if (@int + amount >= requiredTasks)
        {
            num = requiredTasks;
            CompleteGoal(goal, PlaySound: false);
        }
        else
        {
            num = @int + amount;
        }
        PlayerPrefs.SetInt("HypothesisGoal_" + goal.GoalPlayprefName, num);
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Object.Destroy(base.gameObject);
        }
        else
        {
            instance = this;
        }
    }
}
