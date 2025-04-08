using System.Collections.Generic;
using UnityEngine;

public class BattleEventListener : MonoBehaviour
{
    [HideInInspector]
    public BattleSystem battleSystem;

    private bool hasSubscribedToEvents;

    protected virtual void Update()
    {
        if (battleSystem != null)
        {
            if (!hasSubscribedToEvents)
            {
                MonoBehaviour.print("subscribed to battle events");
                hasSubscribedToEvents = true;
                battleSystem.Event_OnPlayerTurn += OnPlayerTurnHandler;
                battleSystem.Event_OnPlayerUseRound += OnPlayerUseRound;
                battleSystem.Event_OnEnemyAttackTurn += OnEnemyTurnHandler;
                battleSystem.Event_OnEnemySpared += OnEnemySpared;
                battleSystem.Event_OnEnemyKilled += OnEnemyKilled;
                battleSystem.Event_OnEnemyDamaged += OnEnemyDamaged;
                battleSystem.Event_OnBattleStart += OnBattleStart;
                battleSystem.Event_OnBattleEnd += OnBattleEnd;
                battleSystem.Event_OnBattleStateChange += OnBattleStateChange;
                battleSystem.Event_OnMemberDamaged += OnMemberDamaged;
                OnBattleStart(battleSystem.CurrentBattle);
            }
        }
        else
        {
            battleSystem = BattleSystem.Instance;
        }
    }

    protected virtual void OnBattleStart(Battle Battle)
    {
        Debug.LogError($"Battle has Begun: {Battle}");
    }

    protected virtual void OnBattleEnd(Battle Battle, BattleSystem.EndBattleTypes EndType)
    {
        Debug.Log($"Battle has ended: {Battle} in the context of {EndType}");
    }

    protected virtual void OnPlayerTurnHandler(int turnCount)
    {
        Debug.Log("Player's Turn: " + turnCount);
    }

    protected virtual void OnPlayerUseRound(List<BattlePartyMemberUse> playerActions)
    {
        Debug.Log($"Player Use Round : \n{playerActions}");
    }

    protected virtual void OnEnemyTurnHandler(int turnCount)
    {
        Debug.Log("Enemy's Turn: " + turnCount);
    }

    protected virtual void OnEnemySpared(BattleActiveEnemy targetEnemy, bool wasSpareable, BattlePartyMember inflictor)
    {
        Debug.Log($"{targetEnemy} spared: {inflictor} | WasSpareable = {wasSpareable}");
    }

    protected virtual void OnEnemyDamaged(BattleActiveEnemy targetEnemy, float Damage, BattlePartyMember inflictor)
    {
        Debug.Log($"{targetEnemy} damaged by {inflictor} for {Damage} damage");
    }

    protected virtual void OnMemberDamaged(BattlePartyMember targetMember, float Damage)
    {
        Debug.Log($"{targetMember} damaged for {Damage} damage");
    }

    protected virtual void OnEnemyKilled(BattleActiveEnemy targetEnemy, float Damage, BattlePartyMember inflictor)
    {
        Debug.Log($"{targetEnemy} killed {inflictor} with {Damage} damage");
    }

    protected virtual void OnBattleStateChange(BattleSystem.BattleState newState, BattleSystem.BattleState oldState)
    {
        Debug.Log($"New Battle State: {newState} | Previous Battle State {oldState}");
    }

    protected virtual void OnDestroy()
    {
        if (battleSystem != null && hasSubscribedToEvents)
        {
            hasSubscribedToEvents = false;
            battleSystem.Event_OnPlayerTurn -= OnPlayerTurnHandler;
            battleSystem.Event_OnEnemyAttackTurn -= OnEnemyTurnHandler;
            battleSystem.Event_OnPlayerUseRound -= OnPlayerUseRound;
            battleSystem.Event_OnEnemySpared -= OnEnemySpared;
            battleSystem.Event_OnEnemyKilled -= OnEnemyKilled;
            battleSystem.Event_OnEnemyDamaged -= OnEnemyDamaged;
            battleSystem.Event_OnBattleStart -= OnBattleStart;
            battleSystem.Event_OnBattleStateChange -= OnBattleStateChange;
            battleSystem.Event_OnMemberDamaged -= OnMemberDamaged;
        }
    }
}
