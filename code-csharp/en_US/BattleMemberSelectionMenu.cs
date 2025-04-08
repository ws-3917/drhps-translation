using System.Collections.Generic;
using UnityEngine;

public class BattleMemberSelectionMenu : MonoBehaviour
{
    public enum BattleMemberSelectionWindow_Context
    {
        Act = 0,
        Item = 1
    }

    [SerializeField]
    private RectTransform SoulSelection;

    [SerializeField]
    private int currentIndex;

    [SerializeField]
    private InventoryItem targetItem;

    public BattlePartyMember storedTargetMember;

    [SerializeField]
    private GameObject MemberNameStatusPrefab;

    [SerializeField]
    private List<RectTransform> MemberStatuses = new List<RectTransform>();

    [SerializeField]
    private GameObject MemberNameStatusHolder;

    [SerializeField]
    private BattleActItemWindow ActItemWindow;

    [Space(5f)]
    public BattleMemberSelectionWindow_Context MenuContext;

    public BattleAction StoredBattleAction;

    private void OnEnable()
    {
        currentIndex = 0;
        UpdateGraphics();
    }

    public void AddMemberName(BattlePartyMember member, InventoryItem newItem)
    {
        currentIndex = 0;
        targetItem = newItem;
        GameObject gameObject = Object.Instantiate(MemberNameStatusPrefab, MemberNameStatusHolder.transform);
        gameObject.GetComponent<BattleMemberNameStatus>().SetupMemberNameStatus(member.PartyMemberInBattle.PartyMemberName, member.PartyMember_Health, member.PartyMember_MaxHealth);
        MemberStatuses.Add((RectTransform)gameObject.transform);
        UpdateGraphics();
    }

    private void ClearMembers()
    {
        currentIndex = 0;
        UpdateGraphics();
        foreach (RectTransform memberStatus in MemberStatuses)
        {
            Object.Destroy(memberStatus.gameObject);
        }
        BattleSystem.Instance.GlowMember(null);
        MemberStatuses.Clear();
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
                currentIndex = MemberStatuses.Count - 1;
            }
            BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_move);
            currentIndex = Mathf.Clamp(currentIndex, 0, MemberStatuses.Count - 1);
            UpdateGraphics();
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Down))
        {
            currentIndex++;
            if (currentIndex == MemberStatuses.Count)
            {
                currentIndex = 0;
            }
            BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_move);
            currentIndex = Mathf.Clamp(currentIndex, 0, MemberStatuses.Count - 1);
            UpdateGraphics();
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Cancel))
        {
            CancelSelection();
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm))
        {
            UpdateGraphics();
            SelectMember();
        }
    }

    private void SelectMember()
    {
        switch (MenuContext)
        {
            case BattleMemberSelectionWindow_Context.Item:
                BattleSystem.AddNewPlayerMove(BattlePlayerMove.Item, storedTargetMember, 0, BattleSystem.Instance.BattlePartyMembers[currentIndex], null, targetItem, DarkworldInventory.Instance.PlayerInventory.IndexOf(targetItem));
                DarkworldInventory.Instance.PlayerInventory.Remove(targetItem);
                BattleSystem.Instance.PartyMembers_MemberPlayAnimation(BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex], "PrepareUseItem");
                BattleSystem.IncrementCurrentPartyMemberStatusSelected();
                BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.None);
                BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_select);
                currentIndex = 0;
                ClearMembers();
                base.gameObject.SetActive(value: false);
                break;
            case BattleMemberSelectionWindow_Context.Act:
                if (StoredBattleAction.TPRequired > 0)
                {
                    if (BattleSystem.Instance.BattleSetting_TPAmount >= StoredBattleAction.TPRequired)
                    {
                        if (!BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex].PartyMemberInBattle.HasMagic)
                        {
                            BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_select);
                            BattleSystem.Instance.BattleWindow_ActItemSelection.GetComponent<BattleActItemWindow>().targetEnemyIndex = currentIndex;
                            base.gameObject.SetActive(value: false);
                            BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.Act_SelectAction);
                            BattleSystem.Instance.PartyMembers_MemberPlayAnimation(BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex], "PrepareSpell");
                            BattleSystem.Instance.GlowMember(null);
                            currentIndex = 0;
                            ClearMembers();
                            base.gameObject.SetActive(value: false);
                        }
                        else
                        {
                            BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_select);
                            BattleSystem.AddNewPlayerMove(BattlePlayerMove.Action, BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex], 0, BattleSystem.Instance.BattlePartyMembers[currentIndex], StoredBattleAction);
                            BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.None);
                            BattleSystem.IncrementCurrentPartyMemberStatusSelected();
                            BattleSystem.Instance.PartyMembers_MemberPlayAnimation(BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex], "Pre-Attack");
                            BattleSystem.Instance.GlowMember(null);
                            currentIndex = 0;
                            ClearMembers();
                            base.gameObject.SetActive(value: false);
                        }
                    }
                    else
                    {
                        BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_deny);
                    }
                }
                else if (!BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex].PartyMemberInBattle.HasMagic)
                {
                    BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_select);
                    BattleSystem.Instance.BattleWindow_ActItemSelection.GetComponent<BattleActItemWindow>().targetEnemyIndex = currentIndex;
                    base.gameObject.SetActive(value: false);
                    BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.Act_SelectAction);
                    BattleSystem.Instance.PartyMembers_MemberPlayAnimation(BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex], "PrepareAct");
                    BattleSystem.Instance.GlowMember(null);
                    currentIndex = 0;
                    ClearMembers();
                    base.gameObject.SetActive(value: false);
                }
                else
                {
                    BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_select);
                    BattleSystem.AddNewPlayerMove(BattlePlayerMove.Action, BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex], 0, BattleSystem.Instance.BattlePartyMembers[currentIndex], StoredBattleAction);
                    BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.None);
                    BattleSystem.IncrementCurrentPartyMemberStatusSelected();
                    BattleSystem.Instance.PartyMembers_MemberPlayAnimation(BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex], "PrepareAct");
                    BattleSystem.Instance.GlowMember(null);
                    currentIndex = 0;
                    ClearMembers();
                    base.gameObject.SetActive(value: false);
                }
                break;
        }
        ResetSoulToRestPosition();
    }

    private void CancelSelection()
    {
        currentIndex = 0;
        UpdateGraphics();
        ActItemWindow.UpdateCurrentSelection();
        switch (MenuContext)
        {
            case BattleMemberSelectionWindow_Context.Item:
                BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.Item_SelectItem);
                break;
            case BattleMemberSelectionWindow_Context.Act:
                if (!BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex].PartyMemberInBattle.HasMagic)
                {
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
        BattleSystem.Instance.GlowMember(null);
        ClearMembers();
        base.gameObject.SetActive(value: false);
    }

    private void UpdateGraphics()
    {
        if (MemberStatuses.Count > 0 && currentIndex > -1 && currentIndex < MemberStatuses.Count && MemberStatuses[currentIndex].anchoredPosition.y != 0f)
        {
            SoulSelection.anchoredPosition = new Vector2(-512f, MemberStatuses[currentIndex].anchoredPosition.y + 50f);
        }
        BattleSystem.Instance.GlowMember(BattleSystem.Instance.BattlePartyMembers[currentIndex]);
    }

    public void ResetSoulToRestPosition()
    {
        SoulSelection.anchoredPosition = new Vector2(-512f, 17.5f);
    }
}
