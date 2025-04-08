using System.Collections;
using UnityEngine;

public class Trailer_PapyrusCutscene : MonoBehaviour
{
    [SerializeField]
    private int CutsceneIndex;

    [Header("-- References --")]
    [SerializeField]
    private Transform PapyrusTransform;

    [SerializeField]
    private Animator PapyrusAnimator;

    [SerializeField]
    private Animator PapyrusArmorAnimator;

    [SerializeField]
    private Animator SansArmorAnimator;

    [SerializeField]
    private Animator SansAnimator;

    [SerializeField]
    private Susie_Follower Susie;

    [SerializeField]
    private PlayerManager Kris;

    [SerializeField]
    private GameObject Sans;

    [SerializeField]
    private GameObject Candle;

    [SerializeField]
    private GameObject TableLighting;

    [SerializeField]
    private GameObject DarkRoomObjects;

    [SerializeField]
    private GameObject LightRoomObjects;

    [Header("-- Settings --")]
    [SerializeField]
    private Vector2[] KrisWalkPositions;

    [SerializeField]
    private Vector2[] SusieWalkPositions;

    [Space(10f)]
    [SerializeField]
    private Vector2[] KrisWalkDirections;

    [SerializeField]
    private Vector2[] SusieWalkDirections;

    [Header("-- Cutscene Chats --")]
    [SerializeField]
    private INT_Chat CutsceneChatter;

    [SerializeField]
    private CHATBOXTEXT[] CutsceneChats;

    [Header("-- Cutscene Sounds --")]
    [SerializeField]
    private AudioSource CutsceneSource;

    [SerializeField]
    private AudioClip[] CutsceneSounds;

    public string ImgName;

    private void Start()
    {
        StartCutscene();
    }

    private void Update()
    {
        if (CutsceneIndex > 0)
        {
            Kris._PlayerState = PlayerManager.PlayerState.Cutscene;
            LightworldMenu.Instance.CanOpenMenu = false;
            Susie.FollowingEnabled = false;
            Susie.AnimationOverriden = false;
        }
        if (CutsceneChatter.CurrentlyBeingUsed)
        {
            if (ChatboxManager.Instance.storedchatboxtext.Textboxes[ChatboxManager.Instance.CurrentAdditionalTextIndex].Character[ChatboxManager.Instance.CurrentTextIndex].CharacterHasTalkingAnimation)
            {
                PapyrusAnimator.SetBool("Talking", value: true);
            }
            else
            {
                PapyrusAnimator.SetBool("Talking", value: false);
            }
        }
        else
        {
            PapyrusAnimator.SetBool("Talking", value: false);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            RunFreshChat(CutsceneChatter.Text, 0, ForcePosition: false, OnBottom: false);
        }
    }

    public void StartCutscene()
    {
        StartCoroutine(Cutscene());
    }

    public void IncrementCutsceneIndex()
    {
        CutsceneIndex++;
    }

    private void RunFreshChat(CHATBOXTEXT text, int index, bool ForcePosition, bool OnBottom)
    {
        CutsceneChatter.FirstTextPlayed = false;
        CutsceneChatter.CurrentIndex = index;
        CutsceneChatter.FinishedText = false;
        CutsceneChatter.Text = text;
        if (ForcePosition)
        {
            CutsceneChatter.ManualTextboxPosition = true;
            CutsceneChatter.OnBottom = OnBottom;
        }
        CutsceneChatter.RUN();
    }

    private IEnumerator Cutscene()
    {
        yield return new WaitForSeconds(0.2f);
        Kris = PlayerManager.Instance;
        ActivePartyMember activePartyMember = PartyMemberSystem.Instance.HasMemberInParty(PartyMemberSystem.Instance.Default_Susie);
        if (activePartyMember != null)
        {
            MonoBehaviour.print(activePartyMember.PartyMemberDescription);
            Susie = activePartyMember.PartyMemberFollowSettings;
            Susie.transform.position = new Vector2(-0.6f, -3.55f);
        }
        else
        {
            Debug.LogError("Susie is not in the party?????");
        }
        IncrementCutsceneIndex();
    }

    public void PapyrusKnight_Stand()
    {
        PapyrusAnimator.Play("Papyrus_Knight_Stand");
    }

    public void PapyrusKnight_Pose()
    {
        PapyrusAnimator.Play("Papyrus_Knight_Pose");
    }

    public void PapyrusKnight_Laugh()
    {
        PapyrusAnimator.Play("Papyrus_Knight_Laugh");
    }

    public void PapyrusKnight_Paper()
    {
        PapyrusAnimator.Play("Papyrus_Knight_Paper");
    }

    public void Papyrus_Idle_Up()
    {
        PapyrusAnimator.Play("Papyrus_Idle_Up");
        float normalizedTime = PapyrusArmorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        PapyrusArmorAnimator.Play("MP_PapyrusRoom_PapyrusArmor_Up", -1, normalizedTime);
    }

    public void Papyrus_Idle_Right()
    {
        PapyrusAnimator.Play("Trailer_Papyrus_Idle_Right");
        float normalizedTime = PapyrusArmorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        PapyrusArmorAnimator.Play("MP_PapyrusRoom_PapyrusArmor_Right", -1, normalizedTime);
    }

    public void Papyrus_Idle_Left()
    {
        PapyrusAnimator.Play("Trailer_Papyrus_Idle_Left");
        float normalizedTime = PapyrusArmorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        PapyrusArmorAnimator.Play("MP_PapyrusRoom_PapyrusArmor_Left", -1, normalizedTime);
    }

    public void Papyrus_Idle_Down()
    {
        PapyrusAnimator.Play("Trailer_Papyrus_Idle_Down");
        float normalizedTime = PapyrusArmorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        PapyrusArmorAnimator.Play("MP_PapyrusRoom_PapyrusArmor_Down", -1, normalizedTime);
    }

    public void Sans_Idle_Right()
    {
        SansAnimator.Play("Trailer_Sans_Idle_Right");
        SansArmorAnimator.Play("MP_PapyrusRoom_SansArmor_Right");
    }

    public void Sans_Idle_Down()
    {
        SansAnimator.Play("Trailer_Sans_Idle_Down");
        SansArmorAnimator.Play("MP_PapyrusRoom_SansArmor_Down");
    }
}
