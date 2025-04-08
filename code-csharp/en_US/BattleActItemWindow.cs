using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BattleActItemWindow : MonoBehaviour
{
    [Header("-- References --")]
    [SerializeField]
    private TextMeshProUGUI SelectedInfo_Description;

    [SerializeField]
    private TextMeshProUGUI SelectedInfo_TPCount;

    [SerializeField]
    private Transform SelectableHolder;

    [Space(5f)]
    public Transform Soul;

    public bool AllowInput;

    [SerializeField]
    private int currentSelectionIndex;

    private int previousCurrentSelectionIndex = -1;

    public BattlePartyMember targetPartyMember;

    public int targetEnemyIndex;

    [Space(5f)]
    [SerializeField]
    private GameObject Prefab_BAIWSelectable;

    [SerializeField]
    private GameObject Prefab_BAIW_CharIcon;

    [SerializeField]
    private BattleMemberSelectionMenu MemberSelectionMenu;

    [Space(10f)]
    public List<BattleActItem_StoredInformation> BAIW_CurrentSelectables;

    private void Update()
    {
        if (AllowInput && !GonerMenu.Instance.gonerMenuWasOpen && !GonerMenu.Instance.GonerMenuOpen)
        {
            InputUpdate();
        }
    }

    private void InputUpdate()
    {
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Right))
        {
            currentSelectionIndex++;
            BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_move);
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Left))
        {
            currentSelectionIndex--;
            BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_move);
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Up))
        {
            currentSelectionIndex -= 2;
            BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_move);
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Down))
        {
            currentSelectionIndex += 2;
            BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_move);
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Cancel))
        {
            CancelSelection();
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm))
        {
            SelectCurrentSelection();
        }
        if (currentSelectionIndex != previousCurrentSelectionIndex)
        {
            previousCurrentSelectionIndex = currentSelectionIndex;
            currentSelectionIndex = Mathf.Clamp(currentSelectionIndex, 0, BAIW_CurrentSelectables.Count - 1);
            UpdateCurrentSelection();
        }
    }

    private void CancelSelection()
    {
        if (BAIW_CurrentSelectables.Count > 0 && currentSelectionIndex > -1 && currentSelectionIndex <= BAIW_CurrentSelectables.Count)
        {
            if (BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Item != null)
            {
                BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_move);
                BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.None);
                BattleSystem.BattleState_PlayerTurn_UpdateCurrentlySelectedStatus();
            }
            if (BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Action != null)
            {
                if (BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex].PartyMemberInBattle.HasMagic)
                {
                    BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_move);
                    BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.None);
                    BattleSystem.BattleState_PlayerTurn_UpdateCurrentlySelectedStatus();
                }
                else
                {
                    BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_move);
                    switch (BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Action.ActionTarget)
                    {
                        case BattleAction.BattleActionTarget.PartyMember:
                            MemberSelectionMenu.storedTargetMember = BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex];
                            foreach (BattlePartyMember battlePartyMember in BattleSystem.Instance.BattlePartyMembers)
                            {
                                MemberSelectionMenu.AddMemberName(battlePartyMember, BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Item);
                            }
                            BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.Act_MemberSelection);
                            break;
                        case BattleAction.BattleActionTarget.Enemy:
                            BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.Act_EnemySelection);
                            break;
                    }
                }
            }
        }
        AllowInput = false;
        currentSelectionIndex = 0;
        ClearAllSelectables();
        base.gameObject.SetActive(value: false);
    }

    private void SelectCurrentSelection()
    {
        if (BAIW_CurrentSelectables.Count <= 0 || currentSelectionIndex <= -1 || currentSelectionIndex > BAIW_CurrentSelectables.Count)
        {
            return;
        }
        if (BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Item != null)
        {
            if (BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Item.IsSharedItem)
            {
                BattleSystem.AddNewPlayerMove(BattlePlayerMove.Item, BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex], 0, null, null, BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Item, DarkworldInventory.Instance.PlayerInventory.IndexOf(BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Item));
                BattleSystem.Instance.PartyMembers_MemberPlayAnimation(BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex], "PrepareUseItem");
                BattleSystem.IncrementCurrentPartyMemberStatusSelected();
                DarkworldInventory.Instance.PlayerInventory.Remove(BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Item);
                BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_select);
                BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.None);
                AllowInput = false;
                currentSelectionIndex = 0;
                previousCurrentSelectionIndex = -1;
                ClearAllSelectables();
                return;
            }
            BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_select);
            MemberSelectionMenu.storedTargetMember = BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex];
            foreach (BattlePartyMember battlePartyMember in BattleSystem.Instance.BattlePartyMembers)
            {
                MemberSelectionMenu.AddMemberName(battlePartyMember, BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Item);
            }
            base.gameObject.SetActive(value: false);
            BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.Item_MemberSelection);
        }
        else
        {
            if (!(BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Action != null))
            {
                return;
            }
            if (BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Action.TPRequired > 0)
            {
                if (BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Action.TPRequired <= BattleSystem.Instance.BattleSetting_TPAmount)
                {
                    SetupActionMove();
                }
                else
                {
                    BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_deny);
                }
            }
            else
            {
                SetupActionMove();
            }
        }
    }

    private void SetupActionMove()
    {
        PartyMember[] requiredPartyMembers;
        if (BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex].PartyMemberInBattle.HasMagic)
        {
            BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_select);
            base.gameObject.SetActive(value: false);
            BattleSystem.Instance.BattleWindow_EnemySelection.GetComponent<BattleEnemySelectionMenu>().MenuContext = BattleEnemySelectionMenu.BattleEnemySelectionWindow_Context.Act;
            switch (BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Action.ActionTarget)
            {
                case BattleAction.BattleActionTarget.PartyMember:
                    MemberSelectionMenu.storedTargetMember = BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex];
                    MemberSelectionMenu.StoredBattleAction = BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Action;
                    foreach (BattlePartyMember battlePartyMember2 in BattleSystem.Instance.BattlePartyMembers)
                    {
                        MemberSelectionMenu.AddMemberName(battlePartyMember2, BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Item);
                    }
                    BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.Act_MemberSelection);
                    break;
                case BattleAction.BattleActionTarget.Enemy:
                    BattleSystem.Instance.BattleWindow_EnemySelection.GetComponent<BattleEnemySelectionMenu>().StoredBattleAction = BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Action;
                    BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.Act_EnemySelection);
                    BattleSystem.Instance.BattleWindow_EnemySelection.GetComponent<BattleEnemySelectionMenu>().UpdateGraphics();
                    break;
                case BattleAction.BattleActionTarget.Nobody:
                    BattleSystem.AddNewPlayerMove(BattlePlayerMove.Action, BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex], 0, null, BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Action);
                    BattleSystem.Instance.PartyMembers_MemberPlayAnimation(BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex], BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Action.AnimationToAction);
                    base.gameObject.SetActive(value: false);
                    BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.None);
                    BattleSystem.IncrementCurrentPartyMemberStatusSelected();
                    currentSelectionIndex = 0;
                    break;
            }
            BattleSystem.Instance.PartyMembers_MemberPlayAnimation(BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex], BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Action.AnimationToAction);
            requiredPartyMembers = BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Action.RequiredPartyMembers;
            foreach (PartyMember target in requiredPartyMembers)
            {
                BattlePartyMember battlePartyMember = BattleSystem.Instance.BattlePartyMembers.FirstOrDefault((BattlePartyMember member) => member.PartyMemberInBattle == target);
                if (battlePartyMember != null)
                {
                    BattleSystem.Instance.PartyMembers_MemberPlayAnimation(battlePartyMember, BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Action.AnimationToAction);
                }
            }
            currentSelectionIndex = 0;
            return;
        }
        if (BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Action.RequiredPartyMembers.Length != 0)
        {
            bool flag = true;
            requiredPartyMembers = BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Action.RequiredPartyMembers;
            for (int i = 0; i < requiredPartyMembers.Length; i++)
            {
                BattlePartyMember partyMember = BattleSystem.GetPartyMember(requiredPartyMembers[i]);
                if (partyMember != null && partyMember.PartyMember_Health <= 0f)
                {
                    flag = false;
                }
            }
            if (flag)
            {
                BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_select);
                BattleSystem.AddNewPlayerMove(BattlePlayerMove.Action, BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex], targetEnemyIndex, null, BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Action);
                BattleSystem.Instance.PartyMembers_MemberPlayAnimation(BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex], BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Action.AnimationToAction);
                requiredPartyMembers = BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Action.RequiredPartyMembers;
                for (int i = 0; i < requiredPartyMembers.Length; i++)
                {
                    BattlePartyMember partyMember2 = BattleSystem.GetPartyMember(requiredPartyMembers[i]);
                    if (partyMember2 != null)
                    {
                        BattleSystem.Instance.PartyMembers_MemberPlayAnimation(partyMember2, BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Action.AnimationToAction);
                        partyMember2.SkippingTurn = true;
                    }
                }
                base.gameObject.SetActive(value: false);
                BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.None);
                BattleSystem.IncrementCurrentPartyMemberStatusSelected();
                currentSelectionIndex = 0;
            }
            else
            {
                BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_deny);
            }
            return;
        }
        BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_select);
        BattleSystem.AddNewPlayerMove(BattlePlayerMove.Action, BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex], targetEnemyIndex, null, BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Action);
        BattleSystem.Instance.PartyMembers_MemberPlayAnimation(BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex], BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Action.AnimationToAction);
        requiredPartyMembers = BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Action.RequiredPartyMembers;
        for (int i = 0; i < requiredPartyMembers.Length; i++)
        {
            BattlePartyMember partyMember3 = BattleSystem.GetPartyMember(requiredPartyMembers[i]);
            if (partyMember3 != null)
            {
                BattleSystem.Instance.PartyMembers_MemberPlayAnimation(partyMember3, BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Action.AnimationToAction);
                partyMember3.SkippingTurn = true;
            }
        }
        base.gameObject.SetActive(value: false);
        BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.None);
        BattleSystem.IncrementCurrentPartyMemberStatusSelected();
        currentSelectionIndex = 0;
    }

    public void FinishWindowStuff()
    {
        BattleSystem.Instance.PartyMembers_MemberPlayAnimation(BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex], BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Action.AnimationToAction);
        BattleSystem.IncrementCurrentPartyMemberStatusSelected();
        DarkworldInventory.Instance.PlayerInventory.Remove(BAIW_CurrentSelectables[currentSelectionIndex].Selectable_Item);
        AllowInput = false;
        currentSelectionIndex = 0;
        previousCurrentSelectionIndex = -1;
        ClearAllSelectables();
    }

    public void ResetBattleActItemWindow()
    {
        currentSelectionIndex = 0;
        Soul.localPosition = new Vector3(-597.75f, 66.5f, 0f);
    }

    public void UpdateCurrentSelection()
    {
        foreach (BattleActItem_StoredInformation bAIW_CurrentSelectable in BAIW_CurrentSelectables)
        {
            bAIW_CurrentSelectable.Selectable_References.Selectable_NameText.color = bAIW_CurrentSelectable.Selectable_References.UnselectedColor;
            if (bAIW_CurrentSelectable.Selectable_Action != null && bAIW_CurrentSelectable.Selectable_Action.TPRequired > 0 && bAIW_CurrentSelectable.Selectable_Action.TPRequired > BattleSystem.Instance.BattleSetting_TPAmount)
            {
                bAIW_CurrentSelectable.Selectable_References.Selectable_NameText.color = Color.grey;
            }
        }
        if (currentSelectionIndex > BAIW_CurrentSelectables.Count || currentSelectionIndex < 0)
        {
            return;
        }
        BattleActItem_StoredInformation battleActItem_StoredInformation = BAIW_CurrentSelectables[currentSelectionIndex];
        if (battleActItem_StoredInformation.Selectable_Action != null && battleActItem_StoredInformation.Selectable_Action.TPRequired > 0 && battleActItem_StoredInformation.Selectable_Action.TPRequired > BattleSystem.Instance.BattleSetting_TPAmount)
        {
            battleActItem_StoredInformation.Selectable_References.Selectable_NameText.color = Color.grey;
        }
        Vector2 vector = (Vector2)battleActItem_StoredInformation.Selectable_References.transform.position + new Vector2(-175f, -5f);
        if (vector != new Vector2(-815f, -120f))
        {
            Soul.transform.position = vector;
        }
        if (battleActItem_StoredInformation.Selectable_Item != null)
        {
            SelectedInfo_Description.text = battleActItem_StoredInformation.Selectable_Item.BattleInfo;
            SelectedInfo_TPCount.text = "";
        }
        if (battleActItem_StoredInformation.Selectable_Action != null)
        {
            SelectedInfo_Description.text = battleActItem_StoredInformation.Selectable_Action.ActionDescription;
            if (battleActItem_StoredInformation.Selectable_Action.TPRequired != 0)
            {
                SelectedInfo_TPCount.text = battleActItem_StoredInformation.Selectable_Action.TPRequired + "% TP";
            }
            else
            {
                SelectedInfo_TPCount.text = "";
            }
        }
    }

    public void AddMemberMagicActions()
    {
        BattlePartyMember battlePartyMember = BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex];
        if (battlePartyMember.PartyMemberAction != null)
        {
            AddNewSelectable(null, battlePartyMember.PartyMemberAction, IsMagicAction: true, battlePartyMember.PartyMemberInBattle);
        }
    }

    public void AddNewSelectable(InventoryItem item = null, BattleAction act = null, bool IsMagicAction = false, PartyMember target = null)
    {
        if (!(act != null) && !(item != null))
        {
            return;
        }
        GameObject gameObject = Object.Instantiate(Prefab_BAIWSelectable, SelectableHolder);
        BattleActItem_StoredInformation battleActItem_StoredInformation = new BattleActItem_StoredInformation();
        battleActItem_StoredInformation.Selectable_References = gameObject.GetComponent<BattleActItemSelectable>();
        if (item != null)
        {
            battleActItem_StoredInformation.Selectable_Item = item;
            battleActItem_StoredInformation.Selectable_References.SetSelectableName(item.ItemName);
        }
        if (act != null)
        {
            battleActItem_StoredInformation.Selectable_Action = act;
            battleActItem_StoredInformation.Selectable_References.SetSelectableName(act.ActionName);
            if (IsMagicAction && (bool)target)
            {
                battleActItem_StoredInformation.Selectable_References.Selectable_NameText.color = target.PartyMemberColor_Highlighted;
                battleActItem_StoredInformation.Selectable_References.Selectable_NameText.text = target.name.ToUpper().ToCharArray()[0] + "-Action";
                battleActItem_StoredInformation.Selectable_References.UnselectedColor = target.PartyMemberColor_Highlighted;
            }
            PartyMember[] requiredPartyMembers = act.RequiredPartyMembers;
            foreach (PartyMember partyMember in requiredPartyMembers)
            {
                battleActItem_StoredInformation.Selectable_References.AddCharacterIcon(partyMember);
            }
        }
        BAIW_CurrentSelectables.Add(battleActItem_StoredInformation);
    }

    public void ClearAllSelectables()
    {
        foreach (BattleActItem_StoredInformation bAIW_CurrentSelectable in BAIW_CurrentSelectables)
        {
            if (bAIW_CurrentSelectable.Selectable_References != null)
            {
                Object.Destroy(bAIW_CurrentSelectable.Selectable_References.gameObject);
            }
        }
        SelectedInfo_Description.text = "";
        SelectedInfo_TPCount.text = "";
        BAIW_CurrentSelectables.Clear();
    }

    public void AddDarkworldInventoryToSelectables()
    {
        ClearAllSelectables();
        foreach (InventoryItem item in DarkworldInventory.Instance.PlayerInventory)
        {
            if (item.Useable)
            {
                AddNewSelectable(item);
            }
        }
    }

    public void AddActionsToSelectables(List<BattleAction> actions)
    {
        ClearAllSelectables();
        AddMemberMagicActions();
        foreach (BattleAction action in actions)
        {
            if (action.TargetPartyMember != null)
            {
                if (action.TargetPartyMember == BattleSystem.Instance.BattlePartyMembers[BattleSystem.Instance.CurrentPlayerTurnSelectionIndex].PartyMemberInBattle)
                {
                    MonoBehaviour.print(action.name);
                    AddNewSelectable(null, action);
                }
            }
            else
            {
                MonoBehaviour.print(action.name);
                AddNewSelectable(null, action);
            }
        }
    }

    public void Debounce_AllowInput()
    {
        StartCoroutine(allowInputDelay());
    }

    private IEnumerator allowInputDelay()
    {
        yield return null;
        AllowInput = true;
        UpdateCurrentSelection();
    }
}
