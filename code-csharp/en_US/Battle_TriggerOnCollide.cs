using UnityEngine;

public class Battle_TriggerOnCollide : MonoBehaviour
{
    public Battle Battle;

    [HideInInspector]
    public bool HasBeganBattle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !HasBeganBattle)
        {
            HasBeganBattle = true;
            BattleSystem.StartBattle(Battle, base.transform.position);
        }
    }
}
