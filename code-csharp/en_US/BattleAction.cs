using UnityEngine;

[CreateAssetMenu(fileName = "BattleAction", menuName = "Deltaswap/BattleAction", order = 0)]
public class BattleAction : ScriptableObject
{
    public enum BattleActionTarget
    {
        Enemy = 0,
        PartyMember = 1,
        Nobody = 2
    }

    public PartyMember TargetPartyMember;

    public string ActionName = "Do Something";

    [Header("NOTE, Kris acts always have blank descriptions")]
    public string ActionDescription = "";

    public int TPRequired;

    [Header("What this action targets")]
    public BattleActionTarget ActionTarget;

    public string AnimationToAction = "PrepareAct";

    [Space(10f)]
    [Header("This is the function that the battle script will run")]
    [Header("Use > in function name to call method in BattleSystem instead")]
    public string BattleScript_FunctionToRun = "Function";

    [Header("Keep empty to not require any")]
    public PartyMember[] RequiredPartyMembers;
}
