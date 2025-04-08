using System;
using UnityEngine;

[Serializable]
public class BattlePartyMember
{
    [Header("- References -")]
    public PartyMember PartyMemberInBattle;

    public ActivePartyMember ActiveMemberInBattle;

    public GameObject PartyMemberInBattle_Gameobjects;

    public Animator PartyMemberInBattle_Animator;

    public SpriteRenderer PartyMemberInBattle_MainSpriteRenderer;

    public ParticleSystem PartyMemberInBattle_AfterImageParticleRenderer;

    public Vector2 StoredPartyMembePositions;

    public Vector2 StoredOriginalOverworldPosition;

    public BattlePartyMemberStatus PartyMemberStatus;

    [Header("- Stats -")]
    public float PartyMember_Health;

    public float PartyMember_MaxHealth;

    public bool IsDefending;

    public bool SkippingTurn;

    public BattleAction PartyMemberAction;
}
