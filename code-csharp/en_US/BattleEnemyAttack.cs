using UnityEngine;

[CreateAssetMenu(fileName = "ATTACK", menuName = "Deltaswap/Battle/EnemyAttack", order = 0)]
public class BattleEnemyAttack : ScriptableObject
{
    [Header("- Attack Settings -")]
    public GameObject AttackPrefab;

    [Header("-1 will make the attack last forever, allowing the battle script to take over.")]
    public float AttackTime = 7.5f;

    [Header("Will fade away all enemies and party members, good for wide attacks")]
    public bool FadeCharactersAway;

    [Header("Will destroy every child in bullet holder on attack end")]
    public bool ClearBulletHolderChildren;
}
