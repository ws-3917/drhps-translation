using System.Collections;
using UnityEngine;

public class PapyrusRoom_AmbushGreetCutscene : MonoBehaviour
{
    [SerializeField]
    private int CutsceneIndex;

    [Header("-- References --")]
    [SerializeField]
    private Transform PapyrusTransform;

    [SerializeField]
    private Animator PapyrusAngryEyebrows;

    [SerializeField]
    private Animator PapyrusAnimator;

    [SerializeField]
    private Animator PapyrusArmorAnimator;

    [SerializeField]
    private Animator SansArmorAnimator;

    [SerializeField]
    private Susie_Follower Susie;

    [SerializeField]
    private PlayerManager Kris;

    [SerializeField]
    private Transform Sans;

    [SerializeField]
    private Animator SansAnimator;

    [SerializeField]
    private GameObject Candle;

    [SerializeField]
    private GameObject TableLighting;

    [SerializeField]
    private GameObject DarkRoomObjects;

    [SerializeField]
    private GameObject LightRoomObjects;

    [SerializeField]
    private PapyrusRoom_RoomTourCutscene RoomTourCutscene;

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

    [Space(10f)]
    [SerializeField]
    private Vector2[] PapyrusWalkDirections;

    [SerializeField]
    private Vector2[] PapyrusWalkPositions;

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

