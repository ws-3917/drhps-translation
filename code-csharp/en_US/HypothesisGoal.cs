using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "hypothesisgoal", menuName = "Deltaswap/HypothesisGoal", order = 1)]
public class HypothesisGoal : ScriptableObject
{
    [TextArea]
    public string GoalHint;

    public int RequiredTasks;

    [Header("Will start as \"HypothesisGoal_\"")]
    public string GoalPlayprefName;

    [Header("Hypothetical this goal will be in, used for tracking logs")]
    public Hypothesis ParentHypothesis;
}
