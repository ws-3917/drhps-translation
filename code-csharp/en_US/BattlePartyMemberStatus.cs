using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattlePartyMemberStatus : MonoBehaviour
{
    public bool CurrentlySelected;

    public bool StatusTabOpened;

    private bool previousCurrentlySelected;

    public bool HasActAbility;

    public List<BattlePartyMemberStatusButton> Buttons = new List<BattlePartyMemberStatusButton>();

    public BattlePartyMember TargetPartyMember;

    private float PreviousHP;

    public int SelectedIndex;

    private bool DebounceEnabled;

    public Image MemberIcon;

    public RawImage MemberNameIcon;

    public RawImage BoxOutline;

    public Image SpecialLines1;

    public Image SpecialLines2;

    public Image HealthBar;

    public TextMeshProUGUI Text_Health;

    public TextMeshProUGUI Text_MaxHealth;

    [SerializeField]
    private RectTransform StatusTransform;

    private void Update()
    {
        if (previousCurrentlySelected != CurrentlySelected)
        {
            if (!previousCurrentlySelected && CurrentlySelected)
            {
                MemberIcon.sprite = TargetPartyMember.PartyMemberInBattle.PartyMemberBattleIcon;
                BattleSystem.Instance.PartyMembers_MemberPlayAnimation(TargetPartyMember, "Idle");
            }
            previousCurrentlySelected = CurrentlySelected;
            UpdateButtonSprites();
        }
        if (PreviousHP != TargetPartyMember.PartyMember_Health)
        {
            PreviousHP = TargetPartyMember.PartyMember_Health;
            UpdateHealthbar();
        }
        if (CurrentlySelected && !GonerMenu.Instance.GonerMenuOpen && !GonerMenu.Instance.gonerMenuWasOpen)
        {
            HasActAbility = !TargetPartyMember.PartyMemberInBattle.HasMagic;
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm) && !GonerMenu.Instance.GonerMenuOpen)
            {
                CurrentlySelected = false;
                SelectButton(SelectedIndex);
                BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_select);
            }
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Left) && !DebounceEnabled)
            {
                SelectedIndex--;
                if (SelectedIndex <= -1)
                {
                    SelectedIndex = 4;
                }
                SelectedIndex = Mathf.Clamp(SelectedIndex, 0, 4);
                BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_move);
                UpdateButtonSprites();
            }
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Right) && !DebounceEnabled && !GonerMenu.Instance.GonerMenuOpen)
            {
                SelectedIndex++;
                if (SelectedIndex >= 5)
                {
                    SelectedIndex = 0;
                }
                SelectedIndex = Mathf.Clamp(SelectedIndex, 0, 4);
                BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_move);
                UpdateButtonSprites();
            }
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Cancel) && !GonerMenu.Instance.GonerMenuOpen && !DebounceEnabled)
            {
                BattleSystem.UndoPreviousCurrentPartyMemberStatusSelected();
            }
        }
        if (StatusTabOpened)
        {
            BoxOutline.enabled = true;
            SpecialLines1.enabled = true;
            SpecialLines2.enabled = true;
            StatusTransform.anchoredPosition = Vector2.Lerp(StatusTransform.anchoredPosition, new Vector2(0f, 0f), 18f * Time.fixedDeltaTime);
        }
        else
        {
            BoxOutline.enabled = false;
            SpecialLines1.enabled = false;
            SpecialLines2.enabled = false;
            StatusTransform.anchoredPosition = Vector2.Lerp(StatusTransform.anchoredPosition, new Vector2(0f, -65f), 18f * Time.fixedDeltaTime);
        }
    }

    private void UpdateHealthbar()
    {
        HealthBar.fillAmount = TargetPartyMember.PartyMember_Health / TargetPartyMember.PartyMember_MaxHealth;
        Text_Health.text = TargetPartyMember.PartyMember_Health.ToString();
        Text_MaxHealth.text = TargetPartyMember.PartyMember_MaxHealth.ToString();
        if (TargetPartyMember.PartyMember_Health <= 0f)
        {
            Text_Health.color = Color.red;
            Text_MaxHealth.color = Color.red;
        }
        else
        {
            Text_Health.color = Color.white;
            Text_MaxHealth.color = Color.white;
        }
    }

    public void UpdateButtonSprites()
    {
        for (int i = 0; i < Buttons.Count; i++)
        {
            if (HasActAbility && Buttons[i].HasAct_Deselected != null)
            {
                if (SelectedIndex == i && StatusTabOpened)
                {
                    Buttons[i].Renderer.sprite = Buttons[i].HasAct_Selected;
                }
                else
                {
                    Buttons[i].Renderer.sprite = Buttons[i].HasAct_Deselected;
                }
            }
            else if (SelectedIndex == i && StatusTabOpened)
            {
                Buttons[i].Renderer.sprite = Buttons[i].Selected;
            }
            else
            {
                Buttons[i].Renderer.sprite = Buttons[i].Deselected;
            }
        }
    }

    private void SelectButton(int index)
    {
        switch (index)
        {
            case 0:
                BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.Fight_EnemySelection);
                MemberIcon.sprite = TargetPartyMember.PartyMemberInBattle.PartyMemberBattleAttack;
                break;
            case 1:
                if (TargetPartyMember.PartyMemberInBattle.HasMagic)
                {
                    BattleSystem.Instance.BattleWindow_ActItemSelection.GetComponent<BattleActItemWindow>().ResetBattleActItemWindow();
                    BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.Act_SelectAction);
                    BattleSystem.Instance.BattleWindow_ActItemSelection.GetComponent<BattleActItemWindow>().UpdateCurrentSelection();
                    MemberIcon.sprite = TargetPartyMember.PartyMemberInBattle.PartyMemberBattleAct;
                }
                else
                {
                    BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.Act_EnemySelection);
                    BattleSystem.Instance.BattleWindow_EnemySelection.GetComponent<BattleEnemySelectionMenu>().UpdateGraphics();
                    MemberIcon.sprite = TargetPartyMember.PartyMemberInBattle.PartyMemberBattleAct;
                }
                CurrentlySelected = false;
                break;
            case 2:
                {
                    List<InventoryItem> list = new List<InventoryItem>();
                    foreach (InventoryItem item in DarkworldInventory.Instance.PlayerInventory)
                    {
                        if (item.Useable)
                        {
                            list.Add(item);
                        }
                    }
                    if (list.Count > 0)
                    {
                        BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.Item_SelectItem);
                        MemberIcon.sprite = TargetPartyMember.PartyMemberInBattle.PartyMemberBattleItem;
                    }
                    else
                    {
                        BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_deny);
                        CurrentlySelected = true;
                    }
                    break;
                }
            case 3:
                BattleSystem.OpenBottomBarWindow(BattleSystem.BottomBarWindows.Spare_EnemySelection);
                MemberIcon.sprite = TargetPartyMember.PartyMemberInBattle.PartyMemberBattleSpare;
                break;
            case 4:
                BattleSystem.Instance.PartyMembers_MemberPlayAnimation(TargetPartyMember, "Defend");
                MemberIcon.sprite = TargetPartyMember.PartyMemberInBattle.PartyMemberBattleDefend;
                BattleSystem.PlayBattleSoundEffect(BattleSystem.Instance.SFX_menu_select);
                BattleSystem.AddNewPlayerMove(BattlePlayerMove.Defend, TargetPartyMember, -1);
                BattleSystem.Instance.AddTP(16);
                CurrentlySelected = false;
                StartCoroutine(DebounceInput_BeforeUnselect());
                break;
        }
    }

    private IEnumerator DebounceInput_BeforeUnselect()
    {
        DebounceEnabled = true;
        yield return new WaitForSeconds(0f);
        BattleSystem.IncrementCurrentPartyMemberStatusSelected();
        DebounceEnabled = false;
    }

    public void ResetMemberIcon()
    {
        MemberIcon.sprite = TargetPartyMember.PartyMemberInBattle.PartyMemberBattleIcon;
    }
}
