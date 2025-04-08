using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bullet_Generic : MonoBehaviour
{
    [Header("- References -")]
    public SpriteRenderer _Renderer;

    [Header("- Settings -")]
    public BattleEnemy Dealer;

    public int GrazeGain = 4;

    public Color DefaultColor = Color.white;

    public Color Color_Heal = Color.green;

    public bool DestroyOnDamage;

    public bool ChangeTargetAfterDamage = true;

    private BattlePartyMember InternalTarget;

    private void Awake()
    {
        ChooseNewRandomTarget();
    }

    private void Start()
    {
        if (_Renderer == null)
        {
            _Renderer = base.gameObject.GetComponent<SpriteRenderer>();
        }
        if (_Renderer == null)
        {
            Debug.LogWarning(base.gameObject.name + " + couldn't locate SpriteRenderer, \"_Renderer\"");
        }
        else if (CalculateDamage(InternalTarget) >= 0)
        {
            _Renderer.color = new Color(DefaultColor.r, DefaultColor.g, DefaultColor.b, _Renderer.color.a);
        }
        else
        {
            _Renderer.color = new Color(Color_Heal.r, Color_Heal.g, Color_Heal.b, _Renderer.color.a);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Battle_Soul" && InternalTarget != null)
        {
            if (!Battle_PlayerSoul.Instance.DamageCooldown && Battle_PlayerSoul.Instance.CurrentSoulState == Battle_PlayerSoul.SoulState.Active)
            {
                int num = CalculateDamage(InternalTarget);
                Battle_PlayerSoul.TakeDamage(this, num);
                BattleSystem.Instance.DamagePartyMember(InternalTarget, num);
            }
            if (DestroyOnDamage)
            {
                Object.Destroy(base.gameObject);
            }
            if (ChangeTargetAfterDamage)
            {
                ChooseNewRandomTarget();
            }
        }
    }

    public int CalculateDamage(BattlePartyMember Target)
    {
        if (Target == null)
        {
            return 0;
        }
        float num = 5 * Dealer.ATK;
        for (int i = 0; i < Target.PartyMemberInBattle.DF; i++)
        {
            num = ((!(num > (float)(Target.PartyMemberInBattle.MaximumHealth / 5))) ? ((!(num > (float)(Target.PartyMemberInBattle.MaximumHealth / 8))) ? (num - 1f) : (num - 2f)) : (num - 3f));
        }
        if (Target.IsDefending)
        {
            num *= 2f / 3f;
            num = Mathf.CeilToInt(num);
        }
        return (int)Mathf.Clamp(Mathf.RoundToInt(num), 1f, float.PositiveInfinity);
    }

    private void ChooseNewRandomTarget()
    {
        List<BattlePartyMember> list = BattleSystem.Instance.BattlePartyMembers.Where((BattlePartyMember member) => member.PartyMember_Health > 0f).ToList();
        if (list.Count > 0)
        {
            InternalTarget = list[Random.Range(0, list.Count)];
        }
        else
        {
            InternalTarget = null;
        }
    }
}
