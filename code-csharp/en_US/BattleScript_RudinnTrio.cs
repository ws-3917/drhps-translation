using System.Collections.Generic;
using UnityEngine;

public class BattleScript_RudinnTrio : BattleEventListener
{
    public CHATBOXTEXT[] Dialogue_RandomRudinnText;

    [Space(5f)]
    public CHATBOXTEXT Dialogue_IntroFlavorText;

    [Space(5f)]
    public CHATBOXTEXT[] Dialogue_RandomFlavorText;

    public List<BattleActiveEnemy> CurrentAliveEnemies = new List<BattleActiveEnemy>();

    public GameObject DialogueBubble_Default;

    [Space(5f)]
    public BattleEnemyAttack Attack_DiamondDrop;

    private bool HasPlayedIntroFlavorText;

    protected override void OnBattleStart(Battle battle)
    {
        Debug.Log($"Battle Begun: {battle}");
    }

    protected override void OnPlayerTurnHandler(int turnCount)
    {
        base.OnPlayerTurnHandler(turnCount);
    }

    protected override void OnEnemyTurnHandler(int turnCount)
    {
        base.OnEnemyTurnHandler(turnCount);
    }

    protected override void OnBattleStateChange(BattleSystem.BattleState newState, BattleSystem.BattleState oldState)
    {
        Debug.Log($"New Battle State: {newState} | Previous Battle State {oldState}");
        CurrentAliveEnemies = battleSystem.BattleActiveEnemies;
        if (newState == BattleSystem.BattleState.PlayerTurn && HasPlayedIntroFlavorText)
        {
            int num = Random.Range(0, Dialogue_RandomFlavorText.Length);
            BattleChatbox.Instance.RunText(Dialogue_RandomFlavorText[num], 0, null, ResetCurrentTextIndex: true);
        }
        if (newState == BattleSystem.BattleState.PlayerTurn && oldState == BattleSystem.BattleState.Intro)
        {
            HasPlayedIntroFlavorText = true;
            BattleChatbox.Instance.RunText(Dialogue_IntroFlavorText, 0, null, ResetCurrentTextIndex: true);
        }
        if (newState == BattleSystem.BattleState.Dialogue)
        {
            foreach (BattleActiveEnemy currentAliveEnemy in CurrentAliveEnemies)
            {
                int num2 = Random.Range(0, Dialogue_RandomRudinnText.Length);
                currentAliveEnemy.QueuedDialogue.Add(Dialogue_RandomRudinnText[num2]);
                currentAliveEnemy.SpecificTextIndexes.Add(0);
                currentAliveEnemy.QueuedDialogueBubble.Add(DialogueBubble_Default);
            }
        }
        if (newState == BattleSystem.BattleState.EnemyTurn)
        {
            battleSystem.QueuedBattleAttacks.Add(Attack_DiamondDrop);
        }
    }
}
