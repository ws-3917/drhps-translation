using System.Collections;
using UnityEngine;

public class BattleAttack_RudinnTrio_Attack_DiamondDrop : MonoBehaviour
{
    public GameObject DiamondBulletPrefab;

    private Battle_PlayerSoul Soul;

    private bool HasDelayed;

    private void Awake()
    {
        Soul = Battle_PlayerSoul.Instance;
        StartCoroutine(AttackLoop());
    }

    private IEnumerator AttackLoop()
    {
        if (!HasDelayed)
        {
            HasDelayed = true;
            yield return new WaitForSeconds(0.25f);
        }
        Object.Instantiate(DiamondBulletPrefab, BattleSystem.Instance.Holder_Bullets.transform).transform.position = new Vector3(Soul.transform.position.x + Random.Range(-1.25f, 1.25f), BattleSystem.Instance.BattleBox.transform.position.y - Random.Range(0.75f, 0.9f));
        yield return new WaitForSeconds(0.175f);
        StartCoroutine(AttackLoop());
    }
}
