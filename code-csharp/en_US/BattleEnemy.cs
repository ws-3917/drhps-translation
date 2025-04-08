using UnityEngine;

[CreateAssetMenu(fileName = "ENEMY", menuName = "Deltaswap/Battle/Enemy", order = 0)]
public class BattleEnemy : ScriptableObject
{
    [Header("- Enemy Settings -")]
    public string EnemyName = "PLACEHOLDER";

    public float EnemyMaxHealth = 5f;

    public CHATBOXTEXT EnemyCheckText;

    public int ATK;

    public int DF = 2;

    public int EXP = 1;

    public int DarkDollars = 30;

    [Header("when the player uses \"Spare\" when the enemy isn't sparable yet")]
    public int DefaultSpareAmount = 20;

    [Header("- Enemy Visuals -")]
    public GameObject EnemyPrefab;

    [Header("- Animations -")]
    public string EnemyAnim_Idle;

    public string EnemyAnim_Hurt;

    public string EnemyAnim_Spare;
}
