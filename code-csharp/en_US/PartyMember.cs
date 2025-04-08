using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PARTYMEMBER", menuName = "Deltaswap/PartyMember", order = 0)]
public class PartyMember : ScriptableObject
{
    public GameObject PartyMemberPrefab;

    public string PartyMemberName;

    public Color PartyMemberColor = Color.white;

    public Color PartyMemberColor_Highlighted = Color.white;

    [Header("Stats")]
    public int MaximumHealth;

    public bool HasMagic = true;

    public int ATK;

    public int DF;

    public int MAGIC;

    [Header("Battle")]
    public RuntimeAnimatorController PartyMemberBattleAnimator;

    public List<BattleAction> PartyMember_DefaultSpells = new List<BattleAction>();

    public bool StartFromPlayer;

    public string BattleEffect_AnimationName = "BattleEffect_NAMEAttack";

    [Space(5f)]
    public Sprite PartyMemberBattle_NameIcon;

    public Vector2 PartyMemberBattleIcon_CMenuOffset;

    [Space(5f)]
    public Sprite PartyMemberBattleIcon;

    public Sprite PartyMemberBattleHurtIcon;

    public Sprite PartyMemberBattleDefeatedIcon;

    [Space(5f)]
    public Sprite PartyMemberBattleAttack;

    public Sprite PartyMemberBattleAct;

    public Sprite PartyMemberBattleMagic;

    public Sprite PartyMemberBattleItem;

    public Sprite PartyMemberBattleDefend;

    public Sprite PartyMemberBattleSpare;
}
