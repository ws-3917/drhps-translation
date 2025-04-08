using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DarkworldMenu : MonoBehaviour
{
    public enum DarkworldMenuStates
    {
        Disabled = 0,
        TopRow = 1,
        Item = 2,
        Equip = 3,
        Power = 4,
        Config = 5
    }

    public enum DarkworldMenuSubStates
    {
        Disabled = 0,
        ItemSelection = 1,
        ItemChooseMember = 2,
        EquipSelectSlot = 3,
        EquipChooseSwap = 4,
        PowerSpellInspect = 5
    }

    [Header("-- Variables --")]
    public bool MenuOpen;

    public bool CanOpenMenu;

    private bool PreviousMenuState;

    [SerializeField]
    private bool ForceDisableInput;

    private bool HasEnabledInputCooldown;

    [Header("Menu States and SubStates")]
    public DarkworldMenuStates CurrentState;

    public DarkworldMenuSubStates CurrentSubState;

    public PlayerManager.PlayerState previousPlayerState;

    [Header("-- References --")]
    [SerializeField]
    private GameObject InventoryMenuObject;

    [SerializeField]
    private GameObject EquipMenuObject;

    [SerializeField]
    private GameObject PowerMenuObject;

    [SerializeField]
    private Transform SoulSprite;

    [Space(5f)]
    [SerializeField]
    private Color Color_Disabled;

    [SerializeField]
    private Color Color_Enabled;

    [SerializeField]
    private Color Color_Highlighted;

    [SerializeField]
    private GameObject BarPartyMemberPrefab;

    [SerializeField]
    private List<DarkworldMenu_BarPartyMember> CurrentBarPartyMemberPrefabs = new List<DarkworldMenu_BarPartyMember>();

    [Header("-- Animations --")]
    [SerializeField]
    private Animator _Anim_MainSection;

    [Header("-- Sounds --")]
    [SerializeField]
    private AudioSource _MenuSource;

    [SerializeField]
    private AudioClip snd_menuMove;

    [SerializeField]
    private AudioClip snd_backout;

    [SerializeField]
    private AudioClip snd_select;

    [SerializeField]
    private AudioClip snd_deny;

    [Header("-- Top and Bottom Row References --")]
    [SerializeField]
    private int _TopRowCursorPos;

    [SerializeField]
    private TopButton_SpriteIndex[] TopButtons;

    [SerializeField]
    private RawImage TopButtonHoveredText;

    [SerializeField]
    private TextMeshProUGUI DarkDollarText;

    [Space(10f)]
    [SerializeField]
    private GameObject _TopBarDialogueObject;

    [SerializeField]
    private TextMeshProUGUI _TopBarText;

    [SerializeField]
    private GameObject BottomRow;

    [Header("-- Item Menu References --")]
    [SerializeField]
    private int _ItemCursorPos;

    [SerializeField]
    private int _ItemOptionCursorPos;

    [SerializeField]
    private int _ItemMemberCursorPos;

    [SerializeField]
    private int previousItemOptionPos;

    [SerializeField]
    private TextMeshProUGUI[] _InventoryTextSlots;

    [SerializeField]
    private TextMeshProUGUI[] _InventoryOptions;

    [SerializeField]
    private InventoryItem StoredItem;

    private bool ChatboxRunning;

    private static DarkworldMenu instance;

    public static DarkworldMenu Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Object.Destroy(base.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        UpdateTopRowButtons(Activation: false);
    }

    private void Update()
    {
        if (CanOpenMenu && !ChatboxRunning && !ForceDisableInput)
        {
            ProcessInputs();
        }
        if (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            ChatboxRunning = true;
        }
        else if (ChatboxRunning)
        {
            StartCoroutine(ChatboxToDarkworldMenuDebounce());
        }
        else
        {
            ChatboxRunning = false;
        }
        if (MenuOpen && !GonerMenu.Instance.GonerMenuOpen && !ForceDisableInput)
        {
            switch (CurrentState)
            {
                case DarkworldMenuStates.TopRow:
                    TopRowInputs();
                    break;
                case DarkworldMenuStates.Item:
                    InventoryMenuInputs();
                    break;
            }
        }
        if (GonerMenu.Instance.GonerMenuOpen && !HasEnabledInputCooldown)
        {
            HasEnabledInputCooldown = true;
            StartCoroutine(InputDebounce());
        }
        if (MenuOpen && PlayerManager.Instance._PlayerState == PlayerManager.PlayerState.Game)
        {
            CloseMenu();
        }
    }

    private IEnumerator ChatboxToDarkworldMenuDebounce()
    {
        yield return new WaitForSeconds(0.2f);
        ChatboxRunning = false;
    }

    private void ProcessInputs()
    {
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Menu) && (CurrentState == DarkworldMenuStates.Disabled || CurrentState == DarkworldMenuStates.TopRow))
        {
            MenuOpen = !MenuOpen;
            DarkDollarText.text = SecurePlayerPrefs.GetSecureInt("TotalCash").ToString() ?? "";
            if (MenuOpen)
            {
                OpenMenu();
            }
            else
            {
                CloseMenu();
            }
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Cancel))
        {
            if (!Input.GetKeyDown(PlayerInput.Instance.Key_Confirm) && !Input.GetKeyDown(KeyCode.Return) && CurrentState == DarkworldMenuStates.TopRow && !GonerMenu.Instance.GonerMenuOpen)
            {
                CloseMenu();
                MenuOpen = false;
            }
            if (!Input.GetKeyDown(PlayerInput.Instance.Key_Confirm) && !Input.GetKeyDown(KeyCode.Return) && MenuOpen && CurrentState != DarkworldMenuStates.TopRow && CurrentState != DarkworldMenuStates.Item && CurrentState != DarkworldMenuStates.Config)
            {
                InventoryOptionsSetSoulPosition(_ItemOptionCursorPos);
                InventorySetOptionColor(4, Color_Enabled);
                CurrentSubState = DarkworldMenuSubStates.Disabled;
                InventorySetInventoryColor(Color_Disabled);
                _ItemCursorPos = 0;
                _ItemOptionCursorPos = 0;
                CurrentSubState = DarkworldMenuSubStates.Disabled;
                SetupSelectedMenu(DarkworldMenuStates.TopRow);
            }
        }
        if (PreviousMenuState != MenuOpen)
        {
            PreviousMenuState = MenuOpen;
        }
    }

    private void SetupSelectedMenu(DarkworldMenuStates Menu)
    {
        CurrentState = Menu;
        InventoryMenuObject.SetActive(value: false);
        EquipMenuObject.SetActive(value: false);
        PowerMenuObject.SetActive(value: false);
        InventorySetMemberSelectionSoul(0, SelectAll: true, Selected: false);
        SoulSprite.position = new Vector2(-42f, -42f);
        RemoveTopbarDialogue();
        switch (Menu)
        {
            case DarkworldMenuStates.TopRow:
                UpdateTopRowButtons(Activation: false);
                break;
            case DarkworldMenuStates.Item:
                CurrentSubState = DarkworldMenuSubStates.Disabled;
                UpdateTopRowButtons(Activation: true);
                InventorySetInventoryColor(Color_Disabled);
                InventoryListSetup(IsKeyInventory: false);
                InventoryOptionsSetSoulPosition(_ItemOptionCursorPos);
                InventoryMenuObject.SetActive(value: true);
                _MenuSource.PlayOneShot(snd_select);
                _ItemCursorPos = 0;
                break;
            case DarkworldMenuStates.Equip:
                UpdateTopRowButtons(Activation: false);
                CurrentSubState = DarkworldMenuSubStates.Disabled;
                CurrentState = DarkworldMenuStates.TopRow;
                _MenuSource.PlayOneShot(snd_deny);
                break;
            case DarkworldMenuStates.Power:
                UpdateTopRowButtons(Activation: false);
                CurrentSubState = DarkworldMenuSubStates.Disabled;
                CurrentState = DarkworldMenuStates.TopRow;
                _MenuSource.PlayOneShot(snd_deny);
                break;
            case DarkworldMenuStates.Config:
                CurrentSubState = DarkworldMenuSubStates.Disabled;
                CurrentState = DarkworldMenuStates.TopRow;
                _MenuSource.PlayOneShot(snd_select);
                GonerMenu.Instance.Pause_OpenOptionMenu();
                UpdateTopRowButtons(Activation: false);
                break;
            case DarkworldMenuStates.Disabled:
                break;
        }
    }

    private void TopRowInputs()
    {
        RemoveTopbarDialogue();
        SoulSprite.position = new Vector2(-42f, -42f);
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Left))
        {
            if (_TopRowCursorPos - 1 != -1)
            {
                _MenuSource.PlayOneShot(snd_menuMove);
            }
            _TopRowCursorPos--;
            _TopRowCursorPos = Mathf.Clamp(_TopRowCursorPos, 0, 3);
            UpdateTopRowButtons(Activation: false);
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Right))
        {
            if (_TopRowCursorPos + 1 != 4)
            {
                _MenuSource.PlayOneShot(snd_menuMove);
            }
            _TopRowCursorPos++;
            _TopRowCursorPos = Mathf.Clamp(_TopRowCursorPos, 0, 3);
            UpdateTopRowButtons(Activation: false);
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm))
        {
            SetupSelectedMenu(TopButtons[_TopRowCursorPos].TargetState);
        }
    }

    private void UpdateTopRowButtons(bool Activation)
    {
        TopButton_SpriteIndex[] topButtons = TopButtons;
        foreach (TopButton_SpriteIndex topButton_SpriteIndex in topButtons)
        {
            topButton_SpriteIndex.ButtonRenderer.texture = topButton_SpriteIndex.Unhovered.texture;
        }
        if (Activation)
        {
            TopButtons[_TopRowCursorPos].ButtonRenderer.texture = TopButtons[_TopRowCursorPos].Selected.texture;
        }
        else
        {
            TopButtons[_TopRowCursorPos].ButtonRenderer.texture = TopButtons[_TopRowCursorPos].Hovered.texture;
        }
        TopButtonHoveredText.texture = TopButtons[_TopRowCursorPos].SectionTextSprite.texture;
    }

    private void InventoryMenuInputs()
    {
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Cancel) && !ForceDisableInput && !Input.GetKeyDown(PlayerInput.Instance.Key_Confirm))
        {
            if (CurrentSubState == DarkworldMenuSubStates.ItemSelection)
            {
                InventoryOptionsSetSoulPosition(_ItemOptionCursorPos);
                InventorySetOptionColor(4, Color_Enabled);
                CurrentSubState = DarkworldMenuSubStates.Disabled;
                InventorySetInventoryColor(Color_Disabled);
            }
            else if (CurrentSubState == DarkworldMenuSubStates.ItemChooseMember)
            {
                CurrentSubState = DarkworldMenuSubStates.ItemSelection;
                InventorySetMemberSelectionSoul(0, SelectAll: true, Selected: false);
                if (_ItemOptionCursorPos != 2)
                {
                    InventorySelectionSetSoulPosition(_ItemCursorPos, IsKeyItem: false);
                }
                else
                {
                    InventorySelectionSetSoulPosition(_ItemCursorPos, IsKeyItem: true);
                }
            }
            else
            {
                _ItemCursorPos = 0;
                _ItemOptionCursorPos = 0;
                CurrentSubState = DarkworldMenuSubStates.Disabled;
                SetupSelectedMenu(DarkworldMenuStates.TopRow);
            }
            _MenuSource.PlayOneShot(snd_backout);
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Right))
        {
            if (CurrentSubState == DarkworldMenuSubStates.Disabled)
            {
                if (_ItemOptionCursorPos + 1 != 3)
                {
                    _MenuSource.PlayOneShot(snd_menuMove);
                }
                _ItemOptionCursorPos++;
                _ItemOptionCursorPos = Mathf.Clamp(_ItemOptionCursorPos, 0, 2);
                InventoryOptionsSetSoulPosition(_ItemOptionCursorPos);
            }
            else if (CurrentSubState == DarkworldMenuSubStates.ItemChooseMember)
            {
                if (_ItemMemberCursorPos + 1 != CurrentBarPartyMemberPrefabs.Count)
                {
                    _MenuSource.PlayOneShot(snd_menuMove);
                }
                _ItemMemberCursorPos++;
                _ItemMemberCursorPos = Mathf.Clamp(_ItemMemberCursorPos, 0, CurrentBarPartyMemberPrefabs.Count - 1);
                if (StoredItem.IsSharedItem && _ItemOptionCursorPos == 1)
                {
                    InventorySetMemberSelectionSoul(_ItemMemberCursorPos, SelectAll: true, Selected: true);
                }
                else
                {
                    InventorySetMemberSelectionSoul(_ItemMemberCursorPos, SelectAll: false, Selected: true);
                }
            }
            else
            {
                if (_ItemCursorPos + 1 != 12)
                {
                    _MenuSource.PlayOneShot(snd_menuMove);
                }
                _ItemCursorPos++;
                if (_ItemOptionCursorPos != 2)
                {
                    _ItemCursorPos = Mathf.Clamp(_ItemCursorPos, 0, DarkworldInventory.Instance.PlayerInventory.Count - 1);
                    InventorySelectionSetSoulPosition(_ItemCursorPos, IsKeyItem: false);
                }
                else
                {
                    _ItemCursorPos = Mathf.Clamp(_ItemCursorPos, 0, DarkworldInventory.Instance.PlayerKeyItems.Count - 1);
                    InventorySelectionSetSoulPosition(_ItemCursorPos, IsKeyItem: true);
                }
            }
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Left))
        {
            if (CurrentSubState == DarkworldMenuSubStates.Disabled)
            {
                if (_ItemOptionCursorPos - 1 != -1)
                {
                    _MenuSource.PlayOneShot(snd_menuMove);
                }
                _ItemOptionCursorPos--;
                _ItemOptionCursorPos = Mathf.Clamp(_ItemOptionCursorPos, 0, CurrentBarPartyMemberPrefabs.Count - 1);
                InventoryOptionsSetSoulPosition(_ItemOptionCursorPos);
            }
            else if (CurrentSubState == DarkworldMenuSubStates.ItemChooseMember)
            {
                if (_ItemMemberCursorPos - 1 != -1)
                {
                    _MenuSource.PlayOneShot(snd_menuMove);
                }
                _ItemMemberCursorPos--;
                _ItemMemberCursorPos = Mathf.Clamp(_ItemMemberCursorPos, 0, CurrentBarPartyMemberPrefabs.Count - 1);
                if (StoredItem.IsSharedItem && _ItemOptionCursorPos == 1)
                {
                    InventorySetMemberSelectionSoul(_ItemMemberCursorPos, SelectAll: true, Selected: true);
                }
                else
                {
                    InventorySetMemberSelectionSoul(_ItemMemberCursorPos, SelectAll: false, Selected: true);
                }
            }
            else
            {
                if (_ItemCursorPos - 1 != -1)
                {
                    _MenuSource.PlayOneShot(snd_menuMove);
                }
                _ItemCursorPos--;
                if (_ItemOptionCursorPos != 2)
                {
                    _ItemCursorPos = Mathf.Clamp(_ItemCursorPos, 0, DarkworldInventory.Instance.PlayerInventory.Count - 1);
                    InventorySelectionSetSoulPosition(_ItemCursorPos, IsKeyItem: false);
                }
                else
                {
                    _ItemCursorPos = Mathf.Clamp(_ItemCursorPos, 0, DarkworldInventory.Instance.PlayerKeyItems.Count - 1);
                    InventorySelectionSetSoulPosition(_ItemCursorPos, IsKeyItem: true);
                }
            }
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Up) && CurrentSubState == DarkworldMenuSubStates.ItemSelection)
        {
            if (_ItemCursorPos - 2 > -1)
            {
                _MenuSource.PlayOneShot(snd_menuMove);
            }
            _ItemCursorPos -= 2;
            _ItemCursorPos = Mathf.Clamp(_ItemCursorPos, 0, 11);
            if (_ItemOptionCursorPos != 2)
            {
                InventorySelectionSetSoulPosition(_ItemCursorPos, IsKeyItem: false);
            }
            else
            {
                InventorySelectionSetSoulPosition(_ItemCursorPos, IsKeyItem: true);
            }
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Down) && CurrentSubState == DarkworldMenuSubStates.ItemSelection)
        {
            if (_TopRowCursorPos + 2 < 11)
            {
                _MenuSource.PlayOneShot(snd_menuMove);
            }
            _ItemCursorPos += 2;
            _ItemCursorPos = Mathf.Clamp(_ItemCursorPos, 0, 11);
            if (_ItemOptionCursorPos != 2)
            {
                _ItemCursorPos = Mathf.Clamp(_ItemCursorPos, 0, DarkworldInventory.Instance.PlayerInventory.Count - 1);
                InventorySelectionSetSoulPosition(_ItemCursorPos, IsKeyItem: false);
            }
            else
            {
                _ItemCursorPos = Mathf.Clamp(_ItemCursorPos, 0, DarkworldInventory.Instance.PlayerKeyItems.Count - 1);
                InventorySelectionSetSoulPosition(_ItemCursorPos, IsKeyItem: true);
            }
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm))
        {
            if (CurrentSubState == DarkworldMenuSubStates.ItemSelection)
            {
                _MenuSource.PlayOneShot(snd_select);
                if (_ItemOptionCursorPos != 2)
                {
                    if (DarkworldInventory.Instance.PlayerInventory.Count > 0 && _ItemCursorPos < DarkworldInventory.Instance.PlayerInventory.Count)
                    {
                        StoredItem = DarkworldInventory.Instance.PlayerInventory[_ItemCursorPos];
                    }
                }
                else if (DarkworldInventory.Instance.PlayerKeyItems.Count > 0 && _ItemCursorPos < DarkworldInventory.Instance.PlayerInventory.Count)
                {
                    StoredItem = DarkworldInventory.Instance.PlayerKeyItems[_ItemCursorPos];
                }
                if (_ItemOptionCursorPos == 0)
                {
                    if (DarkworldInventory.Instance.PlayerInventory.Count > 0 && StoredItem.Useable)
                    {
                        SoulSprite.position = new Vector2(-42f, -42f);
                        if (StoredItem.IsSharedItem)
                        {
                            InventorySetMemberSelectionSoul(_ItemMemberCursorPos, SelectAll: true, Selected: true);
                        }
                        else
                        {
                            InventorySetMemberSelectionSoul(_ItemMemberCursorPos, SelectAll: false, Selected: true);
                        }
                        CurrentSubState = DarkworldMenuSubStates.ItemChooseMember;
                    }
                }
                else if (_ItemOptionCursorPos == 1)
                {
                    if (DarkworldInventory.Instance.PlayerInventory.Count > 0)
                    {
                        SoulSprite.position = new Vector2(-42f, -42f);
                        InventorySetMemberSelectionSoul(_ItemMemberCursorPos, SelectAll: true, Selected: true);
                        CurrentSubState = DarkworldMenuSubStates.ItemChooseMember;
                        DisplayTopbarDialogue("Really throw away the " + StoredItem.ItemName + "?");
                    }
                }
                else if (DarkworldInventory.Instance.PlayerKeyItems.Count > 0)
                {
                    MonoBehaviour.print(_ItemCursorPos);
                    MonoBehaviour.print(DarkworldInventory.Instance.PlayerKeyItems[_ItemCursorPos]);
                    PlayerManager.Instance.PlayerINT_Chat.Text = DarkworldInventory.Instance.PlayerKeyItems[_ItemCursorPos].UseText;
                    LightworldMenu.Instance.StartChat(0);
                }
            }
            else if (CurrentSubState == DarkworldMenuSubStates.ItemChooseMember)
            {
                if (_ItemOptionCursorPos == 0)
                {
                    if (StoredItem.Useable)
                    {
                        if (StoredItem.IsSharedItem)
                        {
                            if (StoredItem.ItemUseSound != null)
                            {
                                _MenuSource.PlayOneShot(StoredItem.ItemUseSound);
                            }
                            for (int i = 0; i < CurrentBarPartyMemberPrefabs.Count; i++)
                            {
                                MonoBehaviour.print("currently on " + i);
                                InventoryUseItem(i, playSFX: false);
                            }
                            if (_ItemOptionCursorPos != 2)
                            {
                                if (StoredItem.RemoveOnUse)
                                {
                                    DarkworldInventory.Instance.PlayerInventory.Remove(StoredItem);
                                    InventoryListSetup(IsKeyInventory: false);
                                    StoredItem = null;
                                    _ItemCursorPos = 0;
                                }
                            }
                            else if (StoredItem.RemoveOnUse)
                            {
                                DarkworldInventory.Instance.PlayerKeyItems.Remove(StoredItem);
                                InventoryListSetup(IsKeyInventory: true);
                                StoredItem = null;
                                _ItemCursorPos = 0;
                            }
                        }
                        else
                        {
                            InventoryUseItem(_ItemMemberCursorPos, playSFX: true, AllowDeleteItem: true);
                        }
                    }
                    CurrentSubState = DarkworldMenuSubStates.ItemSelection;
                    InventorySetMemberSelectionSoul(0, SelectAll: true, Selected: false);
                    if (_ItemOptionCursorPos != 2)
                    {
                        InventorySelectionSetSoulPosition(_ItemCursorPos, IsKeyItem: false);
                    }
                    else
                    {
                        InventorySelectionSetSoulPosition(_ItemCursorPos, IsKeyItem: true);
                    }
                }
                else if (_ItemOptionCursorPos == 1)
                {
                    InventoryTossItem(_ItemCursorPos);
                    CurrentSubState = DarkworldMenuSubStates.ItemSelection;
                    InventorySetMemberSelectionSoul(0, SelectAll: true, Selected: false);
                    if (_ItemOptionCursorPos != 2)
                    {
                        InventorySelectionSetSoulPosition(_ItemCursorPos, IsKeyItem: false);
                    }
                    else
                    {
                        InventorySelectionSetSoulPosition(_ItemCursorPos, IsKeyItem: true);
                    }
                }
                else
                {
                    PlayerManager.Instance.PlayerINT_Chat.Text = DarkworldInventory.Instance.PlayerKeyItems[_ItemCursorPos].UseText;
                    LightworldMenu.Instance.StartChat(0);
                    CloseMenu();
                }
            }
            else
            {
                if (_ItemOptionCursorPos != 2)
                {
                    InventorySelectionSetSoulPosition(_ItemCursorPos, IsKeyItem: false);
                }
                else
                {
                    InventorySelectionSetSoulPosition(_ItemCursorPos, IsKeyItem: true);
                }
                switch (_ItemOptionCursorPos)
                {
                    case 0:
                        if (DarkworldInventory.Instance.PlayerInventory.Count > 0)
                        {
                            InventorySetOptionColor(4, Color_Disabled);
                            InventorySetInventoryColor(Color_Enabled);
                            _MenuSource.PlayOneShot(snd_select);
                            InventorySetOptionColor(0, Color_Highlighted);
                            CurrentSubState = DarkworldMenuSubStates.ItemSelection;
                        }
                        break;
                    case 1:
                        if (DarkworldInventory.Instance.PlayerInventory.Count > 0)
                        {
                            InventorySetOptionColor(4, Color_Disabled);
                            InventorySetInventoryColor(Color_Enabled);
                            _MenuSource.PlayOneShot(snd_select);
                            InventorySetOptionColor(1, Color_Highlighted);
                            CurrentSubState = DarkworldMenuSubStates.ItemSelection;
                        }
                        break;
                    case 2:
                        if (DarkworldInventory.Instance.PlayerKeyItems.Count > 0)
                        {
                            InventorySetOptionColor(4, Color_Disabled);
                            InventorySetInventoryColor(Color_Enabled);
                            _MenuSource.PlayOneShot(snd_select);
                            InventorySetOptionColor(2, Color_Highlighted);
                            CurrentSubState = DarkworldMenuSubStates.ItemSelection;
                        }
                        break;
                }
            }
        }
        if (previousItemOptionPos != _ItemOptionCursorPos)
        {
            InventoryOnOptionChange();
            previousItemOptionPos = _ItemCursorPos;
        }
    }

    private void InventoryOnOptionChange()
    {
        switch (_ItemOptionCursorPos)
        {
            case 0:
                InventoryListSetup(IsKeyInventory: false);
                break;
            case 1:
                InventoryListSetup(IsKeyInventory: false);
                break;
            case 2:
                InventoryListSetup(IsKeyInventory: true);
                break;
        }
    }

    private void InventoryListSetup(bool IsKeyInventory)
    {
        TextMeshProUGUI[] inventoryTextSlots = _InventoryTextSlots;
        for (int i = 0; i < inventoryTextSlots.Length; i++)
        {
            inventoryTextSlots[i].text = "";
        }
        if (IsKeyInventory)
        {
            for (int j = 0; j < DarkworldInventory.Instance.PlayerKeyItems.Count; j++)
            {
                if (DarkworldInventory.Instance.PlayerKeyItems.Count > 0 && DarkworldInventory.Instance.PlayerKeyItems[j] != null && DarkworldInventory.Instance.PlayerKeyItems[j].ItemName != "")
                {
                    _InventoryTextSlots[j].text = DarkworldInventory.Instance.PlayerKeyItems[j].ItemName;
                }
                else
                {
                    _InventoryTextSlots[j].text = "";
                }
            }
            return;
        }
        for (int k = 0; k < DarkworldInventory.Instance.PlayerInventory.Count; k++)
        {
            if (DarkworldInventory.Instance.PlayerInventory.Count > 0 && DarkworldInventory.Instance.PlayerInventory[k] != null && DarkworldInventory.Instance.PlayerInventory[k].ItemName != "")
            {
                _InventoryTextSlots[k].text = DarkworldInventory.Instance.PlayerInventory[k].ItemName;
            }
            else
            {
                _InventoryTextSlots[k].text = "";
            }
        }
    }

    private void InventorySelectionSetSoulPosition(int index, bool IsKeyItem)
    {
        if (IsKeyItem)
        {
            _ItemCursorPos = Mathf.Clamp(_ItemCursorPos, 0, DarkworldInventory.Instance.PlayerKeyItems.Count);
            if (index > DarkworldInventory.Instance.PlayerKeyItems.Count - 1)
            {
                index = DarkworldInventory.Instance.PlayerKeyItems.Count - 1;
                _ItemCursorPos = DarkworldInventory.Instance.PlayerKeyItems.Count - 1;
            }
            if (DarkworldInventory.Instance.PlayerKeyItems.Count > 0 && DarkworldInventory.Instance.PlayerKeyItems.Count > 0)
            {
                SoulSprite.position = _InventoryTextSlots[index].transform.position + new Vector3(-200f, 0f, 0f);
                DisplayTopbarDialogue(DarkworldInventory.Instance.PlayerKeyItems[index].TopbarInfo);
            }
        }
        else
        {
            _ItemCursorPos = Mathf.Clamp(_ItemCursorPos, 0, DarkworldInventory.Instance.PlayerInventory.Count);
            if (index > DarkworldInventory.Instance.PlayerInventory.Count - 1)
            {
                index = DarkworldInventory.Instance.PlayerInventory.Count;
                _ItemCursorPos = DarkworldInventory.Instance.PlayerInventory.Count;
            }
            if (DarkworldInventory.Instance.PlayerInventory.Count > 0 && _InventoryTextSlots.Length != 0 && _InventoryTextSlots[index] != null)
            {
                SoulSprite.position = _InventoryTextSlots[index].transform.position + new Vector3(-200f, 0f, 0f);
                DisplayTopbarDialogue(DarkworldInventory.Instance.PlayerInventory[index].TopbarInfo);
            }
        }
    }

    private void InventorySetMemberSelectionSoul(int index, bool SelectAll, bool Selected)
    {
        foreach (DarkworldMenu_BarPartyMember currentBarPartyMemberPrefab in CurrentBarPartyMemberPrefabs)
        {
            currentBarPartyMemberPrefab.selectedIcon.enabled = false;
        }
        if (SelectAll)
        {
            foreach (DarkworldMenu_BarPartyMember currentBarPartyMemberPrefab2 in CurrentBarPartyMemberPrefabs)
            {
                currentBarPartyMemberPrefab2.selectedIcon.enabled = Selected;
            }
            return;
        }
        CurrentBarPartyMemberPrefabs[index].selectedIcon.enabled = Selected;
    }

    private void InventoryOptionsSetSoulPosition(int index)
    {
        RemoveTopbarDialogue();
        SoulSprite.position = _InventoryOptions[index].transform.position + new Vector3(-100f, 0f, 0f);
    }

    private void InventorySetOptionColor(int index, Color color)
    {
        if (index > 2)
        {
            TextMeshProUGUI[] inventoryOptions = _InventoryOptions;
            for (int i = 0; i < inventoryOptions.Length; i++)
            {
                inventoryOptions[i].color = color;
            }
        }
        else
        {
            _InventoryOptions[index].color = color;
        }
    }

    private void InventorySetInventoryColor(Color color)
    {
        TextMeshProUGUI[] inventoryTextSlots = _InventoryTextSlots;
        for (int i = 0; i < inventoryTextSlots.Length; i++)
        {
            inventoryTextSlots[i].color = color;
        }
    }

    private void InventoryUseItem(int index, bool playSFX, bool AllowDeleteItem = false)
    {
        if (playSFX && StoredItem.ItemUseSound != null)
        {
            _MenuSource.PlayOneShot(StoredItem.ItemUseSound);
        }
        if (StoredItem.HealthAddition != 0)
        {
            CurrentBarPartyMemberPrefabs[index].addhealthText.text = "+" + StoredItem.HealthAddition;
            CurrentBarPartyMemberPrefabs[index].addhealthAnimator.enabled = true;
            CurrentBarPartyMemberPrefabs[index].addhealthAnimator.Play("DWM_Inventory_HealthAdded", -1, 0f);
            if (CurrentBarPartyMemberPrefabs[index].CurrentMember.PartyMemberDescription.StartFromPlayer)
            {
                PlayerManager.Instance._PlayerHealth = Mathf.Clamp(PlayerManager.Instance._PlayerHealth + (float)StoredItem.HealthAddition, 0f, PlayerManager.Instance._PlayerMaxHealth);
            }
            else
            {
                CurrentBarPartyMemberPrefabs[index].CurrentMember.CurrentHealth = Mathf.Clamp(CurrentBarPartyMemberPrefabs[index].CurrentMember.CurrentHealth + StoredItem.HealthAddition, 0, CurrentBarPartyMemberPrefabs[index].CurrentMember.PartyMemberDescription.MaximumHealth);
            }
            CurrentBarPartyMemberPrefabs[index].UpdateHealth(CurrentBarPartyMemberPrefabs[index].CurrentMember);
        }
        if (_ItemOptionCursorPos != 2)
        {
            if (StoredItem.RemoveOnUse && AllowDeleteItem)
            {
                DarkworldInventory.Instance.PlayerInventory.Remove(StoredItem);
                InventoryListSetup(IsKeyInventory: false);
                StoredItem = null;
                _ItemCursorPos = 0;
            }
        }
        else if (StoredItem.RemoveOnUse && AllowDeleteItem)
        {
            DarkworldInventory.Instance.PlayerKeyItems.Remove(StoredItem);
            InventoryListSetup(IsKeyInventory: true);
            StoredItem = null;
            _ItemCursorPos = 0;
        }
    }

    private void InventoryTossItem(int index)
    {
        if (_ItemOptionCursorPos != 2)
        {
            DarkworldInventory.Instance.PlayerInventory.Remove(StoredItem);
            InventoryListSetup(IsKeyInventory: false);
            StoredItem = null;
        }
        else
        {
            DarkworldInventory.Instance.PlayerKeyItems.Remove(StoredItem);
            InventoryListSetup(IsKeyInventory: true);
            StoredItem = null;
        }
        _ItemCursorPos = 0;
    }

    private void DisplayPartyMemberDialogue()
    {
        foreach (DarkworldMenu_BarPartyMember currentBarPartyMemberPrefab in CurrentBarPartyMemberPrefabs)
        {
            _ = currentBarPartyMemberPrefab;
        }
        ActivePartyMember currentMember = CurrentBarPartyMemberPrefabs[_ItemMemberCursorPos].CurrentMember;
        foreach (DarkworldMenu_BarPartyMember currentBarPartyMemberPrefab2 in CurrentBarPartyMemberPrefabs)
        {
            if (currentMember.PartyMemberDescription == PartyMemberSystem.Instance.Default_Kris)
            {
                if (currentBarPartyMemberPrefab2.CurrentMember.PartyMemberDescription == PartyMemberSystem.Instance.Default_Ralsei)
                {
                    currentBarPartyMemberPrefab2.addhealthText.text = StoredItem.CharacterDialogue.KrisItemUsed_RalseiDialogue;
                }
                if (currentBarPartyMemberPrefab2.CurrentMember.PartyMemberDescription == PartyMemberSystem.Instance.Default_SusieDarkworld)
                {
                    currentBarPartyMemberPrefab2.addhealthText.text = StoredItem.CharacterDialogue.KrisItemUsed_SusieDialogue;
                }
            }
            if (currentMember.PartyMemberDescription == PartyMemberSystem.Instance.Default_Kris)
            {
                if (currentBarPartyMemberPrefab2.CurrentMember.PartyMemberDescription == PartyMemberSystem.Instance.Default_Ralsei)
                {
                    currentBarPartyMemberPrefab2.addhealthText.text = StoredItem.CharacterDialogue.SusieItemUsed_RalseiDialogue;
                }
                if (currentBarPartyMemberPrefab2.CurrentMember.PartyMemberDescription == PartyMemberSystem.Instance.Default_SusieDarkworld)
                {
                    currentBarPartyMemberPrefab2.addhealthText.text = StoredItem.CharacterDialogue.SusieItemUsed_SusieDialogue;
                }
            }
            if (currentMember.PartyMemberDescription == PartyMemberSystem.Instance.Default_Kris)
            {
                if (currentBarPartyMemberPrefab2.CurrentMember.PartyMemberDescription == PartyMemberSystem.Instance.Default_Ralsei)
                {
                    currentBarPartyMemberPrefab2.addhealthText.text = StoredItem.CharacterDialogue.RalseiItemUsed_RalseiDialogue;
                }
                if (currentBarPartyMemberPrefab2.CurrentMember.PartyMemberDescription == PartyMemberSystem.Instance.Default_SusieDarkworld)
                {
                    currentBarPartyMemberPrefab2.addhealthText.text = StoredItem.CharacterDialogue.RalseiItemUsed_SusieDialogue;
                }
            }
        }
    }

    public void DisplayTopbarDialogue(string Dialogue)
    {
        _TopBarDialogueObject.SetActive(value: true);
        _TopBarText.text = Dialogue;
    }

    public void RemoveTopbarDialogue()
    {
        _TopBarDialogueObject.SetActive(value: false);
        _TopBarText.text = "";
    }

    public void OpenMenu()
    {
        _Anim_MainSection.Play("DWM_Opened", -1, 0f);
        CurrentState = DarkworldMenuStates.TopRow;
        CurrentSubState = DarkworldMenuSubStates.Disabled;
        previousPlayerState = PlayerManager.Instance._PlayerState;
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.NoPlayerMovement;
        MenuOpen = true;
        GameObject gameObject = Object.Instantiate(BarPartyMemberPrefab, BottomRow.transform);
        ActivePartyMember activePartyMember = new ActivePartyMember();
        activePartyMember.PartyMemberDescription = PartyMemberSystem.Instance.Default_Kris;
        activePartyMember.CurrentHealth = (int)PlayerManager.Instance._PlayerHealth;
        gameObject.GetComponent<DarkworldMenu_BarPartyMember>().IsPlayers = true;
        gameObject.GetComponent<DarkworldMenu_BarPartyMember>().SetupPartyMember(activePartyMember);
        CurrentBarPartyMemberPrefabs.Add(gameObject.GetComponent<DarkworldMenu_BarPartyMember>());
        foreach (ActivePartyMember activePartyMember2 in PartyMemberSystem.Instance.ActivePartyMembers)
        {
            GameObject gameObject2 = Object.Instantiate(BarPartyMemberPrefab, BottomRow.transform);
            gameObject2.GetComponent<DarkworldMenu_BarPartyMember>().SetupPartyMember(activePartyMember2);
            CurrentBarPartyMemberPrefabs.Add(gameObject2.GetComponent<DarkworldMenu_BarPartyMember>());
        }
    }

    public void CloseMenu()
    {
        if (PreviousMenuState)
        {
            _Anim_MainSection.Play("DWM_Closed", -1, 0f);
        }
        else
        {
            _Anim_MainSection.Play("DWM_Closed", -1, 1f);
        }
        SetupSelectedMenu(DarkworldMenuStates.Disabled);
        StartCoroutine(InputDebounce());
        foreach (DarkworldMenu_BarPartyMember currentBarPartyMemberPrefab in CurrentBarPartyMemberPrefabs)
        {
            Object.Destroy(currentBarPartyMemberPrefab.gameObject);
        }
        CurrentBarPartyMemberPrefabs.Clear();
        if (!ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            PlayerManager.Instance._PlayerState = previousPlayerState;
        }
        InventorySetInventoryColor(Color_Disabled);
        InventorySetOptionColor(4, Color_Enabled);
        _ItemCursorPos = 0;
        _ItemOptionCursorPos = 0;
        MenuOpen = false;
    }

    private IEnumerator InputDebounce()
    {
        ForceDisableInput = true;
        yield return new WaitForSeconds(0.2f);
        ForceDisableInput = false;
        HasEnabledInputCooldown = false;
    }
}