    private void Start()
    {
        DarkRoomObjects.SetActive(value: true);
        LightRoomObjects.SetActive(value: false);
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
            Susie.transform.position = new Vector2(3.65f, -8.5f);
        }
        else
        {
            Debug.LogError("Susie is not in the party?????");
        }
        PlayerManager.Instance._PAnimation.FootstepsEnabled = true;
        IncrementCutsceneIndex();
        yield return new WaitForSeconds(0.5f);
        while ((Vector2)Kris.transform.position != KrisWalkPositions[0] || (Vector2)Susie.transform.position != SusieWalkPositions[0])
        {
            yield return null;
            if ((Vector2)Kris.transform.position != KrisWalkPositions[0])
            {
                Kris.transform.position = Vector2.MoveTowards(Kris.transform.position, KrisWalkPositions[0], 3f * Time.deltaTime);
                Kris._PMove.AnimationOverriden = true;
                Kris._PMove._anim.SetBool("MOVING", value: true);
                Kris._PMove._anim.SetFloat("MOVEMENTX", KrisWalkDirections[0].x);
                Kris._PMove._anim.SetFloat("MOVEMENTY", KrisWalkDirections[0].y);
            }
            else
            {
                Kris._PMove._anim.SetBool("MOVING", value: false);
                Kris._PMove._anim.SetFloat("MOVEMENTX", -1f);
                Kris._PMove._anim.SetFloat("MOVEMENTY", 0f);
                PlayerManager.Instance._PAnimation.FootstepsEnabled = false;
            }
            if ((Vector2)Susie.transform.position != SusieWalkPositions[0])
            {
                Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, SusieWalkPositions[0], 3f * Time.deltaTime);
                Susie.SusieAnimator.Play("Walk");
                Susie.SusieAnimator.SetFloat("VelocityX", SusieWalkDirections[0].x);
                Susie.SusieAnimator.SetFloat("VelocityY", SusieWalkDirections[0].y);
                Susie.SusieAnimator.SetFloat("VelocityMagnitude", 1f);
            }
            else
            {
                Susie.SusieAnimator.Play("IdleSad");
                Susie.SusieAnimator.SetFloat("VelocityX", -1f);
                Susie.SusieAnimator.SetFloat("VelocityY", 0f);
                Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
            }
        }
        Kris._PMove._anim.SetBool("MOVING", value: false);
        Kris._PMove._anim.SetFloat("MOVEMENTX", -1f);
        Kris._PMove._anim.SetFloat("MOVEMENTY", 0f);
        Susie.SusieAnimator.Play("IdleSad");
        Susie.SusieAnimator.SetFloat("VelocityX", -1f);
        Susie.SusieAnimator.SetFloat("VelocityY", 0f);
        Susie.SusieAnimator.SetFloat("VelocityMagnitude", 0f);
        MonoBehaviour.print("done");
        yield return new WaitForSeconds(1f);
        RunFreshChat(CutsceneChats[0], 0, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 2)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        CutsceneSource.PlayOneShot(CutsceneSounds[0]);
        PapyrusAnimator.Play("Papyrus_Knight_FadeIn");
        Candle.SetActive(value: true);
        TableLighting.SetActive(value: true);
        yield return new WaitForSeconds(3f);
        RunFreshChat(CutsceneChats[0], 1, ForcePosition: true, OnBottom: true);
        while (CutsceneIndex != 3)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        CutsceneSource.PlayOneShot(CutsceneSounds[1]);
        DarkRoomObjects.SetActive(value: false);
        PapyrusAnimator.Play("Papyrus_HoldPaper");
        PapyrusArmorAnimator.Play("MP_PapyrusRoom_PapyrusArmor_Paper");
        LightRoomObjects.SetActive(value: true);
        Sans.gameObject.SetActive(value: true);
        yield return new WaitForSeconds(2.5f);
        RunFreshChat(CutsceneChats[1], 0, ForcePosition: true, OnBottom: true);
        while (CutsceneIndex != 4)
        {
            yield return null;
        }
        PapyrusAnimator.Play("IdleFace");
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.right);
        PapyrusArmorAnimator.Play("MP_PapyrusRoom_PapyrusArmor_Right");
        yield return new WaitForSeconds(1f);
        RunFreshChat(CutsceneChats[1], 1, ForcePosition: true, OnBottom: true);
        while (CutsceneIndex != 5)
        {
            yield return null;
        }
        CutsceneSource.PlayOneShot(CutsceneSounds[2]);
        PapyrusAnimator.Play("Papyrus_HandsUp_Down");
        PapyrusArmorAnimator.Play("MP_PapyrusRoom_PapyrusArmor_Down");
        SPR_YSorting component = PapyrusArmorAnimator.GetComponent<SPR_YSorting>();
        component.AutomaticRealtimeSorting = false;
        component.SPR.sortingOrder = 999;
        yield return new WaitForSeconds(1f);
        CutsceneSource.PlayOneShot(CutsceneSounds[3]);
        while (PapyrusArmorAnimator.transform.position.y < 5f)
        {
            yield return null;
            PapyrusArmorAnimator.transform.position = Vector2.MoveTowards(PapyrusArmorAnimator.transform.position, (Vector2)PapyrusArmorAnimator.transform.position + Vector2.up * 6f, 7.5f * Time.fixedDeltaTime * Time.timeScale);
        }
        PapyrusArmorAnimator.GetComponent<SpriteRenderer>().enabled = false;
        int papyrusWalkIndex = 0;
        while ((Vector2)PapyrusTransform.position != PapyrusWalkPositions[1])
        {
            yield return null;
            if ((Vector2)PapyrusTransform.position != PapyrusWalkPositions[papyrusWalkIndex])
            {
                PapyrusAnimator.Play("Walk");
                PapyrusAnimator.SetFloat("MOVEMENTX", PapyrusWalkDirections[papyrusWalkIndex].x);
                PapyrusAnimator.SetFloat("MOVEMENTY", PapyrusWalkDirections[papyrusWalkIndex].y);
                PapyrusTransform.position = Vector2.MoveTowards(PapyrusTransform.position, PapyrusWalkPositions[papyrusWalkIndex], 4f * Time.deltaTime);
            }
            else
            {
                papyrusWalkIndex++;
            }
        }
        Susie.SusieAnimator.GetComponent<SpriteRenderer>().enabled = false;
        PapyrusAnimator.Play("Papyrus_Handshake_Susie");
        RunFreshChat(CutsceneChats[2], 0, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 6)
        {
            yield return null;
        }
        Susie.SusieAnimator.GetComponent<SpriteRenderer>().enabled = true;
        PapyrusAnimator.Play("Walk");
        while ((Vector2)PapyrusTransform.position != PapyrusWalkPositions[3])
        {
            yield return null;
            if ((Vector2)PapyrusTransform.position != PapyrusWalkPositions[papyrusWalkIndex])
            {
                PapyrusAnimator.Play("Walk");
                PapyrusAnimator.SetFloat("MOVEMENTX", PapyrusWalkDirections[papyrusWalkIndex].x);
                PapyrusAnimator.SetFloat("MOVEMENTY", PapyrusWalkDirections[papyrusWalkIndex].y);
                PapyrusTransform.position = Vector2.MoveTowards(PapyrusTransform.position, PapyrusWalkPositions[papyrusWalkIndex], 4f * Time.deltaTime * Time.timeScale);
            }
            else
            {
                papyrusWalkIndex++;
            }
        }
        Susie.RotateSusieToDirection(Vector2.down);
        Susie.SusieAnimator.Play("Idle");
        Kris._PMove._anim.GetComponent<SpriteRenderer>().enabled = false;
        PapyrusAnimator.Play("Papyrus_Handshake_Kris");
        RunFreshChat(CutsceneChats[2], 1, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 7)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        Kris._PMove._anim.GetComponent<SpriteRenderer>().enabled = true;
        Papyrus_Idle_Up();
        Susie.RotateSusieToDirection(Vector2.up);
        Kris._PMove.RotatePlayerAnim(Vector2.up);
        RunFreshChat(CutsceneChats[3], 0, ForcePosition: true, OnBottom: true);
        while (CutsceneIndex != 8)
        {
            yield return null;
        }
        CutsceneIndex = 0;
        RoomTourCutscene.StartCutscene();
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
        PapyrusAnimator.Play("IdleFace");
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.up);
        CutsceneUtils.RotateCharacterToDirection(PapyrusAngryEyebrows, "MOVEMENTX", "MOVEMENTY", Vector2.up);
        float normalizedTime = PapyrusArmorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        PapyrusArmorAnimator.Play("MP_PapyrusRoom_PapyrusArmor_Up", -1, normalizedTime);
    }

    public void Papyrus_Idle_Right()
    {
        PapyrusAnimator.Play("IdleFace");
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.right);
        CutsceneUtils.RotateCharacterToDirection(PapyrusAngryEyebrows, "MOVEMENTX", "MOVEMENTY", Vector2.right);
        float normalizedTime = PapyrusArmorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        PapyrusArmorAnimator.Play("MP_PapyrusRoom_PapyrusArmor_Right", -1, normalizedTime);
    }

    public void Papyrus_Idle_Left()
    {
        PapyrusAnimator.Play("IdleFace");
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.left);
        CutsceneUtils.RotateCharacterToDirection(PapyrusAngryEyebrows, "MOVEMENTX", "MOVEMENTY", Vector2.left);
        float normalizedTime = PapyrusArmorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        PapyrusArmorAnimator.Play("MP_PapyrusRoom_PapyrusArmor_Left", -1, normalizedTime);
    }

    public void Papyrus_Idle_Down()
    {
        PapyrusAnimator.Play("IdleFace");
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(PapyrusAngryEyebrows, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        float normalizedTime = PapyrusArmorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        PapyrusArmorAnimator.Play("MP_PapyrusRoom_PapyrusArmor_Down", -1, normalizedTime);
    }

    public void Papyrus_Proud()
    {
        PapyrusAnimator.Play("Papyrus_Praise", -1, 0f);
        PapyrusArmorAnimator.Play("MP_PapyrusRoom_PapyrusArmor_Praise", -1, 0f);
    }

    public void Papyrus_EnableAngryEyebrows()
    {
        PapyrusAngryEyebrows.gameObject.SetActive(value: true);
        CutsceneUtils.RotateCharacterToDirection(PapyrusAngryEyebrows, "MOVEMENTX", "MOVEMENTY", new Vector2(PapyrusAnimator.GetFloat("MOVEMENTX"), PapyrusAnimator.GetFloat("MOVEMENTY")));
    }

    public void Papyrus_DisableAngryEyebrows()
    {
        PapyrusAngryEyebrows.gameObject.SetActive(value: false);
    }
}
