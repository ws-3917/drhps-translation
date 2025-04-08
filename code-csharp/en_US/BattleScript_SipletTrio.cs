using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScript_SipletTrio : BattleEventListener
{
    public BattleAction[] SiplettBattleActions;

    [SerializeField]
    private CHATBOXTEXT ActDialogue_Cheers;

    [SerializeField]
    private CHATBOXTEXT ActDialogue_Cheers_X;

    [SerializeField]
    private BattleAction Act_SusieAction_Cheers;

    [SerializeField]
    private CHATBOXTEXT ActDialogue_SusieCheers;

    [SerializeField]
    private BattleAction Act_RalseiAction_Cheers;

    [SerializeField]
    private CHATBOXTEXT ActDialogue_RalseiCheers;

    [SerializeField]
    private GameObject Effect_ActionGlow;

    [Space(5f)]
    public CHATBOXTEXT[] Dialogue_RandomSipletText;

    [Space(5f)]
    public CHATBOXTEXT Dialogue_IntroFlavorText;

    [Space(5f)]
    public CHATBOXTEXT[] Dialogue_RandomFlavorText;

    public List<BattleActiveEnemy> CurrentAliveEnemies = new List<BattleActiveEnemy>();

    public GameObject DialogueBubble_Default;

    public GameObject DialogueBubble_Bubble;

    [Space(5f)]
    public BattleEnemyAttack Attack_SplashJuice;

    public BattleEnemyAttack Attack_SplashJuiceSingle;

    public BattleEnemyAttack Attack_Kaplunk;

    public BattleEnemyAttack Attack_TeamworkExclusive_LiquidFill;

    public BattleEnemyAttack Attack_DoubleBubbleColumn;

    public BattleEnemyAttack Attack_BubbleColumn;

    private int SplashJuiceAttackCount;

    private int KaplunkAttackCount;

    private int DoubleBubbleColumnAttackCount;

    private int BubbleColumnAttackCount;

    private int SingularSplashJuiceAttackCount;

    private int EnemiesKilled;

    private bool HasPlayedIntroFlavorText;

    private bool hasTakenAnyDamage;

    protected override void OnBattleStart(Battle battle)
    {
        Debug.Log($"Battle Begun: {battle}");
        BattleSystem.Instance.QueueBattleActions(SiplettBattleActions);
        BattleSystem.GetPartyMember(PartyMemberSystem.Instance.Default_SusieDarkworld).PartyMemberAction = Act_SusieAction_Cheers;
        BattleSystem.GetPartyMember(PartyMemberSystem.Instance.Default_NoelleDarkworld).PartyMemberAction = Act_RalseiAction_Cheers;
    }

    protected override void OnBattleEnd(Battle battle, BattleSystem.EndBattleTypes EndType)
    {
        if (EndType != 0)
        {
            return;
        }
        if (Object.FindFirstObjectByType<FD_IntroCutscene>() != null)
        {
            if (EnemiesKilled <= 0)
            {
                Object.FindFirstObjectByType<FD_IntroCutscene>().StartEndingCutscene();
            }
            else
            {
                Object.FindFirstObjectByType<FD_IntroCutscene>().StartEndingCutscene(violenceUsed: true);
            }
        }
        if (!hasTakenAnyDamage)
        {
            HypotheticalGoalManager.Instance.CompleteGoal(Object.FindFirstObjectByType<FD_IntroCutscene>().Goal_NoHit);
        }
    }

    protected override void OnEnemyKilled(BattleActiveEnemy targetEnemy, float Damage, BattlePartyMember inflictor)
    {
        EnemiesKilled++;
        Debug.Log($"{targetEnemy} killed {inflictor} with {Damage} damage");
    }

    protected override void OnPlayerTurnHandler(int turnCount)
    {
        base.OnPlayerTurnHandler(turnCount);
    }

    protected override void OnEnemyTurnHandler(int turnCount)
    {
        base.OnEnemyTurnHandler(turnCount);
    }

    protected override void OnMemberDamaged(BattlePartyMember targetMember, float Damage)
    {
        hasTakenAnyDamage = true;
        Debug.Log($"{targetMember} damaged for {Damage} damage");
    }

    protected override void OnBattleStateChange(BattleSystem.BattleState newState, BattleSystem.BattleState oldState)
    {
        Debug.Log($"New Battle State: {newState} | Previous Battle State {oldState}");
        CurrentAliveEnemies = battleSystem.BattleActiveEnemies;
        if (Object.FindFirstObjectByType<FD_IntroCutscene>() != null)
        {
            Object.FindFirstObjectByType<FD_IntroCutscene>().Noelle_LookRight();
            Object.FindFirstObjectByType<FD_IntroCutscene>().Susie_LookRight();
            Object.FindFirstObjectByType<FD_IntroCutscene>().Kris_LookRight();
        }
        if (Object.FindFirstObjectByType<FD_Intro_SiplettArc>() != null)
        {
            Object.Destroy(Object.FindFirstObjectByType<FD_Intro_SiplettArc>().gameObject);
        }
        if (newState == BattleSystem.BattleState.PlayerTurn)
        {
            int num = Random.Range(0, Dialogue_RandomFlavorText.Length);
            BattleChatbox.Instance.RunText(Dialogue_RandomFlavorText[num], 0, null, ResetCurrentTextIndex: true);
        }
        if (newState == BattleSystem.BattleState.Dialogue)
        {
            foreach (BattleActiveEnemy currentAliveEnemy in CurrentAliveEnemies)
            {
                int num2 = Random.Range(0, Dialogue_RandomSipletText.Length);
                currentAliveEnemy.QueuedDialogue.Add(Dialogue_RandomSipletText[num2]);
                currentAliveEnemy.SpecificTextIndexes.Add(0);
                currentAliveEnemy.QueuedDialogueBubble.Add(DialogueBubble_Bubble);
            }
        }
        if (newState != BattleSystem.BattleState.EnemyTurn)
        {
            return;
        }
        if (CurrentAliveEnemies.Count > 1)
        {
            if (SplashJuiceAttackCount >= 2)
            {
                battleSystem.QueuedBattleAttacks.Add(Attack_Kaplunk);
                KaplunkAttackCount = 1;
                SplashJuiceAttackCount = 0;
                DoubleBubbleColumnAttackCount = 0;
                BubbleColumnAttackCount = 0;
                SingularSplashJuiceAttackCount = 0;
                return;
            }
            if (KaplunkAttackCount >= 2)
            {
                battleSystem.QueuedBattleAttacks.Add(Attack_TeamworkExclusive_LiquidFill);
                battleSystem.QueuedBattleAttacks.Add(Attack_SplashJuice);
                SplashJuiceAttackCount = 1;
                KaplunkAttackCount = 0;
                DoubleBubbleColumnAttackCount = 0;
                BubbleColumnAttackCount = 0;
                SingularSplashJuiceAttackCount = 0;
                return;
            }
            if (DoubleBubbleColumnAttackCount >= 2)
            {
                battleSystem.QueuedBattleAttacks.Add(Attack_DoubleBubbleColumn);
                battleSystem.QueuedBattleAttacks.Add(Attack_DoubleBubbleColumn);
                SplashJuiceAttackCount = 0;
                KaplunkAttackCount = 0;
                DoubleBubbleColumnAttackCount = 1;
                BubbleColumnAttackCount = 0;
                SingularSplashJuiceAttackCount = 0;
                return;
            }
            switch (Random.Range(0, 3))
            {
                case 0:
                    battleSystem.QueuedBattleAttacks.Add(Attack_TeamworkExclusive_LiquidFill);
                    battleSystem.QueuedBattleAttacks.Add(Attack_SplashJuice);
                    SplashJuiceAttackCount++;
                    KaplunkAttackCount = 0;
                    DoubleBubbleColumnAttackCount = 0;
                    BubbleColumnAttackCount = 0;
                    SingularSplashJuiceAttackCount = 0;
                    break;
                case 1:
                    battleSystem.QueuedBattleAttacks.Add(Attack_DoubleBubbleColumn);
                    battleSystem.QueuedBattleAttacks.Add(Attack_DoubleBubbleColumn);
                    DoubleBubbleColumnAttackCount++;
                    BubbleColumnAttackCount = 0;
                    SplashJuiceAttackCount = 0;
                    KaplunkAttackCount = 0;
                    SingularSplashJuiceAttackCount = 0;
                    break;
                default:
                    battleSystem.QueuedBattleAttacks.Add(Attack_Kaplunk);
                    KaplunkAttackCount++;
                    SplashJuiceAttackCount = 0;
                    DoubleBubbleColumnAttackCount = 0;
                    BubbleColumnAttackCount = 0;
                    SingularSplashJuiceAttackCount = 0;
                    break;
            }
        }
        else if (SingularSplashJuiceAttackCount >= 2)
        {
            battleSystem.QueuedBattleAttacks.Add(Attack_SplashJuiceSingle);
            SingularSplashJuiceAttackCount = 1;
            BubbleColumnAttackCount = 0;
        }
        else if (BubbleColumnAttackCount >= 2)
        {
            battleSystem.QueuedBattleAttacks.Add(Attack_BubbleColumn);
            SingularSplashJuiceAttackCount = 0;
            BubbleColumnAttackCount = 1;
        }
        else if (Random.Range(0, 2) == 0)
        {
            battleSystem.QueuedBattleAttacks.Add(Attack_SplashJuiceSingle);
            SingularSplashJuiceAttackCount++;
            BubbleColumnAttackCount = 0;
        }
        else
        {
            battleSystem.QueuedBattleAttacks.Add(Attack_BubbleColumn);
            BubbleColumnAttackCount++;
            BubbleColumnAttackCount = 0;
        }
    }

    public void Act_Cheers(BattlePartyMemberUse action)
    {
        BattleSystem.Instance.SpareEnemy(action.targetEnemy, 20f, action.targetPartyMember, wasSpareable: false);
        BattleSystem.Instance.PartyMembers_MemberPlayAnimation(action.targetPartyMember, "SiplettCheers");
        StartCoroutine(PlayActionGlow_Delayed(action.targetPartyMember));
        battleSystem.FlashMemberLight(action.targetPartyMember);
        if (action.targetEnemy.Enemy_MercyAmount + 20f >= 100f)
        {
            BattleSystem.Instance.ActiveEnemies_PlayAnimation(action.targetEnemy, "Spare");
        }
        else
        {
            BattleSystem.Instance.ActiveEnemies_PlayAnimation(action.targetEnemy, "Cheers");
        }
        BattleSystem.BattleChatbox.AllowInput = true;
        BattleSystem.BattleChatbox.RunText(ActDialogue_Cheers, Random.Range(0, ActDialogue_Cheers.Textboxes.Length), null, ResetCurrentTextIndex: false);
    }

    public void Act_CheersSusie(BattlePartyMemberUse action)
    {
        BattleSystem.Instance.SpareEnemy(action.targetEnemy, 20f, action.targetPartyMember, wasSpareable: false);
        BattleSystem.Instance.PartyMembers_MemberPlayAnimation(action.targetPartyMember, "SiplettCheers");
        StartCoroutine(PlayActionGlow_Delayed(action.targetPartyMember));
        battleSystem.FlashMemberLight(action.targetPartyMember);
        if (action.targetEnemy.Enemy_MercyAmount + 20f >= 100f)
        {
            BattleSystem.Instance.ActiveEnemies_PlayAnimation(action.targetEnemy, "Spare");
        }
        else
        {
            BattleSystem.Instance.ActiveEnemies_PlayAnimation(action.targetEnemy, "Cheers");
        }
        BattleSystem.BattleChatbox.AllowInput = true;
        BattleSystem.BattleChatbox.RunText(ActDialogue_SusieCheers, Random.Range(0, ActDialogue_SusieCheers.Textboxes.Length), null, ResetCurrentTextIndex: false);
    }

    public void Act_CheersRalsei(BattlePartyMemberUse action)
    {
        BattleSystem.Instance.SpareEnemy(action.targetEnemy, 20f, action.targetPartyMember, wasSpareable: false);
        BattleSystem.Instance.PartyMembers_MemberPlayAnimation(action.targetPartyMember, "SiplettCheers");
        StartCoroutine(PlayActionGlow_Delayed(action.targetPartyMember));
        if (action.targetEnemy.Enemy_MercyAmount + 20f >= 100f)
        {
            BattleSystem.Instance.ActiveEnemies_PlayAnimation(action.targetEnemy, "Spare");
        }
        else
        {
            BattleSystem.Instance.ActiveEnemies_PlayAnimation(action.targetEnemy, "Cheers");
        }
        battleSystem.FlashMemberLight(action.targetPartyMember);
        BattleSystem.BattleChatbox.AllowInput = true;
        BattleSystem.BattleChatbox.RunText(ActDialogue_RalseiCheers, Random.Range(0, ActDialogue_RalseiCheers.Textboxes.Length), null, ResetCurrentTextIndex: false);
    }

    public void Act_Cheers_X(BattlePartyMemberUse action)
    {
        BattleSystem.Instance.SpareEnemy(action.targetEnemy, 70f, action.targetPartyMember, wasSpareable: false);
        foreach (BattlePartyMember battlePartyMember in battleSystem.BattlePartyMembers)
        {
            BattleSystem.Instance.PartyMembers_MemberPlayAnimation(battlePartyMember, "SiplettCheers");
            StartCoroutine(PlayActionGlow_Delayed(battlePartyMember));
            battleSystem.FlashMemberLight(battlePartyMember);
        }
        if (action.targetEnemy.Enemy_MercyAmount + 70f >= 100f)
        {
            BattleSystem.Instance.ActiveEnemies_PlayAnimation(action.targetEnemy, "Spare");
        }
        else
        {
            BattleSystem.Instance.ActiveEnemies_PlayAnimation(action.targetEnemy, "Cheers");
        }
        BattleSystem.BattleChatbox.AllowInput = true;
        BattleSystem.BattleChatbox.RunText(ActDialogue_Cheers_X, Random.Range(0, ActDialogue_Cheers_X.Textboxes.Length), null, ResetCurrentTextIndex: false);
    }

    private IEnumerator PlayActionGlow_Delayed(BattlePartyMember member)
    {
        yield return null;
        yield return null;
        Object.Instantiate(Effect_ActionGlow, member.PartyMemberInBattle_Gameobjects.transform.position, Quaternion.identity).GetComponent<Effect_ActionGlow>().SetNewGlowSprite(member.PartyMemberInBattle_Animator.GetComponent<SpriteRenderer>().sprite);
    }
}
