using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class LightworldMenu : MonoBehaviour
{
    public PlayerManager PlayerManager;

    [Header("UI")]
    [Space(15f)]
    public CHATBOXTEXT[] QueuedCharacterCalls;

    public CHATBOXTEXT NoResponseCall;

    public INT_Chat PlayerCallChat;

    [Space(15f)]
    public Vector3[] ItemMenuOptions;

    public Vector3[] ItemMenuSelectedOptions;

    public TextMeshProUGUI[] ItemInventoryText;

    private int currentItemMenuState;

    private int currentSelectedItem;

    private int InventoryLength;

    private int LastItemSelectedIndex;

    [Space(15f)]
    public bool IsInLightworld = true;

    public GameObject MenuHolder;

    public RectTransform[] BaseMenuOptions;

    [SerializeField]
    private PlayerManager.PlayerState PreviousPlayerState;

    public Transform[] CallMenuOptions;

    public int[] CallChatIndexes;

    public string[] CallMenuNames;

    [Space(15f)]
    public GameObject LightworldStats;

    public GameObject LightworldCall;

    public GameObject LightworldItem;

    public bool CanOpenMenu;

    public bool MenuOpen;

    public bool AllowInput = true;

    private bool MenuToggleCooldown;

    public string CurrentOpenMenu;

    public RectTransform HeartCursor;

    private int CursorPos;

    public AudioSource MenuSound;

    private bool ChatboxRunning;

    public AudioClip CantSelectSound;

    public AudioClip SelectSound;

    public AudioClip ClickSound;

    public AudioClip RingSound;

    private InventoryItem CurrentHoveredItem;

    private static LightworldMenu instance;

    public static LightworldMenu Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Object.Destroy(base.gameObject);
            return;
        }
        instance = this;
        Object.DontDestroyOnLoad(instance);
    }

    private void Start()
    {
        PlayerManager = Object.FindFirstObjectByType<PlayerManager>();
        if (PlayerManager != null)
        {
            PlayerCallChat = PlayerManager.PlayerINT_Chat;
            MenuSound = PlayerManager.GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (PlayerManager == null)
        {
            PlayerManager = Object.FindFirstObjectByType<PlayerManager>();
            PlayerCallChat = PlayerManager.PlayerINT_Chat;
            MenuSound = PlayerManager.GetComponent<AudioSource>();
        }
        if (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            ChatboxRunning = true;
        }
        else if (ChatboxRunning)
        {
            StartCoroutine(ChatboxToLightworldMenuDebounce());
        }
        else
        {
            ChatboxRunning = false;
        }
        if (!ChatboxRunning)
        {
            ProcessInputs();
        }
        if (MenuOpen)
        {
            if (PlayerManager != null && PlayerManager._PlayerState == PlayerManager.PlayerState.Game)
            {
                ForceCloseMenu();
            }
            else
            {
                LightworldMenuUpdate();
            }
        }
    }

    private void ProcessInputs()
    {
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Menu) && !MenuToggleCooldown && PlayerManager._PlayerState != PlayerManager.PlayerState.Cutscene && !ChatboxManager.Instance.ChatIsCurrentlyRunning && CurrentOpenMenu == "" && IsInLightworld && AllowInput && CanOpenMenu)
        {
            MenuOpen = !MenuOpen;
            MenuHolder.SetActive(MenuOpen);
            if (MenuOpen)
            {
                PreviousPlayerState = PlayerManager._PlayerState;
                PlayerManager._PlayerState = PlayerManager.PlayerState.NoPlayerMovement;
            }
            else
            {
                PlayerManager._PlayerState = PreviousPlayerState;
            }
            MenuToggleCooldown = true;
            StartCoroutine(ToggleMenuCooldown());
        }
    }

    private IEnumerator ToggleMenuCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        MenuToggleCooldown = false;
    }

    private IEnumerator ChatboxToLightworldMenuDebounce()
    {
        yield return new WaitForSeconds(0.2f);
        ChatboxRunning = false;
    }

    private void LightworldMenuUpdate()
    {
        switch (CurrentOpenMenu)
        {
            case "call":
                {
                    if (Input.GetKeyDown(PlayerInput.Instance.Key_Up) && CurrentOpenMenu != "stat" && AllowInput)
                    {
                        MenuSound.PlayOneShot(SelectSound);
                        CursorPos--;
                    }
                    if (Input.GetKeyDown(PlayerInput.Instance.Key_Down) && CurrentOpenMenu != "stat" && AllowInput)
                    {
                        MenuSound.PlayOneShot(SelectSound);
                        CursorPos++;
                    }
                    SetupCallNames();
                    int num = 0;
                    string[] callMenuNames = CallMenuNames;
                    for (int i = 0; i < callMenuNames.Length; i++)
                    {
                        if (callMenuNames[i] != "")
                        {
                            num++;
                        }
                    }
                    CursorPos = Mathf.Clamp(CursorPos, 0, num - 1);
                    HeartCursor.position = CallMenuOptions[CursorPos].position + new Vector3(-55f, -3f, 0f);
                    LightworldCall.SetActive(value: true);
                    if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm) && AllowInput)
                    {
                        if (QueuedCharacterCalls[CursorPos] != null && CallChatIndexes[CursorPos] < QueuedCharacterCalls[CursorPos].Textboxes.Length)
                        {
                            PlayerCallChat.Text = QueuedCharacterCalls[CursorPos];
                            PlayerCallChat.CurrentIndex = CallChatIndexes[CursorPos];
                        }
                        else
                        {
                            PlayerCallChat.Text = NoResponseCall;
                            PlayerCallChat.CurrentIndex = 0;
                        }
                        InitiateCall();
                        MenuSound.PlayOneShot(ClickSound);
                    }
                    if (Input.GetKeyDown(PlayerInput.Instance.Key_Cancel) && PlayerManager._PlayerState != PlayerManager.PlayerState.Cutscene && AllowInput)
                    {
                        CursorPos = 2;
                        CurrentOpenMenu = "";
                    }
                    return;
                }
            case "stat":
                if (Input.GetKeyDown(PlayerInput.Instance.Key_Up) && CurrentOpenMenu != "stat" && AllowInput)
                {
                    MenuSound.PlayOneShot(SelectSound);
                    CursorPos--;
                }
                if (Input.GetKeyDown(PlayerInput.Instance.Key_Down) && CurrentOpenMenu != "stat" && AllowInput)
                {
                    MenuSound.PlayOneShot(SelectSound);
                    CursorPos++;
                }
                HeartCursor.localPosition = new Vector2(999f, 999f);
                LightworldStats.SetActive(value: true);
                CursorPos = 1;
                if (Input.GetKeyDown(PlayerInput.Instance.Key_Cancel) && AllowInput)
                {
                    CursorPos = 1;
                    CurrentOpenMenu = "";
                }
                return;
            case "item":
                if (InventoryLength > 0)
                {
                    if (currentItemMenuState == 0)
                    {
                        if (Input.GetKeyDown(PlayerInput.Instance.Key_Up) && CurrentOpenMenu != "stat" && AllowInput)
                        {
                            MenuSound.PlayOneShot(SelectSound);
                            CursorPos--;
                            CursorPos = Mathf.Clamp(CursorPos, 0, InventoryLength - 1);
                        }
                        if (Input.GetKeyDown(PlayerInput.Instance.Key_Down) && CurrentOpenMenu != "stat" && AllowInput)
                        {
                            MenuSound.PlayOneShot(SelectSound);
                            CursorPos++;
                            CursorPos = Mathf.Clamp(CursorPos, 0, InventoryLength - 1);
                        }
                        CurrentHoveredItem = LightworldInventory.Instance.PlayerInventory[CursorPos];
                        CursorPos = Mathf.Clamp(CursorPos, 0, InventoryLength - 1);
                        HeartCursor.localPosition = ItemMenuOptions[CursorPos];
                        LastItemSelectedIndex = CursorPos;
                    }
                }
                else
                {
                    CursorPos = 1;
                    HeartCursor.localPosition = new Vector2(-500f, -500f);
                }
                if (currentItemMenuState == 1)
                {
                    if (Input.GetKeyDown(PlayerInput.Instance.Key_Left) && CurrentOpenMenu != "stat" && AllowInput)
                    {
                        MenuSound.PlayOneShot(SelectSound);
                        CursorPos--;
                    }
                    if (Input.GetKeyDown(PlayerInput.Instance.Key_Right) && CurrentOpenMenu != "stat" && AllowInput)
                    {
                        MenuSound.PlayOneShot(SelectSound);
                        CursorPos++;
                    }
                    CursorPos = Mathf.Clamp(CursorPos, 0, ItemMenuSelectedOptions.Length - 1);
                    HeartCursor.localPosition = ItemMenuSelectedOptions[CursorPos];
                }
                LightworldItem.SetActive(value: true);
                if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm) && AllowInput)
                {
                    if (currentItemMenuState == 1)
                    {
                        ResetCallChat();
                        if (CursorPos == 0)
                        {
                            if (CurrentHoveredItem != null && CurrentHoveredItem.UseText != null)
                            {
                                PlayerCallChat.Text = CurrentHoveredItem.UseText;
                                StartChat(0);
                            }
                            else
                            {
                                MenuSound.PlayOneShot(CantSelectSound);
                            }
                        }
                        else if (CursorPos == 1)
                        {
                            if (CurrentHoveredItem != null && CurrentHoveredItem.InfoText != null)
                            {
                                PlayerCallChat.Text = CurrentHoveredItem.InfoText;
                                StartChat(0);
                            }
                            else
                            {
                                MenuSound.PlayOneShot(CantSelectSound);
                            }
                        }
                        else if (CurrentHoveredItem != null && CurrentHoveredItem.DropText != null)
                        {
                            PlayerCallChat.Text = CurrentHoveredItem.DropText;
                            StartChat(0);
                        }
                        else
                        {
                            MenuSound.PlayOneShot(CantSelectSound);
                        }
                    }
                    if (currentItemMenuState == 0 && InventoryLength > 0)
                    {
                        currentSelectedItem = CursorPos;
                        CursorPos = 0;
                        currentItemMenuState = 1;
                    }
                    MenuSound.PlayOneShot(ClickSound);
                }
                if (Input.GetKeyDown(PlayerInput.Instance.Key_Cancel) && PlayerManager._PlayerState != PlayerManager.PlayerState.Cutscene && AllowInput)
                {
                    if (currentItemMenuState == 0)
                    {
                        CursorPos = 0;
                        CurrentOpenMenu = "";
                    }
                    if (currentItemMenuState == 1)
                    {
                        CursorPos = currentSelectedItem;
                        currentItemMenuState = 0;
                    }
                }
                return;
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Up) && CurrentOpenMenu != "stat" && AllowInput)
        {
            MenuSound.PlayOneShot(SelectSound);
            CursorPos--;
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Down) && CurrentOpenMenu != "stat" && AllowInput)
        {
            MenuSound.PlayOneShot(SelectSound);
            CursorPos++;
        }
        CursorPos = Mathf.Clamp(CursorPos, 0, BaseMenuOptions.Length - 1);
        HeartCursor.localPosition = new Vector2(BaseMenuOptions[CursorPos].anchoredPosition.x - 59f, BaseMenuOptions[CursorPos].anchoredPosition.y - 17f);
        LightworldCall.SetActive(value: false);
        LightworldStats.SetActive(value: false);
        LightworldItem.SetActive(value: false);
        currentItemMenuState = 0;
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm) && AllowInput)
        {
            if (CursorPos == 0)
            {
                SetupInventoryText();
                MenuSound.PlayOneShot(ClickSound);
                CurrentOpenMenu = "item";
                CursorPos = 0;
            }
            else if (CursorPos == 1)
            {
                MenuSound.PlayOneShot(ClickSound);
                CurrentOpenMenu = "stat";
                CursorPos = 1;
            }
            else if (CursorPos == 2)
            {
                MenuSound.PlayOneShot(ClickSound);
                CurrentOpenMenu = "call";
                CursorPos = 0;
                ResetCallChat();
            }
        }
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Cancel) && PlayerManager._PlayerState != PlayerManager.PlayerState.Cutscene && AllowInput)
        {
            MenuOpen = false;
            MenuHolder.SetActive(value: false);
            LightworldCall.SetActive(value: false);
            LightworldStats.SetActive(value: false);
            CurrentOpenMenu = "";
            PlayerManager._PlayerState = PlayerManager.PlayerState.Game;
        }
    }

    private void InitiateCall()
    {
        MenuOpen = false;
        MenuHolder.SetActive(value: false);
        LightworldCall.SetActive(value: false);
        LightworldStats.SetActive(value: false);
        CurrentOpenMenu = "";
        MenuSound.PlayOneShot(RingSound);
        PlayerManager._PlayerState = PlayerManager.PlayerState.Game;
        PlayerCallChat.RUN();
        CallChatIndexes[CursorPos]++;
        CursorPos = 0;
    }

    public void ForceCloseMenu()
    {
        MenuOpen = false;
        MenuHolder.SetActive(value: false);
        LightworldCall.SetActive(value: false);
        LightworldStats.SetActive(value: false);
        CurrentOpenMenu = "";
        CursorPos = 0;
    }

    public void StartChat(int index)
    {
        MenuOpen = false;
        CursorPos = 0;
        PlayerCallChat.FirstTextPlayed = false;
        MenuHolder.SetActive(value: false);
        LightworldCall.SetActive(value: false);
        LightworldStats.SetActive(value: false);
        PlayerCallChat.CurrentIndex = index;
        CurrentOpenMenu = "";
        PlayerManager._PlayerState = PlayerManager.PlayerState.Game;
        PlayerCallChat.RUN();
    }

    public void DropLastSelectedItem()
    {
        if (LightworldInventory.Instance.PlayerInventory.Count > 0 && LightworldInventory.Instance.PlayerInventory[LastItemSelectedIndex] != null)
        {
            LightworldInventory.Instance.RemoveItem(LastItemSelectedIndex);
        }
    }

    private void SetupInventoryText()
    {
        InventoryLength = 0;
        for (int i = 0; i < LightworldInventory.Instance.PlayerInventory.Count; i++)
        {
            if (LightworldInventory.Instance.PlayerInventory.Count > 0 && LightworldInventory.Instance.PlayerInventory[i] != null && LightworldInventory.Instance.PlayerInventory[i].ItemName != "")
            {
                ItemInventoryText[i].text = LightworldInventory.Instance.PlayerInventory[i].ItemName;
                InventoryLength++;
                ItemInventoryText[i].text = Regex.Replace(ItemInventoryText[i].text, "([‘’“”\\-\\u201A-\\u201F\\u4E00-\\u9FFF\\u3000-\\u303F\\uFF00-\\uFFEF])", "$1 ");
                ItemInventoryText[i].text = Regex.Replace(ItemInventoryText[i].text, "([a-zA-Z0-9\\.\\-@#])([‘’“”\\u201A-\\u201F\\u4E00-\\u9FFF\\u3000-\\u303F\\uFF00-\\uFFEF])", "$1 $2");
            }
            else
            {
                ItemInventoryText[i].text = "";
            }
        }
    }

    private void ResetCallChat()
    {
        PlayerCallChat.CanUse = true;
        PlayerCallChat.FinishedText = false;
        PlayerCallChat.FirstTextPlayed = false;
        PlayerCallChat.CurrentIndex = 0;
    }

    private void SetupCallNames()
    {
        for (int i = 0; i < CallMenuOptions.Length; i++)
        {
            if (i < QueuedCharacterCalls.Length && CallMenuNames[i] != "" && CallMenuNames[i] != null)
            {
                if (CallMenuNames[i].Contains('#'))
                {
                    string text = CallMenuNames[i].Replace("#", string.Empty);
                    CallMenuOptions[i].GetComponent<TextMeshProUGUI>().text = text;
                }
                else
                {
                    CallMenuOptions[i].GetComponent<TextMeshProUGUI>().text = "给" + CallMenuNames[i] + "打电话";
                }
                CallMenuOptions[i].GetComponent<TextMeshProUGUI>().text = Regex.Replace(CallMenuOptions[i].GetComponent<TextMeshProUGUI>().text, "([‘’“”\\-\\u201A-\\u201F\\u4E00-\\u9FFF\\u3000-\\u303F\\uFF00-\\uFFEF])", "$1 ");
                CallMenuOptions[i].GetComponent<TextMeshProUGUI>().text = Regex.Replace(CallMenuOptions[i].GetComponent<TextMeshProUGUI>().text, "([a-zA-Z0-9\\.\\-@#])([‘’“”\\u201A-\\u201F\\u4E00-\\u9FFF\\u3000-\\u303F\\uFF00-\\uFFEF])", "$1 $2");
            }
            else
            {
                CallMenuOptions[i].GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }
}
