using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "hypothesis", menuName = "Deltaswap/Hypothesis", order = 1)]
public class Hypothesis : ScriptableObject
{
    public string HypothesisName;

    [SerializeField]
    public int HypothesisStartingScene;

    [TextArea]
    public string HypothesisTagline;

    [TextArea]
    public string HypothesisDescription;

    public Sprite HypothesisMenuSprite;

    public HypothesisGoal[] HypothesisGoals;

    public string TimeWhenWritten = "this Hypothetical was written before DELTARUNE\r\nChapters 3 & 4's release";

    public bool HypotheticalReleased;

    public CollectibleLog LogRewardedOnCompletion;

    public CollectibleLog LogRewardedOnGoalsCompleted;
}
