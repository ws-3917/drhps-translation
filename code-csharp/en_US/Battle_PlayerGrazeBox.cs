using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_PlayerGrazeBox : MonoBehaviour
{
    [SerializeField]
    private Animator GrazeboxAnimator;

    [SerializeField]
    private List<GameObject> BulletsInHolding = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(IdleTPGain());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Battle_Bullet" && !BulletsInHolding.Contains(other.gameObject) && !Battle_PlayerSoul.Instance.DamageCooldown)
        {
            BulletsInHolding.Add(other.gameObject);
            BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_graze, 0.5f);
            if (other.GetComponent<Bullet_Generic>() != null)
            {
                BattleSystem.Instance.AddTP(1);
            }
            else
            {
                MonoBehaviour.print("Error whilst grazing! Bullet_Generic not found in collider?");
            }
            GrazeboxAnimator.Play("battlegrazebox_graze", -1, 0f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Battle_Bullet" && BulletsInHolding.Contains(other.gameObject))
        {
            BulletsInHolding.Remove(other.gameObject);
        }
    }

    private void Update()
    {
        if (BulletsInHolding.Count > 0)
        {
            if (Battle_PlayerSoul.Instance.DamageCooldown)
            {
                BulletsInHolding.Clear();
                GrazeboxAnimator.SetBool("AnyBullets", value: false);
            }
            else
            {
                GrazeboxAnimator.SetBool("AnyBullets", value: true);
            }
        }
        else
        {
            GrazeboxAnimator.SetBool("AnyBullets", value: false);
        }
    }

    private IEnumerator IdleTPGain()
    {
        yield return new WaitForSeconds(1f);
        if (BulletsInHolding.Count > 0)
        {
            BattleSystem.Instance.AddTP(1);
        }
        StartCoroutine(IdleTPGain());
    }
}
