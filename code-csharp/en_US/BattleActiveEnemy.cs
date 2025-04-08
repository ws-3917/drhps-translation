using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BattleActiveEnemy
{
    [Header("- References -")]
    public BattleEnemy EnemyInBattle;

    public GameObject EnemyInBattle_Gameobject;

    public Animator EnemyInBattle_Animator;

    public Vector2 StoredEnemyPositions;

    public BattleEnemyStatus EnemyStatus;

    public SpriteRenderer EnemyInBattle_MainSpriteRenderer;

    public ParticleSystem EnemyInBattle_AfterImageParticleRenderer;

    [Header("- Dialogue -")]
    public List<CHATBOXTEXT> QueuedDialogue = new List<CHATBOXTEXT>();

    public List<int> SpecificTextIndexes = new List<int>();

    public List<GameObject> QueuedDialogueBubble = new List<GameObject>();

    public int TextIndex;

    public BattleBubbleChatbox ActiveBubbleChatbox;

    [Header("- Stats -")]
    public float Enemy_Health;

    public float Enemy_MaxHealth;

    public bool Enemy_Spareable;

    public float Enemy_MercyAmount;

    public bool Enemy_IsTired;

    public bool Enemy_ConsideredInBattle = true;
}
