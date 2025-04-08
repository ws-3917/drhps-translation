using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEnemySelectionMenu : MonoBehaviour
{
    public enum BattleEnemySelectionWindow_Context
    {
        Fight = 0,
        Spare = 1,
        Act = 2,
        Cutscene = 3
    }

    [SerializeField]
    private RectTransform SoulSelection;

    public int currentIndex;

    [SerializeField]
    private List<RectTransform> EnemyStatuses = new List<RectTransform>();

    [SerializeField]
    private GameObject EnemyStatusHolder;

    [Space(5f)]
    public BattleEnemySelectionWindow_Context MenuContext;

    public BattleAction StoredBattleAction;

    private void OnEnable()
    {
        EnemyStatuses.Clear();
        BattleEnemyStatus[] componentsInChildren = EnemyStatusHolder.GetComponentsInChildren<BattleEnemyStatus>();
        foreach (BattleEnemyStatus battleEnemyStatus in componentsInChildren)
        {
            if (battleEnemyStatus.transform.parent == EnemyStatusHolder.transform)
            {
                battleEnemyStatus.UpdateStatusGraphics();
                EnemyStatuses.Add((RectTransform)battleEnemyStatus.transform);
            }
        }
        currentIndex = 0;
        UpdateGraphics();
    }

    private void Update()
    {
        if (!GonerMenu.Instance.GonerMenuOpen && !GonerMenu.Instance.gonerMenuWasOpen)
        {
            ProcessInputs();
        }
    }

    private void ProcessInputs()
    {
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Up))
        {
            currentIndex--;
            if (currentIndex <= -1)
            {
                currentIndex = EnemyStatuses.Count - 1;
            }
            BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_move);
            currentIndex = Mathf.Clamp(currentIndex, 0, EnemyStatuses.Count - 1);
            UpdateGraphics();
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Down))
        {
            currentIndex++;
            if (currentIndex == EnemyStatuses.Count)
            {
                currentIndex = 0;
            }
            BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_move);
            currentIndex = Mathf.Clamp(currentIndex, 0, EnemyStatuses.Count - 1);
            UpdateGraphics();
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Cancel))
        {
            CancelSelection();
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm))
        {
            SelectEnemy();
        }
    }

    private void SelectEnemy()
    {
        switch (MenuContext)
        {
            case BattleEnemySelectionWindow_Context.Fight:
                BattleSystem.AddNewPlayerMove(BattlePlayerMove.Fight, BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex], currentIndex);
                BattleSystem.Instance.PartyMembers_MemberPlayAnimation(BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex], "Pre-Attack");
                BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.None);
                BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_select);
                BattleSystem.Instance.GlowEnemy(null);
                BattleSystem.IncrementCurrentPartyMemberStatusSelected();
                break;
            case BattleEnemySelectionWindow_Context.Spare:
                BattleSystem.AddNewPlayerMove(BattlePlayerMove.Spare, BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex], currentIndex);
                BattleSystem.Instance.PartyMembers_MemberPlayAnimation(BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex], "PrepareAct");
                BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.None);
                BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_select);
                BattleSystem.Instance.GlowEnemy(null);
                BattleSystem.IncrementCurrentPartyMemberStatusSelected();
                break;
            case BattleEnemySelectionWindow_Context.Act:
                if (BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex].PartyMemberInBattle.HasMagic)
                {
                    if (StoredBattleAction.TPRequired > 0)
                    {
                        if (BattleSystem.Instance.BattleSetting_TPAmount >= StoredBattleAction.TPRequired)
                        {
                            if (!BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex].PartyMemberInBattle.HasMagic)
                            {
                                BattleSystem.Instance.BattleWindow_ActItemSelection.GetComponent<BattleActItemWindow>().targetEnemyIndex = currentIndex;
                                base.gameObject.SetActive(value: false);
                                BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.Act_SelectAction);
                            }
                            else
                            {
                                BattleSystem.AddNewPlayerMove(BattlePlayerMove.Action, BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex], currentIndex, null, StoredBattleAction);
                                BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.None);
                                BattleSystem.IncrementCurrentPartyMemberStatusSelected();
                            }
                            BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_select);
                            BattleSystem.Instance.GlowEnemy(null);
                            currentIndex = 0;
                        }
                        else
                        {
                            BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_deny);
                        }
                    }
                    else
                    {
                        if (!BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex].PartyMemberInBattle.HasMagic)
                        {
                            BattleSystem.Instance.BattleWindow_ActItemSelection.GetComponent<BattleActItemWindow>().targetEnemyIndex = currentIndex;
                            base.gameObject.SetActive(value: false);
                            BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.Act_SelectAction);
                        }
                        else
                        {
                            BattleSystem.AddNewPlayerMove(BattlePlayerMove.Action, BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex], currentIndex, null, StoredBattleAction);
                            BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.None);
                            BattleSystem.IncrementCurrentPartyMemberStatusSelected();
                        }
                        BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_select);
                        BattleSystem.Instance.GlowEnemy(null);
                        currentIndex = 0;
                    }
                }
                else
                {
                    if (!BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex].PartyMemberInBattle.HasMagic)
                    {
                        BattleSystem.Instance.BattleWindow_ActItemSelection.GetComponent<BattleActItemWindow>().targetEnemyIndex = currentIndex;
                        base.gameObject.SetActive(value: false);
                        BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.Act_SelectAction);
                    }
                    else
                    {
                        BattleSystem.AddNewPlayerMove(BattlePlayerMove.Action, BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex], currentIndex, null, StoredBattleAction);
                        BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.None);
                        BattleSystem.IncrementCurrentPartyMemberStatusSelected();
                    }
                    BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_select);
                    BattleSystem.Instance.GlowEnemy(null);
                    currentIndex = 0;
                }
                break;
        }
    }

    private void CancelSelection()
    {
        currentIndex = 0;
        UpdateGraphics();
        switch (MenuContext)
        {
            case BattleEnemySelectionWindow_Context.Fight:
                BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.None);
                BattleSystem.BattleState_PlayerTurn_UpdateCurrentlySelectedStatus();
                break;
            case BattleEnemySelectionWindow_Context.Spare:
                BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.None);
                BattleSystem.BattleState_PlayerTurn_UpdateCurrentlySelectedStatus();
                break;
            case BattleEnemySelectionWindow_Context.Act:
                if (!BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex].PartyMemberInBattle.HasMagic)
                {
                    base.gameObject.SetActive(value: false);
                    BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.None);
                    BattleSystem.BattleState_PlayerTurn_UpdateCurrentlySelectedStatus();
                }
                else
                {
                    base.gameObject.SetActive(value: false);
                    BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.Act_SelectAction);
                }
                break;
        }
        BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_move);
        BattleSystem.Instance.GlowEnemy(null);
    }

    public void UpdateGraphics()
    {
        StartCoroutine(DelayGraphicForFrame());
        BattleSystem.Instance.GlowEnemy(EnemyStatuses[currentIndex].GetComponent<BattleEnemyStatus>().ActiveEnemy);
    }

    private IEnumerator DelayGraphicForFrame()
    {
        yield return null;
        if (EnemyStatuses[currentIndex].anchoredPosition.y != 0f)
        {
            SoulSelection.anchoredPosition = new Vector2(-512f, EnemyStatuses[currentIndex].anchoredPosition.y + 50f);
        }
    }

    public void ResetSoulToRestPosition()
    {
        SoulSelection.anchoredPosition = new Vector2(-512f, 17.5f);
    }
}
