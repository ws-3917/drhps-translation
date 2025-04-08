using UnityEngine;

[CreateAssetMenu(fileName = "BATTLE", menuName = "Deltaswap/Battle/Battle", order = 0)]
public class Battle : ScriptableObject
{
    [Header("- Setup -")]
    public PartyMember[] PartyMembers;

    [Space(5f)]
    public BattleEnemy[] Enemies;

    [Space(5f)]
    public GameObject BattleScriptPrefab;

    public string BattleScriptMainComponent;

    [Space(10f)]
    [Header("Warning! Must match PartyMembers")]
    public Vector2[] PartyMemberPositions = new Vector2[3]
    {
        new Vector2(-5.25f, 2.85f),
        new Vector2(-5.25f, 0.65f),
        new Vector2(-5.25f, -1.4f)
    };

    [Space(5f)]
    [Header("Warning! Must match Enemies")]
    public Vector2[] EnemyPositions = new Vector2[3]
    {
        new Vector2(3.3f, 2.85f),
        new Vector2(4.85f, 0.6f),
        new Vector2(3.3f, -1.7f)
    };

    [Space(10f)]
    public AudioClip BattleSong;

    [Header("Will choose a random dialogue, or use the default if none are present\nTry add dialogues that fit whos in the party")]
    public CHATBOXTEXT[] GameOverDialogues;
}
