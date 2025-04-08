using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAttackWindow : MonoBehaviour
{
    [SerializeField]
    private List<RectTransform> AttackBars = new List<RectTransform>();

    [SerializeField]
    private List<BattleAttackBar> AttackBarComponents = new List<BattleAttackBar>();

    [SerializeField]
    private GameObject AttackBarHolder;

    [SerializeField]
    private bool AttackBarsActive;

    private int FinishedAttackBars;

    private void OnEnable()
    {
        AttackBars.Clear();
        AttackBarComponents.Clear();
        BattleAttackBar[] componentsInChildren = AttackBarHolder.GetComponentsInChildren<BattleAttackBar>();
        foreach (BattleAttackBar battleAttackBar in componentsInChildren)
        {
            if (battleAttackBar.transform.parent == AttackBarHolder.transform)
            {
                AttackBars.Add((RectTransform)battleAttackBar.transform);
                battleAttackBar.FadeAttackBar(5f, Color.white);
                AttackBarComponents.Add(battleAttackBar.GetComponentInChildren<BattleAttackBar>());
            }
        }
    }

    private void Update()
    {
        if (AttackBarsActive && Input.GetKeyDown(PlayerInput.Instance.Key_Confirm) && !GonerMenu.Instance.gonerMenuWasOpen && !GonerMenu.Instance.GonerMenuOpen)
        {
            ProcessAttackInput();
        }
        if (AttackBarsActive)
        {
            foreach (BattleAttackBar closestBar in GetClosestBars())
            {
                if (CheckBarMissed(closestBar))
                {
                    closestBar.HasSetupPosition = false;
                    closestBar.PlayPartyMemberAttackAnimation();
                    BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_fight_slash);
                    StartCoroutine(DamageEnemyDelay(closestBar.TargetActiveEnemy, 0, closestBar.TargetPartyMember, closestBar));
                }
            }
        }
        if (FinishedAttackBars == AttackBarComponents.Count)
        {
            ClearAttackBars();
            BattleSystem.Instance.CurrentBattlePlayerTurnState = BattleSystem.BattlePlayerTurnState.FINISHED;
        }
    }

    private void ProcessAttackInput()
    {
        foreach (BattleAttackBar closestBar in GetClosestBars())
        {
            if (closestBar.AttackBar.anchoredPosition.x <= -230f)
            {
                float num = closestBar.CalculateAccuracy();
                int damage = Mathf.CeilToInt(closestBar.CalculateDamage(closestBar.TargetPartyMember.PartyMemberInBattle.ATK, num, closestBar.TargetActiveEnemy.EnemyInBattle.DF));
                closestBar.HasSetupPosition = false;
                closestBar.PlayPartyMemberAttackAnimation();
                BattleSystem.Instance.AddTP(Mathf.CeilToInt(num / 10f));
                BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_fight_slash);
                if (closestBar.IsCurrentlyCritical)
                {
                    BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_fight_critical, 0.5f);
                }
                StartCoroutine(DamageEnemyDelay(closestBar.TargetActiveEnemy, damage, closestBar.TargetPartyMember, closestBar));
            }
        }
    }

    private bool CheckBarMissed(BattleAttackBar bar)
    {
        if (bar.AttackBar.anchoredPosition.x <= -500f && bar.HasSetupPosition)
        {
            return true;
        }
        return false;
    }

    private IEnumerator DamageEnemyDelay(BattleActiveEnemy target, int Damage, BattlePartyMember dealer, BattleAttackBar targetBar)
    {
        yield return new WaitForSeconds(0.35f);
        GameObject gameObject = Object.Instantiate(BattleSystem.Instance.BattlePrefab_Effect_PartyMemberAttack);
        Object.Destroy(gameObject, 0.5f);
        gameObject.GetComponent<Animator>().Play(dealer.PartyMemberInBattle.BattleEffect_AnimationName);
        if (target != null && target.EnemyInBattle_Gameobject != null)
        {
            gameObject.transform.position = target.EnemyInBattle_Gameobject.transform.position + new Vector3(0f, 0.75f, 0f);
        }
        targetBar.FadeAttackBar(3f, new Color(1f, 1f, 1f, 0f));
        BattleSystem.Instance.DamageEnemy(target, Damage, dealer);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(1f);
        FinishedAttackBars++;
    }

    private List<BattleAttackBar> GetClosestBars()
    {
        List<BattleAttackBar> list = new List<BattleAttackBar>();
        float num = float.MaxValue;
        foreach (BattleAttackBar attackBarComponent in AttackBarComponents)
        {
            if (attackBarComponent.HasSetupPosition)
            {
                float distanceToCritical = attackBarComponent.GetDistanceToCritical();
                if (distanceToCritical < num)
                {
                    num = distanceToCritical;
                    list.Clear();
                    list.Add(attackBarComponent);
                }
                else if (Mathf.Approximately(distanceToCritical, num))
                {
                    list.Add(attackBarComponent);
                }
            }
        }
        return list;
    }

    public void ClearAttackBars()
    {
        foreach (RectTransform attackBar in AttackBars)
        {
            Object.Destroy(attackBar.gameObject);
        }
        AttackBars.Clear();
        AttackBarComponents.Clear();
        FinishedAttackBars = 0;
        AttackBarsActive = false;
        base.gameObject.SetActive(value: false);
    }

    public void SetupAttackBarPositions()
    {
        switch (Random.Range(0, 4))
        {
            case 0:
                if (AttackBars.Count > 0)
                {
                    if (AttackBars.Count >= 1)
                    {
                        AttackBarComponents[0].SetNewBarPosition(160f);
                    }
                    if (AttackBars.Count >= 2)
                    {
                        AttackBarComponents[1].SetNewBarPosition(30f);
                    }
                    if (AttackBars.Count >= 3)
                    {
                        AttackBarComponents[2].SetNewBarPosition(30f);
                    }
                }
                break;
            case 1:
                if (AttackBars.Count > 0)
                {
                    if (AttackBars.Count >= 1)
                    {
                        AttackBarComponents[0].SetNewBarPosition(30f);
                    }
                    if (AttackBars.Count >= 2)
                    {
                        AttackBarComponents[1].SetNewBarPosition(160f);
                    }
                    if (AttackBars.Count >= 3)
                    {
                        AttackBarComponents[2].SetNewBarPosition(220f);
                    }
                }
                break;
            case 2:
                if (AttackBars.Count > 0)
                {
                    if (AttackBars.Count >= 1)
                    {
                        AttackBarComponents[0].SetNewBarPosition(30f);
                    }
                    if (AttackBars.Count >= 2)
                    {
                        AttackBarComponents[1].SetNewBarPosition(30f);
                    }
                    if (AttackBars.Count >= 3)
                    {
                        AttackBarComponents[2].SetNewBarPosition(160f);
                    }
                }
                break;
            case 3:
                if (AttackBars.Count > 0)
                {
                    if (AttackBars.Count >= 1)
                    {
                        AttackBarComponents[0].SetNewBarPosition(160f);
                    }
                    if (AttackBars.Count >= 2)
                    {
                        AttackBarComponents[1].SetNewBarPosition(160f);
                    }
                    if (AttackBars.Count >= 3)
                    {
                        AttackBarComponents[2].SetNewBarPosition(30f);
                    }
                }
                break;
            case 4:
                if (AttackBars.Count > 0)
                {
                    if (AttackBars.Count >= 1)
                    {
                        AttackBarComponents[0].SetNewBarPosition(30f);
                    }
                    if (AttackBars.Count >= 2)
                    {
                        AttackBarComponents[1].SetNewBarPosition(160f);
                    }
                    if (AttackBars.Count >= 3)
                    {
                        AttackBarComponents[2].SetNewBarPosition(30f);
                    }
                }
                break;
        }
        AttackBarsActive = true;
    }
}
